using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		WorldGenerator worldGenerator = (WorldGenerator)target;
		
		if (DrawDefaultInspector())
		{
			if (worldGenerator.autoUpdate)
			{
				worldGenerator.GenAndSetMesh();
			}
		}

		if (GUILayout.Button("Generate mesh"))
		{
			worldGenerator.GenAndSetMesh();
		}

		if(GUILayout.Button("Generate new seed"))
		{
			worldGenerator.SetSeed(Random.Range(0, 100000));
			worldGenerator.GenAndSetMesh();
		}

		if(GUILayout.Button("Generate color map"))
		{
			worldGenerator.GenerateNoiseMap();
		}
	}
}
