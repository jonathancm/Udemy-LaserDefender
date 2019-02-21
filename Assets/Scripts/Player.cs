using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Configurable Parameters
	[Header("Player Stats")]
	[SerializeField] int health = 200;
	[SerializeField] float hitFlashPeriod = 0.3f;

	[Header("Player Movement")]
	[SerializeField] float moveSpeedX = 15f;
	[SerializeField] float moveSpeedY = 20f;
	[SerializeField] float padding = 1f;

	[Header("Projectile")]
	[SerializeField] GameObject laserPrefab = null;
	[SerializeField] float projectileSpeed = 15f;
	[SerializeField] float projectileFiringPeriod = 0.1f;

	[Header("Special Effects")]
	[SerializeField] GameObject deathVFX = null;
	[SerializeField] float deathVFXDuration = 1f;
	[SerializeField] AudioClip deathSFX = null;
	[SerializeField] [Range(0, 1)] float deathVolume = 0.5f;
	[SerializeField] AudioClip laserSFX = null;
	[SerializeField] [Range(0, 1)] float laserVolume = 0.5f;

	// Cached references
	Level level;
	SpriteRenderer spriteRenderer;
	Color originalSpriteColor;

	// Internal Variables
	Coroutine firingCoroutine;
	float xMin;
	float xMax;
	float yMin;
	float yMax;

	// Use this for initialization
	void Start () {
		SetupMoveBoundaries();
		level = FindObjectOfType<Level>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalSpriteColor = spriteRenderer.color;
	}

	private void SetupMoveBoundaries()
	{
		Camera gameCamera = Camera.main;
		xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
		xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
		yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
		yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Fire();
	}

	private void Fire()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			firingCoroutine = StartCoroutine(FireContinuously());
		}
		if (Input.GetButtonUp("Fire1"))
		{
			StopCoroutine(firingCoroutine);
		}
	}

	IEnumerator FireContinuously()
	{
		while (true)
		{
			GameObject laser = Instantiate(
				laserPrefab,
				transform.position,
				Quaternion.identity) as GameObject;
			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
			AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserVolume);

			yield return new WaitForSeconds(projectileFiringPeriod);
		}
	}

	private void Move()
	{
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedX;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedY;

		var newPosX = transform.position.x + deltaX;
		var newPosY = transform.position.y + deltaY;

		newPosX = Mathf.Clamp(newPosX,xMin,xMax);
		newPosY = Mathf.Clamp(newPosY, yMin, yMax);

		transform.position = new Vector2(newPosX, newPosY);
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
			health = 0;
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

		Destroy(gameObject);
		level.LoadGameOver();
	}
	
	public int GetHealth()
	{
		return health;
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
		AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
	}
}
