﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class MapMeshGenerator : MonoBehaviour {

	// Use this for initialization
	private int mapHeight;
	private int mapWidth;

	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();

	Mesh mesh;
	MeshFilter filter;
	MeshCollider coll;

	//public Dictionary<Vector2, float> mapHeightInfo = new Dictionary<Vector2, float>();


	void Start () {

		GetMapSize ();

		mesh = new Mesh ();
		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();

		GenerateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// SHOULD take the map size from another object (mapGenerator) - PLACEHOLDER
	void GetMapSize(){

		GameObject mapGenerator;
		mapGenerator = GameObject.Find ("MapGenerator");
		mapWidth = mapGenerator.GetComponent<MapGenerator> ().mapWidth;
		mapHeight = mapGenerator.GetComponent<MapGenerator> ().mapHeight;

	}

	//------------ GENERATE MESH FUNCTIONS ---------------------------

	void GenerateMesh(){

		GenerateVertices ();
		TriangulateSquare ();

		filter.mesh = mesh;
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();
		coll.sharedMesh = mesh;

		Vector2[] uvs = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; i++){
			uvs [i] = new Vector2 (vertices [i].x, vertices [i].z);
		}
		mesh.uv = uvs;

	}

	//Generating all the vertices for the mesh
	void GenerateVertices(){

		//jos halutaan korkeutta terrainiin
		float vertexHeight;
		int vertexHeightScale = 3;
		int frequency = 3;

		for (int y = 0; y < mapHeight; y++){
			for (int x = 0; x < mapWidth; x++){
				
				//+0.5f is to get the middle of the four vertices to be in the middle of the square IS -.5f BETTER?
				//vertices.Add (new Vector3 (x+0.5f, 0, y+0.5f));


				vertices.Add (new Vector3 (x, 0, y));

				//jos halutaan korkeutta terrainiin -- PerlinNoise käyttää moduloa ja sen takia pitää addaa jotain joka eroaa tasasta
//				vertexHeight = Mathf.PerlinNoise ((x+0.01f) * frequency, (y+0.01f) * frequency) * vertexHeightScale;
//				//Debug.Log ("VertexHeight = " + vertexHeight);
//				if (vertexHeight >= 0.8f){
//					vertexHeight = 0.5f;
//				}
//				vertices.Add (new Vector3 (x, vertexHeight, y));
//				mapHeightInfo.Add (new Vector2 (x, y), vertexHeight);
			}
		}
	}

	//Listing all the triangles in order not make two triangles to form a square
	void TriangulateSquare(){

		int step = -1;

		for (int y = 0; y < mapHeight-1; y++){
			step += 1;
			for (int x = 0; x < mapWidth-1; x++){

				AddTrianglesToTrianglesList (0+step, step+mapWidth, 1+step+mapWidth);
				AddTrianglesToTrianglesList (0+step, 1+step+mapWidth, 1+step);

				step += 1;

			}
		}

	}

	//adding the Vertex intex to the list
	void AddTrianglesToTrianglesList(int VertexA, int VertexB, int VertexC){

		triangles.Add (VertexA);
		triangles.Add (VertexB);
		triangles.Add (VertexC);
		
	}

	// -------------- GENERATE MESH FUNCTIONS END --------------------------


	void TransformHeightValues(){
		
	}
}
