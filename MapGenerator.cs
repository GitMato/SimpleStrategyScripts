using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour {

	// Use this for initialization
	public int mapHeight = 128;
	public int mapWidth = 128;
	public int tileX;
	public int tileY;

	public GameObject TreePrefab;
	public GameObject RockPrefab;

	GameObject gameController;
	Dictionary<Vector2, int> occupiedSpaces;

	Dictionary<List<int>,int> objectsByCoords = new Dictionary<List<int>, int>();

	public float noiseValueForTrees = 0.55f; //normal value for trees 0.5 - 0.55f
	public float noiseValueForRocks = 0.7f;

	void Start () {

		gameController = GameObject.Find ("GameController");
		occupiedSpaces = gameController.GetComponent<GameController> ().occupiedSpaces;

		GeneratePerlinNoiseToList (noiseValueForTrees, 1);
		GeneratePerlinNoiseToList (noiseValueForRocks, 2);
		GenerateObjectsToMap ();

		//Work around for not having gameobject in occupiedSpaces list- SHOULD I CHANGE IT TO THAT? 
		// USELESS FUNCTION - LITERALLY
		//ScaleObjects ();

		//GenerateRocks ();

		AddObjectToOrigDict ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GeneratePerlinNoiseToList(float noiseValue, int type){
		//IMPLEMENT SEED SYSTEM

		float pNoiseValue;
		float frequency = 1;
		if (type != 1){
			frequency = 1.2f;
		}
		float scale = 1;

		//starts with one, cos coord system also starts with 1,1
		for (int y = 0; y < mapHeight-1; y++){
			for (int x = 0; x < mapWidth-1; x++){

				//+0.1f cos perlinnoise works with the remainders so that happens now better.
				pNoiseValue = Mathf.PerlinNoise ((x+0.1f)*frequency, (y+0.1f)*frequency);
				//pNoiseValue *= scale;

				//joko on puu, tai ei ole
				if (pNoiseValue >= noiseValue){
					//Debug.Log ("Generate tree: x:" + x + ", y:" + y);
					if (!occupiedSpaces.ContainsKey (new Vector2(x, y))) {
						occupiedSpaces.Add (new Vector2 (x, y), type);
					} else {
						Debug.Log("OccupiedSpaces already contains key:" + x + ", " + y);
					}
				}

			}
		}

	}

	void ScaleObjects(){

		List<GameObject> objectit = new List<GameObject>();

		foreach (GameObject objecti in Resources.FindObjectsOfTypeAll(typeof (GameObject))){
			if (objectit.Contains(objecti)){
				continue;
			}
			objectit.Add(objecti);
		}
		float scale = 1.0f;

		foreach (GameObject clone in objectit){
			if (clone.name == "SimplePineTree(Clone)" || clone.name == "SimpleRock(Clone)") {
				scale = Random.Range (50, 100);
				scale = scale / 100;

				clone.transform.localScale = new Vector3 (scale, scale, scale);
			}
		}
	}

	//käy lapi listan ja asettaa alku objectin jokaisen valuen mukaisesti
	//placeholder:
	// 0: tyhjä
	// 1: puu
	// 2: kivi
	void GenerateObjectsToMap(){
		Vector3 coords = new Vector3 ();
		GameObject tree;
		GameObject rock;


//		for (int i = 0; i < occupiedSpaces.Count; i++){
//			alkio = occupiedSpaces [i];
//			coords.x = alkio.Key.x - 0.5f;
//			coords.z = alkio.Key.y - 0.5f;
//			coords.y = 2.5f;
//
//			tree = Instantiate (TreePrefab, coords, Quaternion.identity);
//			alkio.Value = tree.GetInstanceID;
//		}

		//can't modify dict if in foreach loop. That's why the new Vector2 array which consists of all the keys.
		//NOTE: 'using system.linq' would have made this easier.
		Vector2[] keys = new Vector2[occupiedSpaces.Keys.Count];
		occupiedSpaces.Keys.CopyTo (keys, 0);
		float scale = 1.0f;

		foreach (Vector2 key in keys){

			scale = 1.0f;
			coords.x = key.x + 0.5f;
			coords.z = key.y + 0.5f;
			//GameObject mapMeshGenObject = GameObject.Find ("Map");
			//coords.y = mapMeshGenObject.GetComponent<MapMeshGenerator> ().mapHeightInfo [key];
			coords.y = 0.0f;
			//coords.x -= 0.5f;
			//coords.y -= 0.5f;
			//Debug.Log (coords.x);
			//Debug.Log ("Creating object");
			if (occupiedSpaces [key] == 1) {
				tree = Instantiate (TreePrefab, coords, Quaternion.Euler (new Vector3 (0f, Random.Range (0, 359), 0f)));
				//tree = Instantiate (TreePrefab, coords, Quaternion.identity);
				scale = Random.Range(60,100);
				scale = scale / 100;

				tree.transform.localScale = new Vector3 (scale, scale, scale);

				occupiedSpaces [key] = tree.GetInstanceID ();

			} else if (occupiedSpaces [key] == 2) {
				rock = Instantiate (RockPrefab, coords, Quaternion.Euler (new Vector3 (0f, Random.Range (0, 359), 0f)));
				//rock = Instantiate (RockPrefab, coords, Quaternion.identity);
				scale = Random.Range(60,100);
				scale = scale / 100;

				rock.transform.localScale = new Vector3 (scale, scale, scale);

				occupiedSpaces [key] = rock.GetInstanceID ();

			}

			//Debug.Log ("Trees: x:" + coords.x + ", y:" + coords.z + " || TreeID: " + occupiedSpaces[key]);
			//Debug.Log ("InDic: x:" + key.x + ", y:" + key.y);


		}

		foreach (KeyValuePair<Vector2, int> alkio in occupiedSpaces){
			Debug.Log (alkio.Key + " : " + alkio.Value);
		}

//		foreach (KeyValuePair<Vector2, int> alkio in occupiedSpaces.Keys.CopyTo()){
//			coords.x = alkio.Key.x - 0.5f;
//			coords.z = alkio.Key.y - 0.5f;
//			coords.y = 2.5f;
//			//Debug.Log (coords.x);
//			//Debug.Log ("Creating object");
//			tree = Instantiate (TreePrefab, coords, Quaternion.identity);
//			alkio.Value = tree.GetInstanceID;
//
//		}
	}

	void AddObjectToOrigDict(){
		gameController.GetComponent<GameController> ().occupiedSpaces = occupiedSpaces;
	}
}
