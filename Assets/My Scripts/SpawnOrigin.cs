using UnityEngine;
using System.Collections;

public class SpawnOrigin : MonoBehaviour {

	string spawnOrigin;
	void Start () {
	
	}

	public void SetSpawnOrigin(string val)
	{
		spawnOrigin = val;
	}

	public string GetSpawnOrigin()
	{
		return spawnOrigin;
	}
		
}
