using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	bool following;
	GameObject target;
	void Start () {
		following = false;
	}

	void Update () {
		CheckTargetPosition ();
		CheckTargetHeight ();
	}

	void CheckTargetPosition(){
		GameObject[] respawns = GameObject.FindGameObjectsWithTag("Protected");
		if(respawns!=null)
		target = respawns [0];
	}

	void CheckTargetHeight()
	{
		if (transform.position.y >= target.transform.position.y -5) {
			following = true;
		}
		ChaseTarget ();
	}

	void ChaseTarget (){
		if (following) {
			transform.position = 
				Vector2.MoveTowards(transform.position, 
					target.transform.position, 0.5f * Time.deltaTime);
		}
	}
}
