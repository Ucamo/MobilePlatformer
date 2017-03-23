using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public string FirstLevelScene;
	public string ContinueScene;
	public string CreditsScene;
	public string TutorialScene;
	public Button btnStartAdventure;
	public Button btnContinue;
	public Button btnCredits;
	public Button btnAccept;
	public Button btnCancel;
	public Canvas CanvasMainMenu;
	public Canvas CanvasCredits;
	public Canvas CanvasDeleteSaves;
	public Canvas CanvasTutorial;

	void Start () {
	
	}

	void ShowCanvasCredits(bool value)
	{
		CanvasCredits.gameObject.SetActive (value);
	}

	void ShowCanvasMainMenu(bool value)
	{
		CanvasMainMenu.gameObject.SetActive (value);
	}

	void ShowCanvasDeleteSaves(bool value)
	{
		CanvasDeleteSaves.gameObject.SetActive (value);
	}

	void ShowCanvasTutorial(bool value)
	{
		CanvasTutorial.gameObject.SetActive (value);
	}

	public void ShowCanvasCredits()
	{
		ShowCanvasMainMenu (false);
		ShowCanvasCredits (true);
		ShowCanvasDeleteSaves (false);
		ShowCanvasTutorial (false);
	}

	public void ShowCanvasMainMenu()
	{
		ShowCanvasMainMenu (true);
		ShowCanvasCredits (false);
		ShowCanvasDeleteSaves (false);
		ShowCanvasTutorial (false);
	}

	public void ShowCanvasDeleteSaves()
	{
		ShowCanvasMainMenu (false);
		ShowCanvasCredits (false);
		ShowCanvasDeleteSaves (true);
		ShowCanvasTutorial (false);
	}

	public void ShowCanvasTutorial()
	{
		ShowCanvasMainMenu (false);
		ShowCanvasCredits (false);
		ShowCanvasDeleteSaves (false);
		ShowCanvasTutorial (true);
	}


	public void StartAdventure()
	{
		PullTextDown (btnStartAdventure);
		if (GetSavedCoins () > 0) {
			ShowCanvasDeleteSaves ();
		} else {
			ShowCanvasTutorial ();
		}

	}

	public void Accept()
	{
		PullTextDown (btnAccept);
	}
	public void AcceptUp()
	{
		PullTextUp (btnAccept);
	}

	public void Cancel()
	{
		PullTextDown (btnCancel);
	}
	public void CancelUp()
	{
		PullTextUp(btnCancel);
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

	public int GetSavedCoins()
	{
		int coins = 0;
		coins=PlayerPrefs.GetInt("coins", 0);
		return coins;
	}

	public void DeleteUserData()
	{
		//Delete PlayerPrefs
		PlayerPrefs.DeleteAll();
		ShowCanvasTutorial ();
	}

	public void GoToTutorial()
	{
		GoToScene(TutorialScene);
	}

	public void GoToFirstLevel()
	{
		GoToScene(FirstLevelScene);
	}

	public void GoToScene(string nextSceneName)
	{
		Application.LoadLevel(nextSceneName);
	}
}
