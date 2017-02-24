using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

	public GameObject projectile;
	public Text txtCoins; 
	public Text txtScore;
	public Text txtMana;
	public int currentMana;
	public int maxMana;
	public float bulletSpeed;
	public float speed=15f;
	public float jumpForce=450;
	bool isRight;
	bool isLeft;
	bool isUp;
	bool isDown;
	public GameObject Player;
	public GameObject manaPotion;
	public bool grounded;
	public bool walled;
	public float publicSpeed;
	bool gameOver;
	bool canDoublejump;
	int jumps;

	int score;

	int coins;


	void Start()
	{
		gameOver = false;
		jumps = 0;
		coins = 0;

	}
	void Update()
	{
		HandleMovement ();
		KeyBoardMovement ();
		DrawUI ();
		walled = Player.GetComponent<PlayerController> ().getWalled();
	}

	public float getPublicSpeed()
	{
		return publicSpeed;
	}

	public void increasePublicSpeed(float val)
	{
		publicSpeed += val;
	}

	public void decreasePublicSpeed(float val)
	{
		publicSpeed -= val;
	}

	void DrawUI()
	{
		txtCoins.text = "Coins: " + coins;
		txtScore.text = "Score: " + score;
		txtMana.text = "Mana: " + currentMana + "/" + maxMana;
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
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			setDown();
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			ResetMovement ();
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			ResetMovement ();
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			ResetMovement ();
		}


	}

	public void IncreaseMana(int val)
	{
		if (currentMana + val <= maxMana) {
			currentMana += val;
		} else {
			currentMana = maxMana;
		}
	}

	public void DecreaseMana(int val)
	{
		if (currentMana - val >= 0) {
			currentMana -= val;
		} else {
			currentMana = 0;
		}
	}

	public void IncreaseCoin()
	{
		coins++;
	}

	public int getCoins()
	{
		return coins;
	}

	public void IncreaseScore(int value)
	{
		score += value;
	}

	void HandleMovement()
	{
		if (isRight) {
			MoveRight (1);
		}
		if (isLeft) {
			MoveLeft (-1);
		}
		if (isDown) {
			Shoot ();
		}
		if (isUp) {
			Jump ();
			isUp = false;
		}
		grounded = Player.GetComponent<PlayerController>().getGrounded();
		if (grounded) {
			canDoublejump = false;
			jumps = 0;
		}
	}

	void Shoot()
	{
		Vector2 forceVector = Vector2.down;
		Vector3 firePosition = new Vector3(Player.transform.position.x, Player.transform.position.y, -1);
		GameObject bPrefab = Instantiate(projectile, firePosition, Quaternion.identity) as GameObject;
		if (currentMana > 0) {
			GameObject secondBullet = Instantiate(projectile, new Vector3(firePosition.x+0.7f,firePosition.y,firePosition.z), Quaternion.identity) as GameObject;
			secondBullet.layer = LayerMask.NameToLayer ("Projectile");
			Vector3 newForce  = Vector2.left;
			secondBullet.GetComponent<Rigidbody2D>().AddForce(newForce* bulletSpeed);
			GameObject thirdBullet = Instantiate(projectile, new Vector3(firePosition.x-0.7f,firePosition.y,firePosition.z), Quaternion.identity) as GameObject;
			thirdBullet.layer = LayerMask.NameToLayer ("Projectile");
			Vector2 newForce2 = Vector2.right;
			thirdBullet.GetComponent<Rigidbody2D>().AddForce(newForce2* bulletSpeed);
			DecreaseMana (1);
		}
		bPrefab.layer = LayerMask.NameToLayer ("Projectile");
		bPrefab.GetComponent<Rigidbody2D>().AddForce(forceVector * bulletSpeed);
		isDown = false;
	}

	public void ProtectedDropItem()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected [0];
			Vector3 firePosition = new Vector3(objProtected.transform.position.x, objProtected.transform.position.y, 0);
			GameObject bPrefab = Instantiate(manaPotion, firePosition, Quaternion.identity) as GameObject;
		}
	}

	void Jump()
	{
		if (Player.transform.position.y <= 6.24) {
			if (grounded && !gameOver) {
				Player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
				Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				canDoublejump = true;
				grounded = false;
				Player.GetComponent<PlayerController> ().setGrounded (grounded);
			} 
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


	void MoveRight(float push)
	{
		if(Player.transform.position.x<2.24)
			Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
	}
	void MoveLeft(float push)
	{
		if(Player.transform.position.x>-2.24)
			Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
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
