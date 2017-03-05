using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {

	public GameObject[] objetoASpawnear;
	public float frecuenciaDeSpawneo;
	public float coordenadaX;
	public float coordenadaY;
	public float coordenadaZ;
	public float probabilidadMinima;
	public float probabilidadMaxima;
	public float wildCard;
	float currentSpeed;
	int counter;
	void Start () {
		counter = 0;
		InvokeRepeating ("SpawnObject", frecuenciaDeSpawneo, frecuenciaDeSpawneo);
		currentSpeed = 0f;
	}

	void Update(){
		CheckForChangesOnSpeed ();
	}

	IEnumerator Spawn(bool cambio,float publicSpeed) {
		if (!cambio) {
			yield return new WaitForSeconds(Random.Range(frecuenciaDeSpawneo, frecuenciaDeSpawneo));
		} else {
			yield return new WaitForSeconds(Random.Range(frecuenciaDeSpawneo-publicSpeed/2, frecuenciaDeSpawneo-publicSpeed/2));
		}
		SpawnObject ();
	}

	void CheckForChangesOnSpeed()
	{
		GameObject[] gc = GameObject.FindGameObjectsWithTag("GameController");
		if (gc != null) {
			float publicSpeed = gc [0].GetComponent<GameController> ().getPublicSpeed();
			if (publicSpeed == -2) {
				CancelInvoke ();
			} else {
				if (publicSpeed != currentSpeed) {
					CancelInvoke ();
					InvokeRepeating ("SpawnObject", frecuenciaDeSpawneo - publicSpeed / 2, frecuenciaDeSpawneo - publicSpeed / 2);
					SpawnObject ();
					currentSpeed = publicSpeed;
				}
			}

		}
	}


	void SpawnObject()
	{
		int index = Random.Range (0, objetoASpawnear.Length);
		int randomNumber = (int)(Random.Range( probabilidadMinima, probabilidadMaxima ));
		if (wildCard != -9) {
			if (randomNumber != wildCard) {
				Vector3 position;
				if (coordenadaX == -99) {
					position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, coordenadaZ);
				} else {
					position = new Vector3 (coordenadaX, coordenadaY, coordenadaZ);
				}

				GameObject newGameObject = Instantiate (objetoASpawnear[index]);
				newGameObject.transform.position = position;
			}
		} else {
			if (counter%2==0) {
				Vector3 position;
				if (coordenadaX ==-99) {
					position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, coordenadaZ);
				} else {
					position = new Vector3(coordenadaX, coordenadaY, coordenadaZ);
				}

				GameObject newGameObject = Instantiate(objetoASpawnear[index]);
				newGameObject.transform.position = position;
			}
		}
		counter++;
	}
}
