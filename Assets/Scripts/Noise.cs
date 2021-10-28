using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings noiseSettings)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random(noiseSettings.seed);
		Vector2[] octaveOffsets = new Vector2[noiseSettings.octaves];
		for (int i = 0; i < noiseSettings.octaves; i++)
		{
			float offsetX = prng.Next(-10000, 10000) + noiseSettings.offset.x;
			float offsetY = prng.Next(-10000, 10000) + noiseSettings.offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}


		if(noiseSettings.scale <= 0)
		{
			noiseSettings.scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < noiseSettings.octaves; i++)
				{
					float sampleX = (x - halfWidth) / noiseSettings.scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / noiseSettings.scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= noiseSettings.persistance;
					frequency *= noiseSettings.lacunarity;
				}

				if (noiseHeight > maxNoiseHeight)
					maxNoiseHeight = noiseHeight;
				if (noiseHeight < minNoiseHeight)
					minNoiseHeight = noiseHeight;

				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}
}

[System.Serializable]
public struct NoiseSettings
{
	public float scale;
	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;
}

//[System.Serializable]
//public struct NoisePass
//{
//	public bool active;
//	public float frequency;
//	public float amplitude;
//	public int seedOffset;
//	public float heightOffset;
//}