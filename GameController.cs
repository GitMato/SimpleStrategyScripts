using System.Collections;
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

	private int mousePosx;
	private int mousePosz;
	private int mousePosy;

	private Vector3 mousePos;
	private Vector3 mousePosRounded;

	GameObject mousePointGameObject;

	//end of raycasting and getting mousepos on map
	//---

	Dictionary<Vector2, int> occupiedSpaces = new Dictionary<Vector2, int> ();

	public float TimeBetweenSpawns;
	private float timer;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController");
		buildingSizes = gameController.GetComponent<BuildingSizes> ().Size;


		floorMask = -1;
		//floorMask = LayerMask.GetMask ("Map");
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;


	

		if(Input.GetButton ("Fire1") && timer >= TimeBetweenSpawns){


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
		mousePosRounded = new Vector3 (mousePosx-.5f, 0, mousePosz-.5f);

	}


	//---------------------------FUNCTIONS FOR RAYCAST AND GETTING MOUSECOORDS HITTING THE MAP LAYER ----------

	void GetMouseCoords(){
		if (coordText != null) {


			Ray camRay = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorhit;
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
		mousePosx = Mathf.RoundToInt (mousePos.x+0.5f);
		mousePosy = Mathf.RoundToInt (mousePos.y+0.5f);
		mousePosz = Mathf.RoundToInt (mousePos.z+0.5f);

		//Aseta osoitinteksti

		string mousePositionText = mousePosx + ", " + mousePosy + ", " + mousePosz;

		coordText.GetComponent<Text>().text = mousePositionText;



	}

	//END------------------------FUNCTIONS FOR RAYCAST AND GETTING MOUSECOORDS HITTING THE MAP LAYER ENDS ------


	//------------------------FUNCTIONS FOR INSTANTIATING OBJECTS AND DESTROYING THEM ------------------

	bool IsThereEnoughSpace(GameObject gameobject, Vector2 coords){
		//check if there is any keys in the occupied spaces dictionary with the building size

		Vector2 key = coords;

		//key.x = coords.x;
		//key.y = coords.z;

		int sizeX;
		int sizeY;


		for (int y = 0; y < gameObject.GetComponent<BuildingProperties>().buildingLenghtY; y++){

			key.x = 0;

			for (int x = 0; x < gameObject.GetComponent<BuildingProperties>().buildingWidthX; x++){

				if (occupiedSpaces.ContainsKey(key)){
					return false;
				}
				key.x +=1;
			}

			key.y +=1;
		}
		return true;

	}




	void InstantiateSelectedObject(GameObject gameObject, Vector3 coords){

		//ekana parametrina tarvitaan objectin nimi. BuildingSizes dictista saadaan arvo leveydelle ja syvyydelle
		//prefab objecti otetaan objectilistalta

		//GameObject gameObject = prefabList.containsblaablaa
		coords.y += 0.5f;
		Vector2 key = new Vector2();
		key.x = coords.x;
		key.y = coords.z;

		if (IsThereEnoughSpace(gameObject, key)){
		//if(!occupiedSpaces.ContainsKey(key) && InMapArea(key)){
			Instantiate (gameObject, coords, Quaternion.identity);

			//lisää objectin koon mukainen alue varattujen listaan
			int plusX = 0;
			int plusY = 0;

			for (int y = 0; y < gameObject.GetComponent<BuildingProperties>().buildingLenghtY; y++){
				
				plusX = 0;

				for (int x = 0; x < gameObject.GetComponent<BuildingProperties>().buildingWidthX; x++){
					
					occupiedSpaces.Add (new Vector2 (coords.x + plusX, coords.z + plusY), 1);
					plusX +=1;
				}

				plusY +=1;
			}
			//occupiedSpaces.Add (new Vector2 (coords.x, coords.z), 1);
		} else {
			Debug.Log("Space is occupied!");
		}

		
	}


	//WORKS INITIALLY WITH MOUSEPOINTCOORDS
	void DestroySelectedObject(GameObject gameObject, Vector3 coords){
		
		//int objectWidth = gameObject.GetComponent<

		coords.y += 0.5f;
		Vector2 key = new Vector2();
		key.x = coords.x;
		key.y = coords.z;

		if(occupiedSpaces.ContainsKey(key)){
			Destroy (gameObject);

			occupiedSpaces.Remove (key);
		}
	}


	//CHECK THAT THE COORDS ARE IN MAP AREA
	bool InMapArea(Vector2 coords){
		return true;
	}


	//END---------------------FUNCTIONS FOR INSTANTIATING OBJECTS AND DESTROYING THEM ------------------

}
