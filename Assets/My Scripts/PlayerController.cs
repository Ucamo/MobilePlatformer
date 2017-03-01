using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool grounded;
	public bool walled;
	public GameObject GameController;

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

	public bool getWalled(){
		return walled;
	}

	public void setWalled(bool wal)
	{
		walled = wal;
	}


	//Handle ground contact
	void OnCollisionEnter2D(Collision2D coll)
	{
		//Contact with ground
		if (coll.gameObject.tag == "Ground")
		{
			grounded = true;
			walled = false;
		}
		if (coll.gameObject.tag == "Coin")
		{
			GameController.GetComponent<GameController>().IncreaseCoin();
			Destroy (coll.gameObject);
		}
		if (coll.gameObject.tag == "ManaPotion")
		{
			GameController.GetComponent<GameController>().IncreaseMana(3);
			Destroy (coll.gameObject);
		}
		if (coll.gameObject.tag == "1Up")
		{
			GameController.GetComponent<GameController>().IncreaseLives();
			Destroy (coll.gameObject);
		}
		if (coll.gameObject.tag == "Item")
		{
			GameController.GetComponent<GameController>().AddItemToInventory(coll.gameObject);
		}
		if (coll.gameObject.tag == "Wall") {
			walled = true;
		}else {
			walled = false;
		}
	}
		

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			grounded = true;
			walled = false;
		}
		if (other.gameObject.tag == "Coin")
		{
			GameController.GetComponent<GameController>().IncreaseCoin();
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "ManaPotion")
		{
			GameController.GetComponent<GameController>().IncreaseMana(3);
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "1Up")
		{
			GameController.GetComponent<GameController>().IncreaseLives();
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "Item")
		{
			GameController.GetComponent<GameController>().AddItemToInventory(other.gameObject);
		}
		if (other.gameObject.tag == "Wall") {
			walled = true;
		} else {
			walled = false;
		}
	}
}
