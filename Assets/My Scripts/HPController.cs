using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
	public GameObject character;

	public GameObject healthBar;

	// Update is called once per frame
	void Update()
	{
		string currentHP = character.GetComponent<CharacterController>().getHealthOfProtected().ToString();
		string maxHP = character.GetComponent<CharacterController>().getMaxHealthOfProtected().ToString();

		float cur_HP= float.Parse(currentHP); 
		float max_HP = float.Parse(maxHP); 

		float calc_health =  cur_HP/ max_HP;
		SetHealthBar (calc_health);
	}

	public void SetHealthBar(float myHealth){
		healthBar.transform.localScale = new Vector3(Mathf.Clamp(myHealth,0f ,1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
