using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

	// Cached References
	Player player;
	TextMeshProUGUI healthText;

	// Use this for initialization
	void Start()
	{
		player = FindObjectOfType<Player>();
		healthText = GetComponent<TextMeshProUGUI>();

		healthText.text = player.GetHealth().ToString();
	}

	// Update is called once per frame
	void Update()
	{
		healthText.text = player.GetHealth().ToString();
	}
}
