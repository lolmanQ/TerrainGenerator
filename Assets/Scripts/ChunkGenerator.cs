using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
	public enum DrawMode { NoiseMap, ColorMap, MeshNoise, MeshColor};
	public DrawMode drawMode;

	public const int CHUNKSIZE = 240;

	private int mapChunkSize => CHUNKSIZE + 1;

	[SerializeField]
	private NoiseSettings noiseSettings;

	[SerializeField]
	private TextureGeneratorSettings textureGeneratorSettings;

	[SerializeField]
	private MeshGeneratorSettings meshGeneratorSettings;

	private TerrainSettings terrainSettings;

	[SerializeField]
	private MapDisplay mapDisplay;

	public bool autoUpdate = false;

	// Start is called before the first frame update
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseSettings);

		MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshGeneratorSettings);
		Mesh mesh = meshData.CreateMesh();

		//MapDisplay display = FindObjectOfType<MapDisplay>();
		switch (drawMode)
		{
			case DrawMode.NoiseMap:
				mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
				break;
			case DrawMode.ColorMap:
				mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, textureGeneratorSettings));
				break;
			case DrawMode.MeshNoise:
				mapDisplay.DrawMesh(mesh, TextureGenerator.TextureFromHeightMap(noiseMap));
				break;
			case DrawMode.MeshColor:
				mapDisplay.DrawMesh(mesh, TextureGenerator.TextureFromHeightMap(noiseMap, textureGeneratorSettings));
				break;
			default:
				break;
		}
	}

	public void SetSeed(int newSeed)
	{
		noiseSettings.seed = newSeed;
	}

	private void OnValidate()
	{
		//GenAndSetMesh();
	}
}
