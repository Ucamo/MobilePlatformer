using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

	public GameObject projectile;
	public Text txtCoins; 
	public Text txtScore;
	public Text txtMana;
	public Text txtLives;
	public Button btnItem;
	public int currentMana;
	public int maxMana;
	public int healthOfProtected;
	public int maxHealthOfProtected;
	public float bulletSpeed;
	public float speed=15f;
	public float jumpForce=450;
	bool isRight;
	bool isLeft;
	bool isUp;
	bool isDown;
	public GameObject Player;
	public GameObject[] normalItems;
	public GameObject[] rareItems;
	public bool grounded;
	public bool walled;
	public float publicSpeed;
	bool gameOver;
	bool canDoublejump;
	int jumps;
	int score;
	int coins;

	bool lose;


	public Canvas CanvasGameOver;
	public Canvas CanvasWin;
	public int lives;
	public int levelScoreGoal;
	public string nextSceneName;

	bool win;
	bool goingToNextLevel;
	bool readyNextLevel;

	void Start()
	{
		gameOver = false;
		jumps = 0;
		coins = 0;
		HideHealthBarProtected ();
	}
	void Update()
	{
		if (Player != null) {
			if (!goingToNextLevel) {
				HandleMovement ();
				KeyBoardMovement ();
			}
			walled = Player.GetComponent<PlayerController> ().getWalled();
			CheckEvents ();
		}
		DrawUI ();
	}

	public void CheckEvents()
	{
		if (win) {
			PlayLevelWarp ();
		}
		if (goingToNextLevel) {
			PlayGoingToNextLevel ();
		}
		if (readyNextLevel) {
			ShowCanvasWin ();
		}
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
		txtLives.text = "Lives: " + getLives ();
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
			CallMana ("+"+val.ToString());
			currentMana += val;
		} else {
			CallMana ("MAX");
			currentMana = maxMana;
		}
	}

	void CallMana(string value)
	{
		if (Player != null) {
			Vector3 firePosition = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, 0);
			GameObject damage = (GameObject)Resources.Load ("Damage");
			GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
			Color blue = new Color (0,0,1);
			bPrefab.GetComponent<DamageController> ().CreateBonusColor (value,blue);
		}
	}

	void CallDamageProtected(string value)
	{
		if (Player != null) {
			GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
			if (objectProtected != null) {
				GameObject objProtected = objectProtected [0];
				Vector3 firePosition = new Vector3(objProtected.gameObject.transform.position.x, objProtected.gameObject.transform.position.y, 0);
				GameObject damage = (GameObject)Resources.Load ("Damage");
				GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
				Color red = new Color (1,0,0);
				bPrefab.GetComponent<DamageController> ().CreateBonusColor (value,red);
			}

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
		CheckScore ();
	}

	public int getScore()
	{
		return score;
	}

	public int getMaxHealthOfProtected()
	{
		return maxHealthOfProtected;
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
			if(!lose)
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
		if (Player.GetComponent<Rigidbody2D> () != null) {
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
	}

	public void ProtectedDropItem()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected [0];
			Vector3 firePosition = new Vector3(objProtected.transform.position.x, objProtected.transform.position.y, 0);
			int index = Random.Range (0, normalItems.Length);
			GameObject randomItem = normalItems [index];
			GameObject bPrefab = Instantiate(randomItem, firePosition, Quaternion.identity) as GameObject;
		}
	}

	public void ProtectedDropRareItem()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected [0];
			Vector3 firePosition = new Vector3(objProtected.transform.position.x, objProtected.transform.position.y, 0);
			int index = Random.Range (0, rareItems.Length);
			GameObject randomItem = rareItems [index];
			GameObject bPrefab = Instantiate(randomItem, firePosition, Quaternion.identity) as GameObject;
		}
	}

	public void DecreaseHealthOfProtected(int val)
	{
		healthOfProtected -= val;
		CallDamageProtected ("-"+val.ToString ());
		ShowHealthBarProtected();
		if (healthOfProtected <= 0) {
			GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
			if (objectProtected != null) {
				Destroy (objectProtected [0]);

			}
			ShowCanvasGameOver ();
		}
	}

	public int getHealthOfProtected()
	{
		return healthOfProtected;
	}

	public void IncreaseHealthOfProtected(int val)
	{
		healthOfProtected += val;
		ShowHealthBarProtected();
	}

	public void ShowHealthBarProtected()
	{
		StartCoroutine(showHealth());
	}

	IEnumerator showHealth ()
	{ 
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected [0];
			GameObject healthBar = objProtected.transform.Find("HealthBar").gameObject;
			healthBar.SetActive (true);
			yield return new WaitForSeconds(1);
			HideHealthBarProtected ();
		}
	}

	public void HideHealthBarProtected()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected [0];
			GameObject healthBar = objProtected.transform.Find("HealthBar").gameObject;
			healthBar.SetActive (false);
		}
	}

	public void DestroyAllEnemies()
	{
		GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemyArray != null) {
			foreach (GameObject objEnemy in enemyArray) {
				if (objEnemy.transform.position.y >= -3.5) {
					int enemyHealth = objEnemy.GetComponent<EnemyController> ().GetHealth ();
					for (int x = 0; x <= enemyHealth; x++) {
						objEnemy.GetComponent<EnemyController> ().DecreaseHealth ();
					}
				}
			}
		}
		//Reset item slot
		ResetItemSlot();
	}

	void ResetItemSlot()
	{
		ActiveItemSlot (false);
		btnItem.GetComponent<Button> ().onClick.RemoveAllListeners ();
	}

	void ActiveItemSlot(bool value)
	{
		btnItem.GetComponent<Image> ().enabled = value;
		btnItem.GetComponent<Button> ().enabled = value;
		btnItem.enabled = value;
		btnItem.interactable = value;
	}

	public void AddItemToInventory(GameObject item)
	{
		ResetItemSlot ();
		ActiveItemSlot (true);
		Sprite itemSprite = item.GetComponent<SpriteRenderer> ().sprite;
		Color itemColor = item.GetComponent<SpriteRenderer> ().color;
		btnItem.GetComponent<Image>().sprite = itemSprite;
		btnItem.GetComponent<Image> ().color = itemColor;
		if (item.name.Contains("Bomb")) {
			btnItem.GetComponent<Button>().onClick.AddListener(() => DestroyAllEnemies());
		}

		Destroy (item);
	}
		

	void Jump()
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
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
	}

	public void countJumps()
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			jumps++;
			if (jumps > 1) {
				Player.GetComponent<Rigidbody2D> ().velocity = new Vector2(Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
				Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				canDoublejump = false;
				jumps = 0;
			}
		}
	}


	void MoveRight(float push)
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			if(Player.transform.position.x<2.24)
				Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
		}
	}
	void MoveLeft(float push)
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			if(Player.transform.position.x>-2.24)
				Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
		}
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

	public void ShowCanvasGameOver()
	{
		CanvasGameOver.gameObject.SetActive (true);
		StopWorld ();
	}

	public void StopWorld()
	{
		decreasePublicSpeed (2);
		Destroy (Player.GetComponent<Rigidbody2D> ());
		lose = true;
	}

	public void ShowCanvasWin()
	{
		CanvasWin.gameObject.SetActive (true);
	}

	public void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void IncreaseLives()
	{
		lives++;
	}

	public void decreaseLives(){
		lives--;
		checkPlayerDeath ();
	}

	public int getLives(){
		return lives;
	}

	public void checkPlayerDeath()
	{
		if (lives <= 0) {
			ShowCanvasGameOver ();
		} else {
			//Spawn character
			RespawnPlayer();
		}
	}

	public void RespawnPlayer()
	{
		Vector3 spawnPosition = new Vector3(0, 7, 0);
		GameObject bPrefab = Instantiate(Player, spawnPosition, Quaternion.identity) as GameObject;
		bPrefab.SetActive (true);
		bPrefab.GetComponent<PlayerController> ().enabled = true; 
		bPrefab.GetComponent<BoxCollider2D> ().enabled = true; 
		Player = bPrefab;
	}

	public void CheckScore()
	{
		if (score >= levelScoreGoal) {
			decreasePublicSpeed (2);
			win = true;
		}
	}

	public void PlayLevelWarp()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			GameObject objProtected = objectProtected[0].gameObject;
			objProtected.transform.position = 
				Vector2.MoveTowards(objProtected.transform.position, 
					Player.transform.position, 3* Time.deltaTime);

			if (objProtected.transform.position.y == Player.transform.position.y) {
				if (objProtected.transform.position.x == Player.transform.position.x) {
					goingToNextLevel = true;
					Destroy (Player.GetComponent<Rigidbody2D> ());
				}
			}
		}
	}

	public void PlayGoingToNextLevel()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected != null) {
			Vector3 nextLevelPosition = new Vector3(0, 9, 0);

			GameObject objProtected = objectProtected[0].gameObject;
			objProtected.transform.position = 
				Vector2.MoveTowards(objProtected.transform.position, 
					nextLevelPosition, 5* Time.deltaTime);
			Player.transform.position = 
				Vector2.MoveTowards(Player.transform.position, 
					nextLevelPosition, 5* Time.deltaTime);

			if (objProtected.transform.position == nextLevelPosition) {
				readyNextLevel = true;
			}
		}
	}

	public void GotoNextLevel()
	{
		Application.LoadLevel(nextSceneName);
	}

	public bool getWin()
	{
		return win;
	}
		
}
