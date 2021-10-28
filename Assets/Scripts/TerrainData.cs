using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
	GameObject meshObject;
	Vector2 position;
	Bounds bounds;

	public TerrainData(Vector2 coord, int size)
	{
		position = coord * size;
		bounds = new Bounds(position, Vector3.one * size);
		Vector3 pos3D = new Vector3(position.x, 0, position.y);

		meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		meshObject.transform.position = pos3D;

		SetVisible(false);
	}

	public void UpdateChunk()
	{
		float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(EndlessTerrain.viwerPosition));
		bool visible = viewerDistanceFromNearestEdge <= EndlessTerrain.maxViewDistance;

		SetVisible(visible);
	}

	public void SetVisible(bool visible)
	{
		meshObject.SetActive(visible);
	}

	public bool IsVisible()
	{
		return meshObject.activeSelf;
	}
}

[System.Serializable]
public struct TerrainSettings
{
	public TextureGeneratorSettings textureSettings;
	public MeshGeneratorSettings meshSettings;
}
