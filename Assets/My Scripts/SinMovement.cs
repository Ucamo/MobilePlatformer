using UnityEngine;
using System.Collections;

public class SinMovement : MonoBehaviour {

	private Vector3 _startPosition;
	// Use this for initialization
	void Start () {
		_startPosition = transform.position;
	}

	void Update()
	{
		transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time)*2, 0.0f, 0.0f);
	}
}
