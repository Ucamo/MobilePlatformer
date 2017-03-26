using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

	public GameObject prefab;
	public float speed;
	public bool isBoss;
	float publicSpeed;
	int modificador;
	GameObject objToProtect;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetSpeed ();
		CheckGameObjectType ();
		CheckHeight ();
	}

	void CheckHeight()
	{
		if (gameObject.transform.position.y < -17) {
			Destroy (gameObject);
		}
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
			objToProtect = gc [0].GetComponent<GameController> ().GetObjProtected ();
		}
	}

	void CheckGameObjectType()
	{
		if (isBoss) {
			if (objToProtect != null) {
				if (prefab.transform.position.y >= objToProtect.transform.position.y - 5) {
					//can't get higher
				} else {
					Move ();
				}
			}
		} else {
			Move ();
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
