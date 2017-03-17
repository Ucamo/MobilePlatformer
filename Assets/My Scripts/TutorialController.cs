using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

	// Use this for initialization
	public int coinsToWin;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckWin ();
	}

	void CheckWin()
	{
		GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		int coinsEarned = gc [0].GetComponent<GameController> ().getCoins ();
		int score = gc [0].GetComponent<GameController> ().getScore ();
		if (coins.Length > 0) {
			if (coinsEarned >= coinsToWin && score>0) {
				
				if (gc != null) {
					gc [0].GetComponent<GameController> ().setWin(true);
					gc [0].GetComponent<GameController> ().PlayLevelWarp();
					gc [0].GetComponent<GameController> ().SetBossDefeated(true);
				}
			}
		}
	}
}
