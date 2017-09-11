using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour {

	// Use this for initialization
	GameObject SelectedTextObject;
	GameObject gameController;
	public GameObject prefab;


	void Start () {

		SelectedTextObject = GameObject.Find ("UICanvas/SelectedText");
		gameController = GameObject.Find ("GameController");
		gameController.GetComponent<GameController> ().placeableObject = prefab;
		SelectedTextObject.GetComponent<Text> ().text = prefab.name;



	}
	




}
