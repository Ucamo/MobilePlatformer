using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	float speed=15f;
	bool isRight;
	bool isLeft;
	bool isUp;
	bool isDown;
	public GameObject Player;
	void Update()
	{
		HandleMovement ();
	}

	void HandleMovement()
	{
		if (isRight) {
			MoveRight ();
		}
		if (isLeft) {
			MoveLeft ();
		}
		if (isDown) {
			MoveDown ();
		}
		if (isUp) {
			MoveUp ();
		}
	}
	void MoveRight()
	{
		Player.transform.position += Vector3.right * speed * Time.deltaTime;
	}
	void MoveLeft()
	{
		Player.transform.position += Vector3.left * speed * Time.deltaTime;
	}
	void MoveUp()
	{
		Player.transform.position += Vector3.up * speed * Time.deltaTime;
	}
	void MoveDown()
	{
		Player.transform.position += Vector3.down * speed * Time.deltaTime;
	}

	public void setRight()
	{
		isRight = true;
	}
	public void setLeft()
	{
		isLeft = true;
	}
	public void setDown()
	{
		isDown = true;
	}
	public void setUp()
	{
		isUp = true;
	}

	public void ResetMovement()
	{
		isRight=false;
		isLeft = false;
		isDown = false;
		isUp = false;
	}
}
