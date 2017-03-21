using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public string FirstLevelScene;
	public string ContinueScene;
	public string CreditsScene;
	public Button btnStartAdventure;
	public Button btnContinue;
	public Button btnCredits;

	void Start () {
	
	}

	void ShowDeleteUserData()
	{
		//TODO: Show canvas asking if the player wants to delete the data of the game to start over.
	}

	public void StartAdventure()
	{
		PullTextDown (btnStartAdventure);
	}

	public void StartAdventureUp()
	{
		PullTextUp (btnStartAdventure);
	}

	public void Continue()
	{
		PullTextDown (btnContinue);
	}

	public void ContinueUp()
	{
		PullTextUp (btnContinue);
		//GoToScene (ContinueScene);
	}

	public void Credits()
	{
		PullTextDown (btnCredits);
		//GoToScene (CreditsScene);
	}

	public void CreditsUp()
	{
		PullTextUp (btnCredits);
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


	public void DeleteUserData()
	{
		//TODO: Delete PlayerPrefs
		GoToScene(FirstLevelScene);
	}

	public void GoToScene(string nextSceneName)
	{
		Application.LoadLevel(nextSceneName);
	}
}
