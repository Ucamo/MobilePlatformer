using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {

	public GameObject objetoASpawnear;
	public float frecuenciaDeSpawneo;
	public float coordenadaX;
	public float coordenadaY;
	public float coordenadaZ;
	public float probabilidadMinima;
	public float probabilidadMaxima;
	public float wildCard;

	int counter;
	void Start () {
		counter = 0;
		InvokeRepeating ("SpawnObject", frecuenciaDeSpawneo, frecuenciaDeSpawneo);
	}


	void SpawnObject()
	{
		int randomNumber = (int)(Random.Range( probabilidadMinima, probabilidadMaxima ));
		if (wildCard != -9) {
			if (randomNumber != wildCard) {
				Vector3 position;
				if (coordenadaX == -99) {
					position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, coordenadaZ);
				} else {
					position = new Vector3 (coordenadaX, coordenadaY, coordenadaZ);
				}

				GameObject newGameObject = Instantiate (objetoASpawnear);
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

				GameObject newGameObject = Instantiate(objetoASpawnear);
				newGameObject.transform.position = position;
			}
		}
		counter++;
	}
}
