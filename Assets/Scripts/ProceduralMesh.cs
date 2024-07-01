using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
	public static List<Vector3> VertexList = new List<Vector3>();
	public static Dictionary<Vector3, int> VertexPointer = new Dictionary<Vector3, int>();
	public static Dictionary<EdgePointer, Edge> EdgeDictionary = new Dictionary<EdgePointer, Edge>();

	public bool autoUpdate = false;

	[SerializeField]
	MeshFilter meshFilter;

	[Range(0,7)]
	[SerializeField]
	private int generalLevelOfDetail = 1;

	[SerializeField]
	private float quadScale = 1;

	[SerializeField]
	private int quadAmount = 4;

	[SerializeField]
	private AnimationCurve heightCurve;

	[SerializeField]
	private float heightScale;

	[SerializeField]
	private NoiseSettings noiseSettings;

	Mesh mesh;

	List<Quad> topQuads = new List<Quad>();

	[SerializeField]
	private Player player;

	// Start is called before the first frame update
	void Start()
	{
		//GetComponent<MeshFilter>().mesh = new Mesh();


		GenerateMesh();
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

		foreach (Quad item in topQuads)
		{
			item.QuadUpdate();
		}

		List<Vector3> verticesList = new List<Vector3>();
		Dictionary<Vector3, int> newDict = new Dictionary<Vector3, int>();

		int vertexIndex = 0;
		foreach (KeyValuePair<Vector3, int> vertex in VertexPointer)
		{
			verticesList.Add(vertex.Key);
			newDict[vertex.Key] = vertexIndex;
			vertexIndex++;
		}

		VertexPointer = newDict;
		VertexList = verticesList;

		List<int> triList = new List<int>();

		for (int i = 0; i < topQuads.Count; i++)
		{
			triList.AddRange(topQuads[i].GenerateTris());
		}

		Vector3[] vertices = VertexList.ToArray();

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];

			vector = new Vector3(vector.x, heightCurve.Evaluate(Noise.GetPoint(vector.x, vector.z, noiseSettings)) * heightScale, vector.z);

			vertices[i] = vector;
		}

		mesh.vertices = vertices;
		mesh.triangles = triList.ToArray();

		mesh.RecalculateNormals();

		meshFilter.sharedMesh = mesh;
	}

	public void GenerateMesh()
	{
		//mesh = GetComponent<MeshFilter>().sharedMesh;
		//mesh = GetComponent<MeshFilter>().mesh;

		mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		VertexList.Clear();
		VertexPointer.Clear();
		EdgeDictionary.Clear();
		topQuads.Clear();

		#region Vertex and Edge assignment
		//AddVertex(0, 0, 0);
		//AddVertex(1, 0, 0);
		//AddVertex(1, 0, 1);
		//AddVertex(0, 0, 1);
		//AddVertex(2, 0, 0);
		//AddVertex(2, 0, 1);
		//AddVertex(0, 0, -1);
		//AddVertex(1, 0, -1);

		//AddEdge(new EdgePointer(1, 0, 0, 1, 0, 1));
		//AddEdge(new EdgePointer(0, 0, 0, 1, 0, 0));
		#endregion

		

		int gridSize = Mathf.CeilToInt(quadAmount/2f);

		if(quadAmount == 1)
		{
			topQuads.Add(CreateQuad(-0.5f, -0.5f, quadScale));
		}
		else
		{
			for (int y = -gridSize; y < gridSize; y++)
			{
				for (int x = -gridSize; x < gridSize; x++)
				{
					topQuads.Add(CreateQuad(x, y, quadScale));
				}
			}
		}

		#region Old Manual Quad assign
		//topQuads.Add(new Quad(new Edge[]
		//{
		//	AddEdge(0, 0, 0, 1, 0, 0),
		//	AddEdge(1, 0, 0, 1, 0, 1),
		//	AddEdge(0, 0, 1, 1, 0, 1),
		//	AddEdge(0, 0, 0, 0, 0, 1)
		//}));

		//topQuads.Add(new Quad(new Edge[]
		//{
		//	new Edge(new Vector3(1,0,0), new Vector3(2,0,0)),
		//	new Edge(new Vector3(2,0,0), new Vector3(2,0,1)),
		//	new Edge(new Vector3(1,0,1), new Vector3(2,0,1)),
		//	AddEdge(new EdgePointer(1, 0, 0, 1, 0, 1))
		//}));

		//topQuads.Add(new Quad(new Edge[]
		//{
		//	new Edge(new Vector3(0,0,-1), new Vector3(1,0,-1)),
		//	new Edge(new Vector3(1,0,-1), new Vector3(1,0,0)),
		//	AddEdge(new EdgePointer(0, 0, 0, 1, 0, 0)),
		//	new Edge(new Vector3(0,0,-1), new Vector3(0,0,0))
		//}));
		#endregion

		//topQuads[0].SubDevide();
		//topQuads[0].subQuads[0].SubDevide();
		//topQuads[0].subQuads[0].subQuads[0].SubDevide();
		//topQuads[0].subQuads[1].SubDevide();
		//topQuads[2].SubDevide();

		foreach (Quad quad in topQuads)
		{
			for (int i = 0; i < generalLevelOfDetail; i++)
			{
				quad.SubDevide();
			}
		}


		List<int> triList = new List<int>();

		for (int i = 0; i < topQuads.Count; i++)
		{
			triList.AddRange(topQuads[i].GenerateTris());
		}

		Vector3[] vertices = VertexList.ToArray();

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];

			vector = new Vector3(vector.x, heightCurve.Evaluate(Noise.GetPoint(vector.x, vector.z, noiseSettings)) * heightScale, vector.z);

			vertices[i] = vector;
		}

		mesh.vertices = vertices;
		mesh.triangles = triList.ToArray();

		mesh.RecalculateNormals();

		meshFilter.sharedMesh = mesh;
	}

	public void ResetMesh()
	{
		mesh = new Mesh();
		meshFilter.sharedMesh = mesh;
	}

	public void AddVertex(float x, float y, float z)
	{
		AddVertex(new Vector3(x, y, z));
	}

	public void AddVertex(Vector3 vector)
	{
		if (!VertexPointer.ContainsKey(vector))
		{
			VertexList.Add(vector);
			VertexPointer.Add(vector, VertexList.Count - 1);
		}
	}

	public Edge AddEdge(Vector3 vertexA, Vector3 vertexB)
	{
		return AddEdge(new EdgePointer(vertexA, vertexB));
	}

	public Edge AddEdge(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		return AddEdge(new EdgePointer(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2)));
	}

	public Edge AddEdge(EdgePointer edgePointer)
	{
		if (!EdgeDictionary.ContainsKey(edgePointer))
		{
			AddVertex(edgePointer.vertexA);
			AddVertex(edgePointer.vertexB);
			
			EdgeDictionary.Add(edgePointer, new Edge(edgePointer));
		}
		return EdgeDictionary[edgePointer];
	}

	public Quad CreateQuad(float xOffset, float yOffset, float scale = 1)
	{
		float xOffsetScaled = xOffset * scale;
		float yOffsetScaled = yOffset * scale;

		Quad quad = new Quad(new Edge[]
		{
			AddEdge(xOffsetScaled, 0, yOffsetScaled, (xOffset + 1) * scale, 0, yOffsetScaled),
			AddEdge((xOffset + 1) * scale, 0, yOffsetScaled, (xOffset + 1) * scale, 0, (yOffset + 1) * scale),
			AddEdge(xOffsetScaled, 0, (yOffset + 1) * scale, (xOffset + 1) * scale, 0, (yOffset + 1) * scale),
			AddEdge(xOffsetScaled, 0, yOffsetScaled, xOffsetScaled, 0, (yOffset + 1) * scale)
		});

		return quad;
	}

	private void OnValidate()
	{
		if(quadAmount < 1)
		{
			quadAmount = 1;
		}
		if(quadScale < 0.00009f)
		{
			quadScale = 0.0001f;
		}
	}
}
