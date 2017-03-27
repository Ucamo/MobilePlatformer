using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	bool following;
	GameObject target;
	public int currentHealth;
	public int maxHealth;
	public int experience;
	public int enemyAttack;
	public bool isProjectile;
	int charAttack;
	public bool isBoss;

	public GameObject healthBar;

	void Start () {
		following = false;
		HideHealthBar();
		if (!isProjectile) {
			StartCoroutine(FadeOutCR());
		}
		if (isProjectile) {
			PlayShootSound ();
		}
	}

	private IEnumerator FadeOutCR()
	{
		float duration = 2f; //0.5 secs
		float currentTime = 0f;
		while(currentTime < duration)
		{
			float alpha = Mathf.Lerp(0f, 1f, currentTime/duration);
			GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, alpha);
			currentTime += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	void Update () {
		CheckTargetPosition ();
		if (!isProjectile) {
			if (!isBoss) {
				CheckTargetHeight ();
			}

		}
		CheckHealth ();
		CheckPosition ();
		CheckPlayer ();
	}

	public void CheckPosition()
	{
		if (transform.position.x > 0) {
			Flip (true);
		} else {
			Flip (false);
		}
	}

	void Flip(bool val)
	{
		GetComponent<SpriteRenderer> ().flipX = val;
	}

	public int getEnemyAttack()
	{
		return enemyAttack;
	}

	void CheckPlayer()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc.Length > 0) {
			int attack = gc [0].GetComponent<GameController> ().getAttack ();
			charAttack = attack;
		}

	}

	void PlayShootSound()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc.Length > 0) {
			gc [0].GetComponent<GameController> ().PlayEnemyShootSound ();
		}
	}

	void PlayEnemyHit()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc.Length > 0) {
			gc [0].GetComponent<GameController> ().PlayEnemyHit ();
		}
	}

	void PlayEnemyExplosion()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc.Length > 0) {
			gc [0].GetComponent<GameController> ().PlayEnemyExplode ();
		}
	}

	void CheckHealth()
	{
		string currentHP = currentHealth.ToString ();
		string maxHP = maxHealth.ToString ();;

		float cur_HP= float.Parse(currentHP); 
		float max_HP = float.Parse(maxHP); 

		float calc_health =  cur_HP/ max_HP;
		SetHealthBar (calc_health);
	}
	public void SetHealthBar(float myHealth){
		healthBar.transform.localScale = new Vector3(Mathf.Clamp(myHealth,0f ,1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	public void ShowHealthBar()
	{
		StartCoroutine (showHealth ());
	}

	IEnumerator showHealth ()
	{ 
		GameObject healthBar = transform.Find("HealthBar").gameObject;
		healthBar.SetActive (true);
		yield return new WaitForSeconds(1);
		HideHealthBar();
	}

	public void HideHealthBar()
	{
		GameObject healthBar = transform.Find("HealthBar").gameObject;
		healthBar.SetActive (false);
	}

	void CheckTargetPosition(){
		GameObject[] objectProtected = GameObject.FindGameObjectsWithTag("Protected");
		if(objectProtected.Length>0)
			target = objectProtected [0];
	}

	void CheckTargetHeight()
	{
		if (target != null) {
			if (transform.position.y >= target.transform.position.y -5) {
				following = true;
			}
			ChaseTarget ();
		}
	}

	void ChaseTarget (){
		if (following) {
			transform.position = 
				Vector2.MoveTowards(transform.position, 
					target.transform.position, 1.5f * Time.deltaTime);
			if (transform.position.y == target.transform.position.y && transform.position.x == target.transform.position.x) {
				InflictDamageToProtected ();
			}
		}
	}

	void InflictDamageToProtected()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc != null) {
			gc [0].GetComponent<GameController> ().DecreaseHealthOfProtected(enemyAttack);
			Destroy (gameObject);
		}
	}
		
	public void DecreaseHealth(int value){
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc != null) {
			int charAttack = gc [0].GetComponent<GameController> ().getAttack ();
			currentHealth -= value;
			PlayEnemyHit ();
			if (!isProjectile) {
				CallDamage (value);
				ShowHealthBar ();
			}
			if (currentHealth <= 0) {
				int index = Random.Range (0, 30);
				if (!isProjectile) {
					gc [0].GetComponent<GameController> ().IncreaseScore (experience);
					CallExperiencie (experience);
					if (index == 15) {
						gc [0].GetComponent<GameController> ().ProtectedDropRareItem ();
					} else {
						gc [0].GetComponent<GameController> ().ProtectedDropItem ();
					}
					PlayEnemyExplosion ();
				} else {
					CallWord ("DEFENDED");
				}
				if (isBoss) {
					gc [0].GetComponent<GameController> ().SetBossDefeated (true);
					gc [0].GetComponent<GameController> ().PlayBossExplode();
				}
				Destroy (this.gameObject.gameObject);
			}
		}
	}

	void CallWord(string value)
	{
		Vector3 firePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
		GameObject damage = (GameObject)Resources.Load ("Damage");
		GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
		Color white = new Color (1,1,1);
		bPrefab.GetComponent<DamageController> ().CreateBonusColor (value,white);
	}

	void CallDamage(int value)
	{
		Vector3 firePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
		GameObject damage = (GameObject)Resources.Load ("Damage");
		GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
		bPrefab.GetComponent<DamageController> ().CreateDamage ("-"+value.ToString());
	}

	void CallExperiencie(int value)
	{
		Vector3 firePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
		GameObject damage = (GameObject)Resources.Load ("Damage");
		GameObject bPrefab = Instantiate(damage, firePosition, Quaternion.identity) as GameObject;
		Color white = new Color (1,1,1);
		bPrefab.GetComponent<DamageController> ().CreateBonusColor ("+"+value+" Pts",white);
	}

	public int GetHealth()
	{
		return currentHealth;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			DecreaseHealth (charAttack);
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "Protected")
		{
			InflictDamageToProtected ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Projectile")
		{
			DecreaseHealth (charAttack);
			Destroy (coll.gameObject);
		}
		if (coll.gameObject.tag == "Protected")
		{
			InflictDamageToProtected ();
		}
	}
}
