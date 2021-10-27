using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
	[SerializeField]
	private Renderer textureRender;

	public void DrawTexture(Texture2D texture)
	{
		textureRender.sharedMaterial.mainTexture = texture;
	}
}
