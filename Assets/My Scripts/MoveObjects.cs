using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

	public GameObject prefab;
	public float speed;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Move ();
	}


	void Move()
	{
		prefab.transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime);
	}
}
