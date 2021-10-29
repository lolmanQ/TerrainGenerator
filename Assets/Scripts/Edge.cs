using System;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
	public Vector3 vertexA;
	public Vector3 vertexB;

	Vector3 subVertex;

	public List<Edge> subEdges = new List<Edge>();

	public void SubDevide()
	{
		if(subEdges.Count > 0)
		{
			//Debug.LogWarning("Line has already been sub devided");
			return;
		}
		

		subVertex = Vector3.Lerp(vertexA, vertexB, 0.5f);

		if (ProceduralMesh.VertexPointer.ContainsKey(subVertex))
		{
			Debug.LogWarning("When making new vertex there already existed one");
		}
		else
		{
			ProceduralMesh.VertexList.Add(subVertex);
			ProceduralMesh.VertexPointer.Add(subVertex, ProceduralMesh.VertexList.Count - 1);
		}

		subEdges.Add(new Edge(vertexA, subVertex));
		subEdges.Add(new Edge(subVertex, vertexB));
	}

	public void ReGroup()
	{
		if (subEdges.Count > 0)
		{
			for (int i = 0; i < subEdges.Count; i++)
			{
				subEdges[i].ReGroup();
			}
		}

		ProceduralMesh.VertexList.Remove(subVertex);
		ProceduralMesh.VertexPointer.Remove(subVertex);
	}

	public int[] GenerateTriangle(Vector3 vertexC, bool invert)
	{
		int[] tri = new int[3];
		if (invert)
		{
			tri = new int[]
			{
				ProceduralMesh.VertexPointer[vertexB],
				ProceduralMesh.VertexPointer[vertexC],
				ProceduralMesh.VertexPointer[vertexA]
			};
		}
		else
		{
			tri = new int[]
			{
				ProceduralMesh.VertexPointer[vertexC],
				ProceduralMesh.VertexPointer[vertexB],
				ProceduralMesh.VertexPointer[vertexA]
			};
		}
		

		return tri;
	}

	public Edge(Vector3 vertexA, Vector3 vertexB)
	{
		this.vertexA = vertexA;
		this.vertexB = vertexB;
	}

	public Edge(EdgePointer edgePointer)
	{
		vertexA = edgePointer.vertexA;
		vertexB = edgePointer.vertexB;
	}
}

public struct EdgePointer
{
	public Vector3 vertexA;
	public Vector3 vertexB;

	public EdgePointer(Vector3 vertexA, Vector3 vertexB)
	{
		this.vertexA = vertexA;
		this.vertexB = vertexB;
	}

	public EdgePointer(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		vertexA = new Vector3(x1, y1, z1);
		vertexB = new Vector3(x2, y2, z2);
	}
}
