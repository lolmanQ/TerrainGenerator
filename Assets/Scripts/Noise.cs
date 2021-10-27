using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings noiseSettings)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;


		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float noiseHeight = 0;

				foreach (NoisePass pass in noiseSettings.passes)
				{
					if (!pass.active)
						continue;

					float sampleX = pass.seedOffset + noiseSettings.seed + x / noiseSettings.scale * pass.frequency;
					float sampleY = pass.seedOffset + noiseSettings.seed + y / noiseSettings.scale * pass.frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * pass.amplitude;
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
	public int seed;
	public float scale;

	public List<NoisePass> passes;
}

[System.Serializable]
public struct NoisePass
{
	public bool active;
	public float frequency;
	public float amplitude;
	public int seedOffset;
	public float heightOffset;
}