using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{

	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText;
	private int score;

	public GUIText restartText;
	public GUIText gameOverText;
	private bool gameOver;
	private bool restart;


	void Start()
	{
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine(SpawnWaves ());

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

		if (restart) {
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Application.LoadLevel(Application.loadedLevel);
			}

		}
	}



	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);
		while(true) {
			for (int i = 0; i < hazardCount;i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);
			if (gameOver)
			{
				restartText.text = "Tap to Restart";
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();

	}

	void UpdateScore()
	{
		scoreText.text = "Score: " + score;

	}

	public void GameOver() 
	{
		gameOverText.text = "Game Over";
		gameOver = true;

	}



}
