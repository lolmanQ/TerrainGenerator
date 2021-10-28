using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
	public override void OnInspectorGUI()
	{
		ProceduralMesh proceduralMesh = (ProceduralMesh)target;

		if (DrawDefaultInspector())
		{
			if (proceduralMesh.autoUpdate)
			{
				proceduralMesh.GenerateMesh();
			}
		}

		if (GUILayout.Button("Generate mesh"))
		{
			proceduralMesh.GenerateMesh();
		}

		//if(GUILayout.Button("Generate color map"))
		//{
		//	worldGenerator.GenerateNoiseMap();
		//}
	}
}
