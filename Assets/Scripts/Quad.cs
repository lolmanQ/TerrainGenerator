using System;
using System.Collections.Generic;
using UnityEngine;

public class Quad
{
	public Vector3 mainVertex;
	Edge[] edges;
	public Quad[] subQuads;
	float quadArea;

	public void SubDevide()
	{
		if(subQuads.Length > 0)
		{
			foreach (Quad item in subQuads)
			{
				item.SubDevide();
			}
			return;
		}

		foreach (Edge edge in edges)
		{
			edge.SubDevide();
		}

		Edge innerEdgeDown = new Edge(edges[0].subEdges[0].vertexB, mainVertex);
		Edge innerEdgeRight = new Edge(mainVertex, edges[1].subEdges[0].vertexB);
		Edge innerEdgeLeft = new Edge(edges[3].subEdges[0].vertexB, mainVertex);
		Edge innerEdgeUp = new Edge(mainVertex, edges[2].subEdges[1].vertexA);


		subQuads = new Quad[4];

		subQuads[0] = new Quad(new Edge[]
		{
			edges[0].subEdges[0],
			innerEdgeDown,
			innerEdgeLeft,
			edges[3].subEdges[0]
		});

		subQuads[1] = new Quad(new Edge[]
		{
			edges[0].subEdges[1],
			edges[1].subEdges[0],
			innerEdgeRight,
			innerEdgeDown
		});

		subQuads[2] = new Quad(new Edge[]
		{
			innerEdgeRight,
			edges[1].subEdges[1],
			edges[2].subEdges[1],
			innerEdgeUp
		});

		subQuads[3] = new Quad(new Edge[]
		{
			innerEdgeLeft,
			innerEdgeUp,
			edges[2].subEdges[0],
			edges[3].subEdges[1],
		});
	}

	public void ReGroup()
	{
		for (int i = 0; i < subQuads.Length; i++)
		{
			if(subQuads[i] != null)
			{
				subQuads[i].ReGroup();
				subQuads[i].RemoveVertex();
			}
		}

		subQuads = new Quad[0];
	}

	public int[] GenerateTris()
	{
		List<int> triList = new List<int>();

		if(subQuads.Length > 0)
		{
			for (int i = 0; i < subQuads.Length; i++)
			{
				triList.AddRange(subQuads[i].GenerateTris());
			}
		}
		else
		{
			for (int i = 0; i < edges.Length; i++)
			{
				triList.AddRange(GenerateTriFromEdge(edges[i], i > 1));
			}
		}

		return triList.ToArray();
	}

	int[] GenerateTriFromEdge(Edge edge, bool invert)
	{
		List<int> triList = new List<int>();
		if (edge.subEdges.Count > 0)
		{
			for (int i = 0; i < edge.subEdges.Count; i++)
			{
				triList.AddRange(GenerateTriFromEdge(edge.subEdges[i], invert));
			}
		}
		else
		{
			triList.AddRange(edge.GenerateTriangle(mainVertex, invert));
		}
		return triList.ToArray();
	}

	public void QuadUpdate()
	{
		float playerDistance = Vector3.Distance(Player.player.transform.position, mainVertex);
		

		if(quadArea > playerDistance / 2.5f && subQuads.Length == 0)
		{
			SubDevide();
		}
		else if(playerDistance / 3 > quadArea && subQuads.Length > 0)
		{
			ReGroup();
		}

		for (int i = 0; i < subQuads.Length; i++)
		{
			subQuads[i].QuadUpdate();
		}
	}

	public Quad(Edge[] edges)
	{
		this.edges = edges;
		subQuads = new Quad[0];

		mainVertex = Vector3.Lerp(Vector3.Lerp(edges[0].vertexA, edges[2].vertexB, 0.5f), Vector3.Lerp(edges[1].vertexA, edges[3].vertexB, 0.5f), 0.5f);
		quadArea = Vector3.Distance(edges[0].vertexA, edges[0].vertexB) * Vector3.Distance(edges[1].vertexA, edges[1].vertexB);

		if (!ProceduralMesh.VertexPointer.ContainsKey(mainVertex))
		{
			ProceduralMesh.VertexList.Add(mainVertex);
			ProceduralMesh.VertexPointer.Add(mainVertex, ProceduralMesh.VertexList.Count - 1);
		}
	}

	public void RemoveVertex()
	{
		//ProceduralMesh.VertexList.Remove(mainVertex);
		//ProceduralMesh.VertexPointer.Remove(mainVertex);
	}
}

