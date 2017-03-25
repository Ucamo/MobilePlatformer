using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectController : MonoBehaviour {

	public Button btnLvl1_1;
	public Button btnLvl1_2;
	public Button btnLvl1_3;

	void Start () {
		LoadUnlockedLevels ();
	}
		
	void LoadUnlockedLevels()
	{
		int lvl1_1 = 0;
		int lvl1_2 = 0;
		int lvl1_3 = 0;
		lvl1_1 = PlayerPrefs.GetInt ("Level_1_1", lvl1_1);
		lvl1_2 = PlayerPrefs.GetInt ("Level_1_2", lvl1_2);
		lvl1_3 = PlayerPrefs.GetInt ("Level_1_3", lvl1_3);

		if (lvl1_1 > 0) {
			UnlockButton (btnLvl1_1);
		}
		if (lvl1_2 > 0) {
			UnlockButton (btnLvl1_2);
		}
		if (lvl1_3 > 0) {
			UnlockButton (btnLvl1_3);
		}
	}

	void ButtonDown(Button btn)
	{
		EventTrigger trigger = btn.GetComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener( ( data ) => { PullTextDown(btn); } );
		trigger.triggers.Add( entry );
	}

	void ButtonUp(Button btn)
	{
		EventTrigger trigger = btn.GetComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener( ( data ) => { PullTextUp(btn); } );
		trigger.triggers.Add( entry );
	}


	void PullTextDown(Button btn)
	{
		GameObject txt1=btn.transform.Find("Front").gameObject;
		GameObject txt2=btn.transform.Find("Back").gameObject;
		txt1.transform.position = new Vector2 (txt1.transform.position.x, txt1.transform.position.y - 3);
		txt2.transform.position= new Vector2 (txt2.transform.position.x, txt2.transform.position.y - 3);
	}

	void PullTextUp(Button btn)
	{
		GameObject txt1=btn.transform.Find("Front").gameObject;
		GameObject txt2=btn.transform.Find("Back").gameObject;
		txt1.transform.position = new Vector2 (txt1.transform.position.x, txt1.transform.position.y + 3);
		txt2.transform.position= new Vector2 (txt2.transform.position.x, txt2.transform.position.y + 3);
	}

	void UnlockButton(Button button)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>("buttons");
		Sprite  btnSprite = sprites [0];
		Color itemColor = Color.cyan;
		button.GetComponent<Image> ().sprite = btnSprite;
		button.GetComponent<Image> ().color = itemColor;
		button.GetComponent<Button> ().enabled = true;
		button.interactable = true;
		ButtonDown (button);
		ButtonUp (button);
	}

	public void GoToScene(string nextSceneName)
	{
		Application.LoadLevel(nextSceneName);
	}

	public void GoTo1_1()
	{
		GoToScene ("Level_1_1");
	}
	public void GoTo1_2()
	{
		GoToScene ("Level_1_2");
	}
	public void GoTo1_3()
	{
		GoToScene ("Level_1_3");
	}

	public void GoToMainMenu()
	{
		GoToScene ("MainMenu");
	}
}
