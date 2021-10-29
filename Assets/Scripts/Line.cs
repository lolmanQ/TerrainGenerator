using System;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
	public Vector3 vertexA;
	public Vector3 vertexB;

	public List<Edge> subEdges = new List<Edge>();

	public void SubDevide()
	{
		if(subEdges.Count > 0)
		{
			Debug.LogWarning("Line has already been sub devided");
		}

		Vector3 newVertex = Vector3.Lerp(vertexA, vertexB, 0.5f);

		if (ProceduralMesh.VertexPointer.ContainsKey(newVertex))
		{
			Debug.LogWarning("When making new vertex there already existed one");
		}
		else
		{
			ProceduralMesh.VertexList.Add(newVertex);
			ProceduralMesh.VertexPointer.Add(newVertex, ProceduralMesh.VertexList.Count - 1);
		}

		subEdges.Add(new Edge(vertexA, newVertex));
		subEdges.Add(new Edge(newVertex, vertexB));
	}

	public int[] GenerateTriangle(Vector3 vertexC)
	{
		int[] tri = new int[]
		{
			ProceduralMesh.VertexPointer[vertexC],
			ProceduralMesh.VertexPointer[vertexB],
			ProceduralMesh.VertexPointer[vertexA]
		};

		return tri;
	}

	public Edge(Vector3 vertexA, Vector3 vertexB)
	{
		this.vertexA = vertexA;
		this.vertexB = vertexB;
	}
}
