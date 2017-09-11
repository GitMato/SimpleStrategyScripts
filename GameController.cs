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

	bool IsThereEnoughSpace(GameObject objectToCheck, Vector2 coords){
		//check if there is any keys in the occupied spaces dictionary with the building size

		Vector2 key = coords;
		int origX = Mathf.RoundToInt (key.x);
		int origY = Mathf.RoundToInt (key.y);
		float origfloatX = coords.x;
		float origfloatY = coords.y;
		//key.x = coords.x;
		//key.y = coords.z;
		string name = objectToCheck.name;

		int sizeX = 0;
		int sizeY = 0;
		//Debug.Log (name);
		//Debug.Log (buildingSizes.ContainsKey (name));

		if (buildingSizes.ContainsKey(name)){
			Vector2 size = buildingSizes [name];
			sizeX = Mathf.RoundToInt(size.x);
			sizeY = Mathf.RoundToInt(size.y);

		}


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

		//GameObject gameObject = prefabList.containsblaablaa
		coords.y += 0.5f;
		Vector2 key = new Vector2();
		key.x = coords.x;
		key.y = coords.z;
		//Debug.Log (gameObject.name);

		//TARKISTA ENSIN LEVEYS JA SYVYYS JOKA MÄÄRITTÄÄ OBJECTIN SIJOITUKSEN
		if (ObjectToPlace.GetComponent<BuildingProperties>().buildingLenghtY % 2 == 0){
			coords.x += 0.5f;
			coords.z += 0.5f;
		}

		if (IsThereEnoughSpace(ObjectToPlace, key)){
		//if(!occupiedSpaces.ContainsKey(key) && InMapArea(key)){
			Instantiate (ObjectToPlace, coords, Quaternion.identity);


			//lisää objectin koon mukainen alue varattujen listaan
			int plusX = 0;
			int plusY = 0;

			for (int y = 0; y < ObjectToPlace.GetComponent<BuildingProperties>().buildingLenghtY; y++){
				
				plusX = 0;

				for (int x = 0; x < ObjectToPlace.GetComponent<BuildingProperties>().buildingWidthX; x++){
					
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
	void DestroySelectedObject(GameObject objectToDestroy, Vector3 coords){
		
		//int objectWidth = gameObject.GetComponent<

		coords.y += 0.5f;
		Vector2 key = new Vector2();
		key.x = coords.x;
		key.y = coords.z;

		if(occupiedSpaces.ContainsKey(key)){
			Destroy (objectToDestroy);

			occupiedSpaces.Remove (key);
		}
	}


	//CHECK THAT THE COORDS ARE IN MAP AREA
	bool InMapArea(Vector2 coords){
		return true;
	}


	//END---------------------FUNCTIONS FOR INSTANTIATING OBJECTS AND DESTROYING THEM ------------------

}
