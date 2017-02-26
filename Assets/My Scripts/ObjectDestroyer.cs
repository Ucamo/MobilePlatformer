using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {

	//public GameObject canvas;

	//private AudioSource audioSource;
	//public AudioClip gameOverSound;
	//public float volume;

	void Start () {

	}

	void Awake()
	{
		//audioSource = GetComponent<AudioSource>();
	}
	void PlayGameOverSound()
	{
		//audioSource.PlayOneShot(gameOverSound, volume);
	}

	// Update is called once per frame
	void Update () {

	}
	public void OnCollisionEnter2D(Collision2D obj) {
		Destroy (obj.gameObject);
		if (obj.gameObject.name.Contains("Player")) {
			DecreaseLives ();
		}
	}

	public void OnTriggerEnter2D(Collider2D node) {
		Destroy(node.gameObject);
		if (node.gameObject.name.Contains("Player")) {
			DecreaseLives ();
		}
	}


	public void DecreaseLives()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc != null) {
			gc [0].GetComponent<CharacterController> ().decreaseLives ();

		}
	}

	void StopTime()
	{
		Time.timeScale = 0.00001f;
	}

}
