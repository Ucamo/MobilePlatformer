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
			transform.position = _startPosition + new Vector3 (Mathf.Sin (Time.time) * 2, 0.0f, 0.0f);
		} else {
			GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
			if (gc != null) {
				bool win = gc [0].GetComponent<GameController> ().getWin ();
				if (!win) {
					transform.position = _startPosition + new Vector3 (Mathf.Sin (Time.time) * 2, 0.0f, 0.0f);
				}
			}
		}

	}
}
