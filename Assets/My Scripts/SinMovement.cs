using UnityEngine;
using System.Collections;

public class SinMovement : MonoBehaviour {

	private Vector3 _startPosition;
	public bool isTarget;

	// Use this for initialization
	void Start () {
		_startPosition = transform.position;
	}

	void Update()
	{
		if (!isTarget) {
			Move();
		} else {
			GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
			if (gc != null) {
				bool win = gc [0].GetComponent<GameController> ().getWin ();
				bool bossDefeated = gc [0].GetComponent<GameController> ().getBossDefeated();
				if (!win) {
					Move();
				} else {
					if (!bossDefeated) {
						Move();
					}
				}
			}
		}
	}

	void Move()
	{
		transform.position = _startPosition + new Vector3 (Mathf.Sin (Time.time) * 2, 0.0f, 0.0f);
	}
}
