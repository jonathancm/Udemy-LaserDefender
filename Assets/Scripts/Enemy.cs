using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Configuration Parameters
	[Header("Enemy Stats")]
	[SerializeField] float health = 100;
	[SerializeField] int scoreValue = 100;
	[SerializeField] float hitFlashPeriod = 0.05f;

	[Header("Projectile")]
	[SerializeField] float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] GameObject laserPrefab = null;
	[SerializeField] float projectileSpeed = 15f;

	[Header("Special Effects")]
	[SerializeField] GameObject deathVFX = null;
	[SerializeField] float deathVFXDuration = 1f;
	[SerializeField] AudioClip deathSFX = null;
	[SerializeField] [Range(0,1)] float deathVolume = 0.5f;
	[SerializeField] AudioClip laserSFX = null;
	[SerializeField] [Range(0, 1)] float laserVolume = 0.5f;

	// Cached References
	GameSession gameSession;
	SpriteRenderer spriteRenderer;
	Color originalSpriteColor;

	// Use this for initialization
	void Start () {
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		gameSession = FindObjectOfType<GameSession>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalSpriteColor = spriteRenderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		CountDownAndShoot();
	}

	private void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime;
		if(shotCounter <= 0f)
		{
			Fire();
			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		}
	}

	private void Fire()
	{
		GameObject laser = Instantiate(
				laserPrefab,
				transform.position,
				Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
		AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position,laserVolume);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
		if (!damageDealer) { return; }
		ProcessHit(damageDealer);
	}

	private void ProcessHit(DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		damageDealer.Hit();

		if(health <= 0)
		{
			Die();
		}
		else
		{
			StartCoroutine(ColorFlash());
		}
	}

	IEnumerator ColorFlash()
	{
		Color flashColor = new Color(1, 0.5f, 0.5f);

		spriteRenderer.color = flashColor;
		yield return new WaitForSeconds(hitFlashPeriod);
		spriteRenderer.color = originalSpriteColor;
	}

	private void Die()
	{
		PlayDeathVFX();
		PlayDeathSFX();

		gameSession.AddScore(scoreValue);
		Destroy(gameObject);
	}

	private void PlayDeathVFX()
	{
		GameObject explosion = Instantiate(
					deathVFX,
					transform.position,
					Quaternion.identity);
		Destroy(explosion, deathVFXDuration);
	}

	private void PlayDeathSFX()
	{
		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position,deathVolume);
	}
}
