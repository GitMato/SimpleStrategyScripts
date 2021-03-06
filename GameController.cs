﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject placeableObject;
	GameObject gameController;
	Dictionary <string, Vector2> buildingSizes;
	//int minSpawn;
	//int maxSpawn;

	//-- raycasting and getting mousepos on map
	int floorMask;
	float camRayLength = 100f;

	public Camera mainCamera;
	public GameObject coordText;
	GameObject tooltipObject;

	private int mousePosx;
	private int mousePosz;
	private int mousePosy;

	private Vector3 mousePos;
	private Vector3 mousePosRounded = new Vector3();

	public GameObject mousePointGameObject;

	//end of raycasting and getting mousepos on map
	//---

	public Dictionary<Vector2, int> occupiedSpaces = new Dictionary<Vector2, int> ();

	public float TimeBetweenSpawns;
	private float timer;

	// Use this for initialization
	void Start () {
		tooltipObject = GameObject.Find ("TooltipText");
		gameController = GameObject.Find ("GameController");
		buildingSizes = gameController.GetComponent<BuildingSizes> ().Size;


		floorMask = -1;
		//floorMask = LayerMask.GetMask ("Map");
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;


	

		if(Input.GetButton ("Fire1") && timer >= TimeBetweenSpawns){

			//Debug.Log ("mouse1 painettu");
			InstantiateSelectedObject (placeableObject, mousePosRounded);
			//GetComponent<ObjectHandler> ().SpawnObject (mousePosx, mousePosy, mousePosz);
			//SpawnCubesOnClick ();
			//GetComponent<NodeHandler>().SpawnCubesOnClick(nodeWall, mousePosx, mousePosy, mousePosz, mousePointGameObject, mousePos);
			timer = 0;
		}

		if (Input.GetButton ("Fire2") && timer >= TimeBetweenSpawns) {

			DestroySelectedObject (mousePointGameObject, mousePosRounded);
			//GetComponent<ObjectHandler> ().RemoveObject (mousePointGameObject);
			//GetComponent<NodeHandler>().RemoveNode(mousePointGameObject);
			timer = 0;
		}
	}

	void FixedUpdate () {
		GetMouseCoords ();
		mousePosRounded = new Vector3 (mousePosx, 0, mousePosz);
//		mousePosRounded.x = mousePosx - 0.5f;
//		mousePosRounded.y = 0;
//		mousePosRounded.x = mousePosz - 0.5f;

	}


	//---------------------------FUNCTIONS FOR RAYCAST AND GETTING MOUSECOORDS HITTING THE MAP LAYER ----------

	void GetMouseCoords(){
		if (coordText != null) {


			Ray camRay = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorhit;
			//RaycastHit objectHit;
			Debug.DrawRay (camRay.origin, camRay.direction * 100, Color.red); 
			if (Physics.Raycast (camRay, out floorhit, camRayLength, floorMask)) {

				//Debug.DrawRay (camRay.origin, camRay.direction * 8, Color.red); 

				mousePos = floorhit.point;


				mousePointGameObject = floorhit.transform.gameObject;


				UpdateMouseCoords (mousePos);
				//Debug (mousePosition.x, mousePosition.z);
				//jos hiiri on osunut floormaskiin.
			} else {
				coordText.GetComponent<Text> ().text = "no hit";
			}
		}
	}

	void UpdateMouseCoords(Vector3 mousePos){

		//pyöristä raycastin hitpointin sijainti
//		mousePosx = Mathf.RoundToInt (mousePos.x);
//		mousePosy = Mathf.RoundToInt (mousePos.y);
//		mousePosz = Mathf.RoundToInt (mousePos.z);

		//Ylempi toimi hyvin, mutta puolessa välissä tuli virhe koska pyöristys, täten uusi:
		mousePosx = Mathf.RoundToInt (mousePos.x-0.5f);
		mousePosy = Mathf.RoundToInt (mousePos.y-0.5f);
		mousePosz = Mathf.RoundToInt (mousePos.z-0.5f);

		//Aseta osoitinteksti

		string mousePositionText = mousePosx + ", " + mousePosy + ", " + mousePosz;

		coordText.GetComponent<Text>().text = mousePositionText;



	}



	//END------------------------FUNCTIONS FOR RAYCAST AND GETTING MOUSECOORDS HITTING THE MAP LAYER ENDS ------


	//------------------------FUNCTIONS FOR INSTANTIATING OBJECTS AND DESTROYING THEM ------------------

	bool IsThereEnoughSpace(GameObject objectToCheck, Vector2 coords, int sizeX, int sizeY){
		//check if there is any keys in the occupied spaces dictionary with the building size

		Vector2 key = coords;
		//key.x += 0.5f;
		//key.y += 0.5f;
		int origX = Mathf.RoundToInt (key.x);
		int origY = Mathf.RoundToInt (key.y);
		float origfloatX = coords.x;
		float origfloatY = coords.y;
		//key.x = coords.x;
		//key.y = coords.z;
		string objectName = objectToCheck.name;

		//int sizeX = 0;
		//int sizeY = 0;
		//Debug.Log (name);
		//Debug.Log (buildingSizes.ContainsKey (name));

//		if (buildingSizes.ContainsKey(objectName)){
//			Vector2 size = buildingSizes [objectName];
//			sizeX = Mathf.RoundToInt(size.x);
//			sizeY = Mathf.RoundToInt(size.y);
//
//		}


		Debug.Log ("IsThereEnoughSpace: " + key);

		for (int y = origY; y < sizeY+origfloatY; y++){

			key.x = origfloatX;

			for (int x = origX; x < sizeX+origfloatX; x++){

				if (occupiedSpaces.ContainsKey(key)){
					return false;
				}
				key.x +=1;
			}

			key.y +=1;
		}
		return true;

	}




	void InstantiateSelectedObject(GameObject ObjectToPlace, Vector3 coords){

		// KUN INSTANTIATAAN OBJECTI PITÄÄ ENSIN KATSOA SEN LEVEYS JA SYVYYS JOTTA VOIDAAN ASETTAA SE OIKEIN MAPPIIN. ESIM 2x2 object on oudossa asennonossa. (puolikas)

		//ekana parametrina tarvitaan objectin nimi. BuildingSizes dictista saadaan arvo leveydelle ja syvyydelle
		//prefab objecti otetaan objectilistalta


		coords.y += 0.5f;
		Vector2 key = new Vector2();

		string objectName = ObjectToPlace.name;

		Vector3 placingCoords = coords;
		//Debug.Log (gameObject.name);

		int sizeX = 0;
		int sizeY = 0;

		if (buildingSizes.ContainsKey (objectName)) {
			//GetBuildingSizes ();
			Vector2 size = buildingSizes [objectName];
			sizeX = Mathf.RoundToInt(size.x);
			sizeY = Mathf.RoundToInt(size.y);

			//OFFSET AMOUNT
			float offsetX = sizeX / 2 - 0.5f;
			float offsetY = sizeY / 2 - 0.5f;

			if (sizeX % 2 != 0){
				placingCoords.x += 0.5f;
			} 
			if (sizeY % 2 != 0){
				placingCoords.z += 0.5f;
			}

			placingCoords.x += offsetX + 0.5f;
			placingCoords.z += offsetY + 0.5f;

			//adding 0.5f to get coordinates right
			//coords.x += 0.5f;
			//coords.z += 0.5f;
			key.x = coords.x;
			key.y = coords.z;

			if (IsThereEnoughSpace(ObjectToPlace, key, sizeX, sizeY)){

				GameObject lastInstantiated;
				//if(!occupiedSpaces.ContainsKey(key) && InMapArea(key)){
				lastInstantiated = Instantiate (ObjectToPlace, placingCoords, Quaternion.identity);
				Debug.Log ("Object instantiated: " + ObjectToPlace.name);

				//lisää objectin koon mukainen alue varattujen listaan
				int plusX = 0;
				int plusY = 0;

//				coords.x += 0.5f;
//				coords.z += 0.5f;

				//for (int y = 0; y < ObjectToPlace.GetComponent<BuildingProperties>().buildingLenghtY; y++){
				for (int y = 0; y < size.y; y++){

					plusX = 0;

					for (int x = 0; x < size.x; x++){

						//MENEE OIKEIN - WHICH CELLS ARE BEING TAKEN WITH THIS OBJECT
						//float[] calc = {coords.x + plusX + 0.5f, coords.z + plusY + 0.5f};
						float[] calc = {coords.x + plusX, coords.z + plusY};
						Debug.Log ("x:" + calc[0] + ", y:" + calc[1]);
						//Debug.Log (coords.z + plusY);


						//occupiedSpaces.Add (new Vector2 (coords.x + plusX + 0.5f, coords.z + plusY + 0.5f), lastInstantiated.GetInstanceID());
						occupiedSpaces.Add (new Vector2 (coords.x + plusX, coords.z + plusY), lastInstantiated.GetInstanceID());
						plusX +=1;
					}

					plusY +=1;

				}

				//occupiedSpaces.Add (new Vector2 (coords.x, coords.z), 1);
			} else {
				Debug.Log("Space is occupied!");
			}

		} else {
			Debug.Log ("No such name in BuildingSizes");
		}

//		if (IsThereEnoughSpace(ObjectToPlace, key)){
//		//if(!occupiedSpaces.ContainsKey(key) && InMapArea(key)){
//			Instantiate (ObjectToPlace, coords, Quaternion.identity);
//
//
//			//lisää objectin koon mukainen alue varattujen listaan
//			int plusX = 0;
//			int plusY = 0;
//
//			coords.x += 0.5f;
//			coords.y += 0.5f;
//
//			for (int y = 0; y < ObjectToPlace.GetComponent<BuildingProperties>().buildingLenghtY; y++){
//				
//				plusX = 0;
//
//				for (int x = 0; x < ObjectToPlace.GetComponent<BuildingProperties>().buildingWidthX; x++){
//
//					Debug.Log (coords.x + plusX);
//					Debug.Log (coords.y + plusY);
//
//					occupiedSpaces.Add (new Vector2 (coords.x + plusX, coords.z + plusY), 1);
//					plusX +=1;
//				}
//
//				plusY +=1;
//			}
//
//			//occupiedSpaces.Add (new Vector2 (coords.x, coords.z), 1);
//		} else {
//			Debug.Log("Space is occupied!");
//		}

		
	}

	int[] GetBuildingSizes(string name){
		int sizeX = 0;
		int sizeY = 0;
		if (buildingSizes.ContainsKey (name)) {
			Vector2 size = buildingSizes [name];
			sizeX = Mathf.RoundToInt(size.x);
			sizeY = Mathf.RoundToInt(size.y);
		}
		int[] sizes = new int[] { sizeX, sizeY };
		return sizes;
	}



	//WORKS INITIALLY WITH MOUSEPOINTCOORDS
	void DestroySelectedObject(GameObject objectToDestroy, Vector3 coords){
		
		//int objectWidth = gameObject.GetComponent<
		int objectID = -1;
		coords.y += 0.5f;
		Vector2 key = new Vector2 ();
		key.x = coords.x;
		key.y = coords.z;
		//key.x += 0.5f;
		//key.y += 0.5f;
		Debug.Log ("Destroy: x:" + key.x + ", y:" + key.y);
		//key.x = objectToDestroy.transform.position.x;
		//key.y = objectToDestroy.transform.position.z;
		Vector2 size = new Vector2 ();

		//layer 8 is the "Map" -layer.
		if (objectToDestroy != null && objectToDestroy.layer != 8) {
			
			objectID = objectToDestroy.GetInstanceID ();
			string objectName = objectToDestroy.name;
			objectName = objectName.Remove(objectName.IndexOf("("));
			if (buildingSizes.ContainsKey (objectName)) {
				size = buildingSizes [objectName];
			} else {
				Debug.Log ("Add building to buildingSizes dict");
			}

		} else {
			Debug.Log("No object which to point");
		}

		//int plusX = 0;
		//int plusY = 0;
		bool objectFound = false;

		if (occupiedSpaces.ContainsKey (key) && objectToDestroy != null && objectToDestroy.layer != 8) {
			Destroy (objectToDestroy);

			//size.x * size.y = total amount of entries
			for (int i = 0; i < size.x * size.y; i++){
				foreach (var item in occupiedSpaces) {
					if (item.Value == objectID) {


						occupiedSpaces.Remove (item.Key);
						objectFound = true;
						break;
					} else {
						objectFound = false;
						//Debug.Log ("Not found in this item.");

					}
				}

				if (!objectFound) {
					Debug.Log ("No removable found.");
				}


			}
		}
//		if(occupiedSpaces.ContainsKey(key) && objectToDestroy != null){
//			Destroy (objectToDestroy);
//
//			for (int y = 0; y < size.y; y++){
//
//				plusX = 0;
//
//				for (int x = 0; x < size.x; x++){
//
//					//MENEE OIKEIN - WHICH CELLS ARE BEING TAKEN WITH THIS OBJECT
//					float[] calc = {key.x + plusX, key.y + plusY};
//					Debug.Log ("Remove: x:" + calc[0] + ", y:" + calc[1]);
//					//Debug.Log (coords.z + plusY);
//
//
//					occupiedSpaces.Remove (new Vector2 (key.x + plusX, key.y + plusY));
//					plusX +=1;
//				}
//
//				plusY +=1;
//			}

			//occupiedSpaces.Remove (key);
//		}
	}


	//CHECK THAT THE COORDS ARE IN MAP AREA
	bool InMapArea(Vector2 coords){
		return true;
	}


	//END---------------------FUNCTIONS FOR INSTANTIATING OBJECTS AND DESTROYING THEM ------------------

}
