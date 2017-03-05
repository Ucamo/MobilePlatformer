using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public float speed;

	void Start () {
	
	}

	void Update () {
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * speed);
	}
}
