using UnityEngine;
using System.Collections;

public class ScrollBackground : MonoBehaviour {

	public float speed =0.2f;
	bool move=true;
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
		if (move) {
			Vector2 offset = new Vector2 (0,Time.time * speed) ;
			GetComponent<Renderer>().material.mainTextureOffset = offset;﻿
		}

	}

	public void Stop()
	{
		move = false;
	}


}
