using UnityEngine;
using System.Collections;
using Thalmic.Myo;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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

    public GameObject myo = null;

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
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

		if (restart) {
			if (thalmicMyo.pose == Pose.Fist)
			{
				SceneManager.LoadScene("Main");
                //ExtendUnlockAndNotifyUserAction(thalmicMyo);
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

    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }

}
