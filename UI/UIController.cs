using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {



	float timer;

	public GameObject button1;
	public GameObject prefab1;
	string buttonText1;

	public GameObject button2;
	public GameObject prefab2;
	string buttonText2;
	// Use this for initialization
	void Start () {
		

		//button1.GetComponent<Button>().onClick.AddListener (ChangePrefab(1));
		//button1.GetComponent<Button>().


	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		CheckIfbuttonPressed ();


	}

	void CheckIfbuttonPressed(){
		
	}

	void ChangeLastButtonPressed(){
		
	}

	void ChangePrefab(int objectNumber){
		
	}
}
