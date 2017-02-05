using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float speed=15f;
	public float jumpForce=450;
	bool isRight;
	bool isLeft;
	bool isUp;
	bool isDown;
	public GameObject Player;
	public bool grounded;
	bool gameOver;
	bool canDoublejump;
	int jumps;

	void Start()
	{
		gameOver = false;
		jumps = 0;

	}
	void Update()
	{
		HandleMovement ();
		KeyBoardMovement ();
	}

	void KeyBoardMovement(){
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			setLeft ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			setRight ();
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Jump ();
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			ResetMovement ();
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			ResetMovement ();
		}


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
			Jump ();
		}
		grounded = Player.GetComponent<PlayerController>().getGrounded();
		if (grounded) {
			canDoublejump = false;
			jumps = 0;
		}
	}

	void Jump()
	{

		if (grounded && !gameOver) {
			Player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
			Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
			canDoublejump = true;
			grounded = false;
			Player.GetComponent<PlayerController> ().setGrounded (grounded);
		} 

	}

	public void countJumps()
	{
		jumps++;
		if (jumps > 1) {
				Player.GetComponent<Rigidbody2D> ().velocity = new Vector2(Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
				Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				canDoublejump = false;
			jumps = 0;
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
