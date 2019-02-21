using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

	// Configurable Paramters
	[SerializeField] float delayInSeconds = 2;

	private void CleanUpGame()
	{
		GameSession gameSession = FindObjectOfType<GameSession>();
		if(gameSession)
		{
			gameSession.ResetGame();
		}
	}

	private void StopGame()
	{
		GameSession gameSession = FindObjectOfType<GameSession>();
		if(gameSession)
		{
			gameSession.SetGameOver();
		}
	}

	public void LoadMainMenu()
	{
		CleanUpGame();
		SceneManager.LoadScene("MainMenu");
	}

	public void LoadCoreGame()
	{
		CleanUpGame();
		SceneManager.LoadScene("CoreGame");
	}

	public void LoadGameOver()
	{
		StartCoroutine(WaitAndLoad());
	}

	IEnumerator WaitAndLoad()
	{
		yield return new WaitForSeconds(delayInSeconds);
		StopGame();
		SceneManager.LoadScene("GameOver");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
