using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	public enum DrawMode { NoiseMap, ColorMap};
	public DrawMode drawMode;

	private MeshFilter meshFilter;

	[SerializeField]
	private Vector2Int cellAmount;

	[SerializeField]
	private Vector2 cellSize;

	[SerializeField]
	private Vector2 cellScale;

	[SerializeField]
	private AnimationCurve heightCurve;

	[SerializeField]
	private Gradient colorGradient;

	public float heightScale = 1f;
	public float heightOffset = 1f;

	[SerializeField]
	private float textureScale;

	[SerializeField]
	private NoiseSettings noiseSettings;

	[SerializeField]
	private MapDisplay mapDisplay;

	public bool autoUpdate = false;

	// Start is called before the first frame update
	void Start()
	{
		GenAndSetMesh();

		// Set up the texture and a Color array to hold pixels during processing.
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void GenerateNoiseMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(cellAmount.x + 1, cellAmount.y + 1, noiseSettings);

		if (drawMode == DrawMode.NoiseMap)
			mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
		if (drawMode == DrawMode.ColorMap)
			mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, colorGradient));
	}


	public void GenAndSetMesh()
	{
		meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = GenerateMesh();
		meshFilter.mesh = mesh;	
	}

	Mesh GenerateMesh()
	{
		if (cellAmount.x < 1 || cellAmount.y < 1)
			return new Mesh();

		float[,] noiseMap = Noise.GenerateNoiseMap(cellAmount.x + 1, cellAmount.y + 1, noiseSettings);

		if (drawMode == DrawMode.NoiseMap)
			mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
		if (drawMode == DrawMode.ColorMap)
			mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, colorGradient));

		Mesh mesh = new Mesh();

		int cellTotal = cellAmount.x * cellAmount.y;
		int vertTotal = (cellAmount.x + 1) * (cellAmount.y + 1);

		Vector3[] vertices = new Vector3[vertTotal];
		int[] tris = new int[cellTotal * 2 * 3];
		Vector3[] normals = new Vector3[vertTotal];
		Vector2[] uv = new Vector2[vertTotal];


		for (int y = 0; y < cellAmount.y + 1; y++)
		{
			for (int x = 0; x < cellAmount.x + 1; x++)
			{
				vertices[GridToArr(x, y)] = new Vector3(x * cellSize.x, heightCurve.Evaluate(noiseMap[x, y]) * heightScale, y * cellSize.y);
				uv[GridToArr(x, y)] = new Vector2(Mathf.Repeat((float)x / (cellAmount.x + 1) * textureScale, 1), Mathf.Repeat((float)y / (cellAmount.y + 1),1) * textureScale);
			}
		}

		int tIndex = 0;
		for (int y = 0; y < cellAmount.y; y++)
		{
			for (int x = 0; x < cellAmount.x; x++)
			{
				tris[tIndex] = GridToArr(x, y);
				tris[tIndex + 1] = GridToArr(x + 1, y + 1);
				tris[tIndex + 2] = GridToArr(x + 1, y);
				tris[tIndex + 3] = GridToArr(x, y);
				tris[tIndex + 4] = GridToArr(x, y + 1);
				tris[tIndex + 5] = GridToArr(x + 1, y + 1);
				tIndex += 6;
			}
		}

		for (int i = 0; i < vertTotal; i++)
		{
			normals[i] = -Vector3.up;
		}

		mesh.vertices = vertices;
		mesh.triangles = tris;
		mesh.normals = normals;
		mesh.uv = uv;

		//Vector2[] uv = new Vector2[4]
		//{
		//	new Vector2(0, 0),
		//	new Vector2(1, 0),
		//	new Vector2(0, 1),
		//	new Vector2(1, 1)
		//};
		//

		//mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		return mesh;
	}

	int GridToArr(int x, int y)
	{
		int index = y * (cellAmount.x + 1) + x;
		return index;
	}

	public void SetSeed(int newSeed)
	{
		
	}

	private void OnValidate()
	{
		//GenAndSetMesh();
	}
}
