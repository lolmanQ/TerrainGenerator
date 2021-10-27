using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHelper : MonoBehaviour
{
	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private Gradient colorGradient;

	public void DrawNoiseMap(float[,] noiseMap)
	{
		int width = noiseMap.GetLength(0);
		int height = noiseMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);

		texture.filterMode = FilterMode.Point;

		Color[] colorMap = new Color[width * height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colorMap[x + y * width] = colorGradient.Evaluate(noiseMap[x, y]);
			}
		}

		texture.SetPixels(colorMap);
		texture.Apply();

		renderer.sharedMaterial.mainTexture = texture;
		//renderer.transform.localScale = new Vector3(width, 1, height);
	}
}
