using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {


	GameObject gameController;
	GameObject prefab;

	// Use this for initialization
	void Start () {
		
		gameController.GetComponent<GameController>().placeableObject = prefab;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//gameController.GetComponent<GameController>.placeableObject = prefab;
}
