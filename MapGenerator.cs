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

	GameObject gameController;
	Dictionary<Vector2, int> occupiedSpaces;

	Dictionary<List<int>,int> objectsByCoords = new Dictionary<List<int>, int>();


	void Start () {

		gameController = GameObject.Find ("GameController");
		occupiedSpaces = gameController.GetComponent<GameController> ().occupiedSpaces;

		GeneratePerlinNoiseToList ();
		GenerateObjectsToMap ();
		//GenerateRocks ();

		AddObjectToOrigDict ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GeneratePerlinNoiseToList(){
		//IMPLEMENT SEED SYSTEM

		float pNoiseValue;
		float frequency = 1;
		float scale = 1;

		//starts with one, cos coord system also starts with 1,1
		for (int y = 1; y <= mapHeight; y++){
			for (int x = 1; x <= mapWidth; x++){

				//+0.1f cos perlinnoise works with the remainders so that happens now better.
				pNoiseValue = Mathf.PerlinNoise ((x+0.1f)*frequency, (y+0.1f)*frequency);
				//pNoiseValue *= scale;

				//joko on puu, tai ei ole
				if (pNoiseValue >= 0.60){
					occupiedSpaces.Add (new Vector2 (x, y), 1);
				}

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
		foreach (KeyValuePair<Vector2, int> alkio in occupiedSpaces){
			coords.x = alkio.Key.x - 0.5f;
			coords.z = alkio.Key.y - 0.5f;
			coords.y = 2.5f;
			//Debug.Log (coords.x);
			//Debug.Log ("Creating object");
			Instantiate (TreePrefab, coords, Quaternion.identity);
		}
	}

	void AddObjectToOrigDict(){
		gameController.GetComponent<GameController> ().occupiedSpaces = occupiedSpaces;
	}
}
