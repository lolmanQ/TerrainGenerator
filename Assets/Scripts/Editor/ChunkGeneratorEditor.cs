using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkGenerator))]
public class ChunkGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		ChunkGenerator worldGenerator = (ChunkGenerator)target;
		
		if (DrawDefaultInspector())
		{
			if (worldGenerator.autoUpdate)
			{
				worldGenerator.GenerateMap();
			}
		}

		if (GUILayout.Button("Generate mesh"))
		{
			worldGenerator.GenerateMap();
		}

		if(GUILayout.Button("Generate new seed"))
		{
			worldGenerator.SetSeed(Random.Range(0, 100000));
			worldGenerator.GenerateMap();
		}

		//if(GUILayout.Button("Generate color map"))
		//{
		//	worldGenerator.GenerateNoiseMap();
		//}
	}
}
