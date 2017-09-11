using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIController : MonoBehaviour {



	//float timer;
	GameObject selectedText;
	GameObject button1;
	public GameObject prefab1;
	Button btn1;
	//string buttonText1;

	GameObject button2;
	public GameObject prefab2;
	//string buttonText2;
	// Use this for initialization
	void Start () {
		button1 = GameObject.Find("Button_Object1");
		button2 = GameObject.Find("Button_Object2");


		selectedText = GameObject.Find ("SelectedText");

		//button1.GetComponent<Button>().

		button1.GetComponentInChildren<Text> ().text = prefab1.name;

		//button1.GetComponent<Button>().onClick.AddListener (ChangePrefab(button1.name));



		button2.gameObject.GetComponentInChildren<Text> ().text = prefab2.name;


	}
	
	// Update is called once per frame
	void Update () {
		//timer += Time.deltaTime;
		//CheckIfbuttonPressed ();


	}

	void CheckIfbuttonPressed(){
		
	}

	void ChangeLastButtonPressed(){
		
	}

	void ChangePrefab(string name){
		selectedText.GetComponent<Text> ().text = name;
	}
}
