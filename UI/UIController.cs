using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIController : MonoBehaviour {

	public GameObject prefab1;
	public GameObject prefab2;
	public GameObject prefab3;
	//public GameObject prefab4;

	GameObject button1;
	GameObject button2;
	GameObject button3;

	List<GameObject> prefabs = new List<GameObject> ();
	List<GameObject> buttons = new List<GameObject> ();
	//float timer;
	GameObject selectedText;

	GameObject gameController;

	// Use this for initialization
	void Start () {
		//List of all of the prefabs available, MUST add manually
		//Needs to be in order with buttons -list.
		AddToPrefabList(prefab1, prefab2, prefab3);

		selectedText = GameObject.Find ("SelectedText");
		gameController = GameObject.Find("GameController");

		button1 = GameObject.Find ("Button_Object1");
		button2 = GameObject.Find ("Button_Object2");
		button3 = GameObject.Find ("Button_Object3");

		AddToButtonsList (button1, button2, button3);

		UpdateButtonTexts ();
		//button1.GetComponentInChildren<Text> ().text = prefab1.name;
		//button2.GetComponentInChildren<Text> ().text = prefab2.name;
		//button3.GetComponentInChildren<Text> ().text = prefab3.name;

		UpdateSelectedText ();
	}
	
	// Update is called once per frame
	void Update () {
		//timer += Time.deltaTime;
		//CheckIfbuttonPressed ();


	}
	void UpdateButtonTexts (){
		for (int i = 0; i < buttons.Count; i++) {
			buttons [i].GetComponentInChildren<Text> ().text = prefabs [i].name;
			Debug.Log (prefabs [i].name);
		}
	}

	void UpdateSelectedText(){
		selectedText.GetComponent<Text> ().text = "Selected: " + gameController.GetComponent<GameController> ().placeableObject.name;

	}

	void AddToButtonsList(params GameObject[] buttonArr){
		for (int i = 0; i < buttonArr.Length; i++){
			buttons.Add (buttonArr [i]);
		}
	}

	void AddToPrefabList(params GameObject[] prefabArr){

		for (int i = 0; i < prefabArr.Length; i++){
			prefabs.Add (prefabArr [i]);
		}
	}

	void CheckIfbuttonPressed(){
		
	}

	void ChangeLastButtonPressed(){
		
	}

	void ChangePrefab(string name){
		selectedText.GetComponent<Text> ().text = name;
	}

	public void ButtonClicked(int buttonNmbr){
		gameController.GetComponent<GameController> ().placeableObject = prefabs[buttonNmbr-1];
		UpdateSelectedText ();
	}
}
