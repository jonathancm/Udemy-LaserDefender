using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour {

	// Cached References
	GameSession gameSession;
	TextMeshProUGUI scoreText;

	// Use this for initialization
	void Start () {
		gameSession = FindObjectOfType<GameSession>();
		scoreText = GetComponent<TextMeshProUGUI>();

		UpdateDisplay();
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		if(gameSession)
		{
			scoreText.text = gameSession.GetScore().ToString();
		}
		else
		{
			scoreText.text = "0";
		}
	}
}
