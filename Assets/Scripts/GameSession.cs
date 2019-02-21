using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour {

	// Configuration Parameters
	[SerializeField] bool isAutoPlayEnabled = false;

	// State Variables
	public static GameSession instance = null; //Static instance of GameSession
	int score = 0;
	bool gamePaused = false;
	bool gameOver = false;

	private void Awake()
	{
		SetupSingleton();
	}

	private void SetupSingleton()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			PauseGame();
	}


	public int GetScore() { return score; }

	public void AddScore(int scoreValue) { score += scoreValue; }

	public bool IsAutoPlayEnabled() { return isAutoPlayEnabled; }

	public void SetGameOver() { gameOver = true; }


	public void PauseGame()
	{
		GameObject pauseCanvas = transform.Find("Game Pause Canvas").gameObject;

		if(!pauseCanvas)
			return;

		if(gameOver)
			return;

		if(gamePaused)
		{
			pauseCanvas.SetActive(false);
			Time.timeScale = 1;
			gamePaused = false;
		}
		else
		{
			pauseCanvas.SetActive(true);
			Time.timeScale = 0;
			gamePaused = true;
		}
	}

	public void ResetGame()
	{
		Time.timeScale = 1;
		Destroy(gameObject);
	}
}
