using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
	public static List<Vector3> VertexList = new List<Vector3>();
	public static Dictionary<Vector3, int> VertexPointer = new Dictionary<Vector3, int>();

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

		mesh = new Mesh();

		VertexList.Clear();
		VertexPointer.Clear();

		VertexList.Add(new Vector3(0, 0, 0));
		VertexPointer.Add(new Vector3(0, 0, 0), 0);

		VertexList.Add(new Vector3(1, 0, 0));
		VertexPointer.Add(new Vector3(1, 0, 0), 1);

		VertexList.Add(new Vector3(1, 0, 1));
		VertexPointer.Add(new Vector3(1, 0, 1), 2);

		VertexList.Add(new Vector3(0, 0, 1));
		VertexPointer.Add(new Vector3(0, 0, 1), 3);

		VertexList.Add(new Vector3(2, 0, 0));
		VertexPointer.Add(new Vector3(2, 0, 0), 4);

		VertexList.Add(new Vector3(2, 0, 1));
		VertexPointer.Add(new Vector3(2, 0, 1), 5);

		Edge sideEdge = new Edge(new Vector3(1, 0, 0), new Vector3(1, 0, 1));

		Quad quad = new Quad(new Edge[]
		{
			new Edge(new Vector3(0,0,0), new Vector3(1,0,0)),
			sideEdge,
			new Edge(new Vector3(1,0,1), new Vector3(0,0,1)),
			new Edge(new Vector3(0,0,1), new Vector3(0,0,0))
		});

		Quad quad1 = new Quad(new Edge[]
		{
			new Edge(new Vector3(1,0,0), new Vector3(2,0,0)),
			new Edge(new Vector3(2,0,0), new Vector3(2,0,1)),
			new Edge(new Vector3(2,0,1), new Vector3(1,0,1)),
			sideEdge
		});

		quad.SubDevide();
		//quad.subQuads[0].SubDevide();

		List<int> triList = new List<int>();

		triList.AddRange(quad.GenerateTris());
		triList.AddRange(quad1.GenerateTris());

		mesh.vertices = VertexList.ToArray();
		mesh.triangles = triList.ToArray();


		//mesh.vertices = new Vector3[]
		//{
		//	new Vector3(0,0,0),
		//	new Vector3(0.5f,0,0),
		//	new Vector3(1,0,0),
		//	new Vector3(0.5f,0,0.5f),
		//	new Vector3(0.5f,0,-0.5f),
		//	new Vector3(1.5f,0,0.5f),
		//	new Vector3(1.5f,0,-0.5f),
		//	new Vector3(2, 0, 0)
		//};

		//mesh.triangles = new int[]
		//{
		//	0,3,1,
		//	1,3,2,
		//	0,2,4,
		//	//1,2,4,
		//	3,5,2,
		//	2,6,4,
		//	2,7,6,
		//	2,5,7
		//};

		//mesh.RecalculateNormals();
	}
}
