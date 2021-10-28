using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
	public bool autoUpdate = false;

	Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
		
		//mesh.normals = new Vector3[]
		//{
		//	Vector3.up,
		//	Vector3.up,
		//	Vector3.up,
		//	Vector3.up,
		//	Vector3.up
		//};
    }

    // Update is called once per frame
    void Update()
    {
		//Vector3[] verts = mesh.vertices;

		//verts[1] = new Vector3(0.5f, Time.realtimeSinceStartup, 0);

		//mesh.vertices = verts;
		//mesh.RecalculateBounds();
		
    }

	public void GenerateMesh()
	{
		mesh = GetComponent<MeshFilter>().sharedMesh;


		mesh.vertices = new Vector3[]
		{
			new Vector3(0,0,0),
			new Vector3(0.5f,0,0),
			new Vector3(1,0,0),
			new Vector3(0.5f,0,0.5f),
			new Vector3(0.5f,0,-0.5f),
			new Vector3(1.5f,0,0.5f),
			new Vector3(1.5f,0,-0.5f),
			new Vector3(2, 0, 0)
		};

		mesh.triangles = new int[]
		{
			0,3,1,
			1,3,2,
			0,2,4,
			//1,2,4,
			3,5,2,
			2,6,4,
			2,7,6,
			2,5,7
		};

		mesh.RecalculateNormals();
	}
}
