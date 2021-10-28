using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
	public const float maxViewDistance = 450;
	public Transform viewer;

	public Transform mapParent;

	public static Vector2 viwerPosition;
	int chunkSize;
	int chunksVisibleInViewDistance;

	Dictionary<Vector2, TerrainData> terrainChunkDictionary = new Dictionary<Vector2, TerrainData>();
	List<TerrainData> terrainChunksVisibleLastUpdate = new List<TerrainData>();

	// Start is called before the first frame update
	void Start()
	{
		chunkSize = ChunkGenerator.CHUNKSIZE;
		chunksVisibleInViewDistance = Mathf.RoundToInt(chunkSize / maxViewDistance);
	}

	void UpdateVisibleChunks()
	{
		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
		{
			terrainChunksVisibleLastUpdate[i].SetVisible(false);
		}
		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viwerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viwerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateChunk();
					if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
					{
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainData(viewedChunkCoord, chunkSize, mapParent));
				}

			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(viewer != null)
		{
			viwerPosition = new Vector2(viewer.position.x, viewer.position.z);
		}
		UpdateVisibleChunks();
	}
}
