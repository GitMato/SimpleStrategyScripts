using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSizes : MonoBehaviour{

//	public struct Size{
//
//		public string name;
//		public int widthX;
//		public int lengthY;
//
//		public Size(string objectName, int x, int y){
//			name = objectName;
//			widthX = x;
//			lengthY = y;
//		}
//	
//	};
//
//	//list of all the buildings
//	Size cube;
//	cube.




	string[,] BuildingInformation;
	public Dictionary<string, Vector2> Size = new Dictionary<string, Vector2>();

	void Start(){
		//needs 3 per building
		//first number = number of different buildings
		//second number consists of name, widthX, lengthY
		//Debug.Log("toimiiko?");
		BuildingInformation = new string[,]
		{
			{"Cube", "1", "1"},
			{"Cube2x2", "2", "2"}
		};


		//TestList ();
		AddBuildingsToDict ();
		//TestList ();
	}


	void AddBuildingsToDict(){

		//foreach (string building in BuildingInformation){
		string name;
		int x;
		int y;
		//Debug.Log (BuildingInformation.Length);
		// divided by 3 cos .Length gets all the cells inside
		for (int buildingIndex = 0; buildingIndex < BuildingInformation.Length/3; buildingIndex++){

			name = BuildingInformation [buildingIndex, 0].ToString();
			x = int.Parse(BuildingInformation [buildingIndex, 1]);
			y = int.Parse(BuildingInformation [buildingIndex, 2]);

			Size.Add (name, new Vector2 (x, y));
		
		}

	}

	void TestList(){
		Debug.Log ("test");
		if (Size.Count == 0){
			Debug.Log ("Size is empty.");
		} else {
			foreach (KeyValuePair <string, Vector2> pair in Size){
				//string name = pair.Key.ToString ();
				Debug.Log (pair.Key);
				Debug.Log(pair.Value.x);
				Debug.Log(pair.Value.y);
				// WORKS !
			}
		}


	}





}
