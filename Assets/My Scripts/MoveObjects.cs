using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

	public GameObject prefab;
	public float speed;
	float publicSpeed;
	int modificador;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetSpeed ();
		Move ();
	}

	void GetSpeed()
	{
		modificador = 1;
		if (speed > 0)
			modificador = -1;
		else
			modificador = 1;
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc != null) {
			publicSpeed = gc [0].GetComponent<GameController> ().getPublicSpeed();
		}
	}


	void Move()
	{
		if (speed < 0) {
			prefab.transform.Translate(new Vector3(0, modificador*(publicSpeed-speed), 0) * Time.deltaTime);
		} else {
			prefab.transform.Translate(new Vector3(0, modificador*(publicSpeed+speed), 0) * Time.deltaTime);
		}

	}
}
