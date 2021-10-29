using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
	public enum NormalizeMode { Local, Global}

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings noiseSettings)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random(noiseSettings.seed);
		Vector2[] octaveOffsets = new Vector2[noiseSettings.octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < noiseSettings.octaves; i++)
		{
			float offsetX = prng.Next(-10000, 10000) + noiseSettings.offset.x;
			float offsetY = prng.Next(-10000, 10000) + noiseSettings.offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= noiseSettings.persistance;
		}


		if(noiseSettings.scale <= 0)
		{
			noiseSettings.scale = 0.0001f;
		}

		float localMaxNoiseHeight = float.MinValue;
		float localMinNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < noiseSettings.octaves; i++)
				{
					float sampleX = (x - halfWidth + octaveOffsets[i].x) / noiseSettings.scale * frequency;
					float sampleY = (y - halfHeight + octaveOffsets[i].y) / noiseSettings.scale * frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= noiseSettings.persistance;
					frequency *= noiseSettings.lacunarity;
				}

				if (noiseHeight > localMaxNoiseHeight)
					localMaxNoiseHeight = noiseHeight;
				if (noiseHeight < localMinNoiseHeight)
					localMinNoiseHeight = noiseHeight;

				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				if(noiseSettings.normalizeMode == NormalizeMode.Local)
				{
					noiseMap[x, y] = Mathf.InverseLerp(localMinNoiseHeight, localMaxNoiseHeight, noiseMap[x, y]);
				}
				else
				{
					float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.75f);
					noiseMap[x, y] = normalizedHeight;
				}
			}
		}

		return noiseMap;
	}

	public static float GetPoint(float x, float y, NoiseSettings settings)
	{
		System.Random prng = new System.Random(settings.seed);
		Vector2[] octaveOffsets = new Vector2[settings.octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < settings.octaves; i++)
		{
			float offsetX = prng.Next(-10000, 10000) + settings.offset.x;
			float offsetY = prng.Next(-10000, 10000) + settings.offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;
		}


		if (settings.scale <= 0)
		{
			settings.scale = 0.0001f;
		}
		
		amplitude = 1;
		frequency = 1;
		float noiseHeight = 0;

		for (int i = 0; i < settings.octaves; i++)
		{
			float sampleX = (x + octaveOffsets[i].x) / settings.scale * frequency;
			float sampleY = (y + octaveOffsets[i].y) / settings.scale * frequency;

			float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
			noiseHeight += perlinValue * amplitude;

			amplitude *= settings.persistance;
			frequency *= settings.lacunarity;
		}


		//for (int y = 0; y < mapHeight; y++)
		//{
		//	for (int x = 0; x < mapWidth; x++)
		//	{
		//		if (noiseSettings.normalizeMode == NormalizeMode.Local)
		//		{
		//			noiseMap[x, y] = Mathf.InverseLerp(localMinNoiseHeight, localMaxNoiseHeight, noiseMap[x, y]);
		//		}
		//		else
		//		{
		//			float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.75f);
		//			noiseMap[x, y] = normalizedHeight;
		//		}
		//	}
		//}

		return noiseHeight;
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
	public Noise.NormalizeMode normalizeMode;
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