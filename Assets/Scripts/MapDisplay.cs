using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
	[SerializeField]
	private Renderer textureRender;

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public void DrawTexture(Texture2D texture)
	{
		textureRender.sharedMaterial.mainTexture = texture;
	}

	public void DrawMesh(Mesh mesh)
	{
		meshFilter.sharedMesh = mesh;
	}

	public void DrawMesh(Mesh mesh, Texture2D texture)
	{
		DrawMesh(mesh);
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}
