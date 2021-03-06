﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject projectile;
	public Text txtCoins; 
	public GameObject scoreBar;
	public GameObject manaBar;
	public Text txtLives;
	public Button btnItem;
	public int currentMana;
	public int maxMana;
	public int healthOfProtected;
	public int maxHealthOfProtected;
	public float bulletSpeed;
	public float speed=8f;
	public float jumpForce=450;
	public int attack;
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
	public bool levelBoss;
	public GameObject objBoss;
	public bool bossDefeated;
	bool bossActive;
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

	bool facingRigh;
	bool a_idle;
	bool a_jump;
	bool a_attack;

	public Sprite defaultItemSlot;
	bool hasItem;

	AudioSource audioSource;
	public float volume;
	public AudioClip[] coinSound;
	public AudioClip itemSound;
	public AudioClip bombSound;
	public AudioClip[] shootSound;
	public AudioClip enemyShootSound;
	public AudioClip[] enemy_Hit;
	public AudioClip bossExplode;
	public AudioClip[] enemyExplode;
	public AudioClip ProtectedHurt;
	public AudioClip gameSong;
	public AudioClip bossSong;
	public AudioClip winSong;
	public AudioClip gameOverSong;

	bool playGameOver;
	bool playWin;

	void Start()
	{
		gameOver = false;
		jumps = 0;
		coins = 0;
		HideHealthBarProtected ();
		GetPlayerPrefs();
	}

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		PlayGameSong ();
	}

	void GetPlayerPrefs()
	{
		Scene scene = SceneManager.GetActiveScene();
		PlayerPrefs.SetInt (scene.name, 1);
		coins=PlayerPrefs.GetInt("coins", coins);
		currentMana = PlayerPrefs.GetInt ("currentMana", currentMana);
		int oldLives = PlayerPrefs.GetInt ("lives", lives);
		if (oldLives <= 0) {
			PlayerPrefs.SetInt ("lives", lives);
		} else {
			lives = oldLives;
		}
		CheckItemPlayerPref ();
	}

	void CheckItemPlayerPref()
	{
		string item = PlayerPrefs.GetString ("currentItem", null);
		if (item != null && item != "null") {
			if (item == "Bomb") {
				GameObject bomb = (GameObject)Resources.Load ("Bomb");
				AddItemToInventory (bomb);
			}
		}
	}

	void SetPlayerPrefs()
	{
		PlayerPrefs.SetInt ("coins", coins);
		PlayerPrefs.SetInt ("currentMana", currentMana);
		PlayerPrefs.SetInt ("lives", lives);
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
			CheckPlayerAnimations ();
		}
		DrawUI ();
	}

	public void CheckEvents()
	{
		if (win) {
			if (!levelBoss) {
				PlayLevelWarp ();
				bossDefeated = true;
			} else {
				if (!bossActive) {
					SpawnBoss ();
					bossActive = true;
				}
			}
			if (bossDefeated) {
				PlayLevelWarp ();
			}
		}
		if (goingToNextLevel) {
			PlayGoingToNextLevel ();
		}
		if (readyNextLevel) {
			ShowCanvasWin ();
		}
	}

	public int getAttack(){
		return attack;
	}
	public void IncreaseAttack(int val)
	{
		attack += val;
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
		if (val == 2)
			publicSpeed = -2;
	}

	void DrawUI()
	{
		txtCoins.text = coins.ToString();
		txtLives.text = getLives ().ToString();
		CheckMana ();
		CheckScoreBar ();
		CheckItemSlotAnimation ();
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
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
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
		PlayItemSound ();
	}

	void CallMana(string value)
	{
		Color blue = new Color (0,0,1);
		CallText (value, blue);
	}

	void CallLive(string value)
	{
		Color color = new Color (1,1,1);
		CallText (value, color);
	}

	void CallText(string value, Color color)
	{
		if (Player != null) {
			Vector3 firePosition = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, 0);
			GameObject damage = (GameObject)Resources.Load ("Damage");
			GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
			bPrefab.GetComponent<DamageController> ().CreateBonusColor (value,color);
		}
	}

	void CallCoin(string value)
	{
		Color color = new Color (1,1,0);
		CallText (value, color);
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
		CallCoin ("+1");
		coins++;
		PlayCoinSound ();
	}

	public int getCoins()
	{
		return coins;
	}

	public void IncreaseScore(int value)
	{
		score += value;
		CheckScore ();
		PlayEnemyExplode ();
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
		if (isRight && isUp&&grounded) {
			Jump ();
			MoveRight (1);
			grounded = true;
		} else {
			if (isLeft && isUp&&grounded) {
				Jump ();
				MoveLeft (-1);
				grounded = true;
			} else {
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
				if (isUp&&grounded) {
					Jump ();
					isUp = false;
				}
			}
		}
		grounded = Player.GetComponent<PlayerController>().getGrounded();
		if (grounded) {
			canDoublejump = false;
			jumps = 0;
			a_jump = false;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Scene scene = SceneManager.GetActiveScene();
			if (scene.name == "MainMenu") {
				Application.Quit();
			} else {
				Application.LoadLevel("MainMenu");
			}
		}
	}

	void Shoot()
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			Vector2 forceVector = Vector2.down;
			Vector3 firePosition = new Vector3(Player.transform.position.x, Player.transform.position.y-1, -1);
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
			a_attack = true;
			PlayShootSound ();
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
		PlayProtectedHurt ();
		if (healthOfProtected <= 0) {
			GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
			if (objectProtected != null) {
				Destroy (objectProtected [0]);
			}
			ShowCanvasGameOver ();
		}

	}

	void CheckMana()
	{
		float cur_M= float.Parse(currentMana.ToString()); 
		float max_M = float.Parse(maxMana.ToString()); 

		float calc_mana =  cur_M/ max_M;
		SetManaBar (calc_mana);
	}

	public void SetManaBar(float myMana){
		manaBar.transform.localScale = new Vector3(Mathf.Clamp(myMana,0f ,1f), manaBar.transform.localScale.y, manaBar.transform.localScale.z);
	}

	void CheckScoreBar()
	{
		float cur_S= float.Parse(score.ToString()); 
		float max_S = float.Parse(levelScoreGoal.ToString()); 

		float calc_score =  cur_S/ max_S;
		SetScoreBar (calc_score);
	}

	public void SetScoreBar(float myScore){
		scoreBar.transform.localScale = new Vector3(Mathf.Clamp(myScore,0f ,1f), scoreBar.transform.localScale.y, scoreBar.transform.localScale.z);
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
		if (objectProtected.Length>0) {
			GameObject objProtected = objectProtected [0];
			GameObject healthBar = objProtected.transform.Find("HealthBar").gameObject;
			healthBar.SetActive (false);
		}
	}

	public void DestroyAllEnemies()
	{
		ShakeCamera (1f, 3f);
		GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemyArray.Length>0 && enemyArray!=null) {
			foreach (GameObject objEnemy in enemyArray) {
				if (objEnemy.transform.position.y >= -7.35) {
					if (objEnemy.GetComponent<EnemyController> () != null) {
						int enemyHealth = objEnemy.GetComponent<EnemyController> ().GetHealth ();
						if (!objEnemy.name.Contains ("Boss")) {
							objEnemy.GetComponent<EnemyController> ().DecreaseHealth (enemyHealth);
						}
					}
				}
			}
		}
		PlayBombSound ();
		//Reset item slot
		ResetItemSlot();
	}

	void ResetItemSlot()
	{
		hasItem = false;
		ActiveItemSlot (false);
		btnItem.GetComponent<Button> ().onClick.RemoveAllListeners ();
		btnItem.GetComponent<Image> ().sprite = defaultItemSlot;
		GameObject objItemSprite =  GameObject.Find("itemSprite");
		objItemSprite.GetComponent<SpriteRenderer> ().sprite = null;
		PlayerPrefs.SetString ("currentItem", null);
	}

	void CheckItemSlotAnimation()
	{
		if (hasItem) {
			Sprite sprite = btnItem.GetComponent<SpriteRenderer> ().sprite;
			btnItem.GetComponent<Image> ().sprite = sprite;
		}
	}

	void ActiveItemSlot(bool value)
	{
		//btnItem.GetComponent<Image> ().enabled = value;
		btnItem.GetComponent<Button> ().enabled = value;
		//btnItem.enabled = value;
		btnItem.interactable = value;
		PlayItemSound ();
	}

	public void AddItemToInventory(GameObject item)
	{
		ResetItemSlot ();
		ActiveItemSlot (true);
		CallLive ("New Item!");
		Sprite itemSprite = item.GetComponent<SpriteRenderer> ().sprite;
		Color itemColor = item.GetComponent<SpriteRenderer> ().color;
		PlayerPrefs.SetString ("currentItem", item.name);
		//btnItem.GetComponent<Image>().sprite = itemSprite;
		GameObject objItemSprite =  GameObject.Find("itemSprite");
		objItemSprite.GetComponent<SpriteRenderer> ().sprite = itemSprite;
		objItemSprite.GetComponent<SpriteRenderer> ().color = itemColor;
		if (item.name.Contains("Bomb")) {
			btnItem.GetComponent<Button>().onClick.AddListener(() => DestroyAllEnemies());
		}

		Destroy (item);
		hasItem = true;
	}
		

	public void Jump()
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			if (Player.transform.position.y <= 6.24) {
				if (grounded && !gameOver) {
					Player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
					Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
					canDoublejump = true;
					grounded = false;
					Player.GetComponent<PlayerController> ().setGrounded (grounded);
					a_jump = true;
				} 
			}
		}
	}

	public void countJumps()
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			jumps++;
			if (jumps > 1) {
				//Player.GetComponent<Rigidbody2D> ().velocity = new Vector2(Player.GetComponent<Rigidbody2D> ().velocity.x, 0);
				//Player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				canDoublejump = false;
				jumps = 0;
			}
		}
	}


	void MoveRight(float push)
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			if (Player.transform.position.x < 2.24) {
				Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
				facingRigh = true;
				Flip (facingRigh);
			}
		}
	}
	void MoveLeft(float push)
	{
		if (Player.GetComponent<Rigidbody2D> () != null) {
			if(Player.transform.position.x>-2.24){
				Player.GetComponent<Rigidbody2D> ().position += speed * Vector2.right * push * Time.deltaTime;
				facingRigh = false;
				Flip (facingRigh);
			}
		}
	}

	void CheckPlayerAnimations()
	{
		Animator anim = Player.GetComponent<Animator> ();
		anim.SetBool ("idle", a_idle);
		anim.SetBool ("jump", a_jump);
		anim.SetBool ("attack", a_attack);
	}

	void Flip(bool val)
	{
		Player.GetComponent<SpriteRenderer> ().flipX = val;
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
		a_idle = true;
		a_jump = false;
		a_attack = false;
	}

	public void ResetRight(){
		isRight = false;
	}
	public void ResetLeft(){
		isLeft = false;
	}
	public void ResetUp(){
		isUp = false;
		a_jump = false;
	}

	public void ShowCanvasGameOver()
	{
		CanvasGameOver.gameObject.SetActive (true);
		SetPlayerPrefs ();
		PlayGameOverSong ();
		StopWorld ();
	}

	public void StopWorld()
	{
		decreasePublicSpeed (2);
		Destroy (Player.GetComponent<Rigidbody2D> ());
		lose = true;
		StopBackground ();
	}

	public void StopBackground()
	{
		GameObject[] arrayBackground = GameObject.FindGameObjectsWithTag("background");
		if (arrayBackground != null) {
			arrayBackground [0].GetComponent<ScrollBackground> ().Stop ();
		}
	}

	public void ShowCanvasWin()
	{
		CanvasWin.gameObject.SetActive (true);
		SetPlayerPrefs ();
		PlayWinSong ();
	}

	public void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void IncreaseLives()
	{
		CallLive ("+1UP");
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
		if (lives < 0) {
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
		bPrefab.GetComponent<Animator> ().enabled = true;
		Player = bPrefab;
	}

	public void CheckScore()
	{
		if (score >= levelScoreGoal) {
			
			win = true;
		}
	}

	public void setWin(bool val)
	{
		win = val;
	}

	public void PlayLevelWarp()
	{
		StopWorld ();
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

	void SpawnBoss()
	{
		PlayBossSong ();
		Vector3 firePosition = new Vector3(0, -5, -1);
		GameObject bPrefab = Instantiate(objBoss, firePosition, Quaternion.identity) as GameObject;
		ShakeCamera (1f, 5f);
	}

	public void ShakeCamera(float amount, float duration)
	{
		GameObject[] arrayCamera = GameObject.FindGameObjectsWithTag("MainCamera");
		if (arrayCamera.Length > 0 && arrayCamera != null) {
			GameObject mainCamera = arrayCamera [0].gameObject;
			mainCamera.GetComponent<ShakeCamera>().CameraShake(amount,duration);
		}
	}

	public void SetBossDefeated(bool val)
	{
		bossDefeated = val;
	}

	public bool getBossDefeated()
	{
		return bossDefeated;
	}

	public GameObject GetObjProtected()
	{
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if (objectProtected.Length > 0 && objectProtected != null) {
			return objectProtected [0];
		} else {
			return null;
		}
	}

	public void PlayItemSound()
	{
		audioSource.PlayOneShot(itemSound, volume/2);
	}

	public void PlayCoinSound()
	{
		int index = Random.Range (0, coinSound.Length);
		audioSource.PlayOneShot(coinSound[index], volume/2);
	}

	public void PlayBombSound()
	{
		audioSource.PlayOneShot(bombSound, volume/2);
	}

	public void PlayShootSound()
	{
		int index = Random.Range (0, shootSound.Length);
		audioSource.PlayOneShot(shootSound[index], volume/2);
	}

	public void PlayEnemyShootSound()
	{
		audioSource.PlayOneShot(enemyShootSound, volume/2);
	}

	public void PlayEnemyHit()
	{
		int index = Random.Range (0, enemy_Hit.Length);
		audioSource.PlayOneShot(enemy_Hit[index], volume/2);
	}

	public void PlayEnemyExplode()
	{
		int index = Random.Range (0, enemyExplode.Length);
		audioSource.PlayOneShot(enemyExplode[index], volume/2);
	}

	public void PlayBossExplode()
	{
		audioSource.PlayOneShot(bossExplode, volume/2);
	}

	public void PlayProtectedHurt()
	{
		audioSource.PlayOneShot(ProtectedHurt, volume/2);
	}

	public void PlayGameSong()
	{
		audioSource.loop = true;
		audioSource.PlayOneShot(gameSong, volume);
	}

	public void PlayBossSong()
	{
		audioSource.Stop();
		audioSource.loop = true;
		audioSource.PlayOneShot(bossSong, volume);
	}

	public void PlayWinSong()
	{
		if (!playWin) {
			audioSource.Stop();
			audioSource.loop = true;
			audioSource.PlayOneShot(winSong, volume);
			playWin = true;
		}
	}

	public void PlayGameOverSong()
	{
		if (!playGameOver) {
			audioSource.Stop();
			audioSource.loop = true;
			audioSource.PlayOneShot(gameOverSong, volume);
			playGameOver = true;
		}
	}
		
}
