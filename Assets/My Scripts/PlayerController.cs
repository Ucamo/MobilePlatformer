using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool grounded;
	public GameObject CharacterController;

	void Start () {
	
	}

	void Update () {
		
	}

	public bool getGrounded()
	{
		return grounded;
	}

	public void setGrounded(bool gro)
	{
		grounded = gro;
	}


	//Handle ground contact
	void OnCollisionEnter2D(Collision2D coll)
	{
		//Contact with ground
		if (coll.gameObject.tag == "Ground")
		{
			grounded = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			grounded = true;
		}

	}
}
