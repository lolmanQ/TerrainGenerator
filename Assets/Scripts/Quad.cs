using System;
using System.Collections.Generic;
using UnityEngine;

public class Quad
{
	Vector3 mainVertex;
	Edge[] edges;
	public Quad[] subQuads;

	public void SubDevide()
	{
		foreach (Edge edge in edges)
		{
			edge.SubDevide();
		}

		subQuads = new Quad[4];

		subQuads[0] = new Quad(new Edge[]
		{
			edges[0].subEdges[0],
			new Edge(edges[0].subEdges[0].vertexB, mainVertex),
			new Edge(mainVertex, edges[3].subEdges[1].vertexA),
			edges[3].subEdges[1]
		});

		subQuads[1] = new Quad(new Edge[]
		{
			edges[0].subEdges[1],
			edges[1].subEdges[0],
			new Edge(edges[1].subEdges[0].vertexB, mainVertex),
			new Edge(mainVertex, edges[0].subEdges[0].vertexB)
		});

		subQuads[2] = new Quad(new Edge[]
		{
			edges[1].subEdges[1],
			edges[2].subEdges[0],
			new Edge(edges[2].subEdges[0].vertexB, mainVertex),
			new Edge(mainVertex, edges[1].subEdges[0].vertexB)
		});

		subQuads[3] = new Quad(new Edge[]
		{
			edges[2].subEdges[1],
			edges[3].subEdges[0],
			new Edge(edges[3].subEdges[0].vertexB, mainVertex),
			new Edge(mainVertex, edges[2].subEdges[0].vertexB)
		});
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
				triList.AddRange(GenerateTriFromEdge(edges[i]));
			}
		}

		return triList.ToArray();
	}

	int[] GenerateTriFromEdge(Edge edge)
	{
		List<int> triList = new List<int>();
		if (edge.subEdges.Count > 0)
		{
			for (int i = 0; i < edge.subEdges.Count; i++)
			{
				triList.AddRange(GenerateTriFromEdge(edge.subEdges[i]));
			}
		}
		else
		{
			triList.AddRange(edge.GenerateTriangle(mainVertex));
		}
		return triList.ToArray();
	}


	public Quad(Edge[] edges)
	{
		this.edges = edges;
		subQuads = new Quad[0];

		mainVertex = Vector3.Lerp(edges[0].vertexA, edges[2].vertexA, 0.5f);

		ProceduralMesh.VertexList.Add(mainVertex);
		ProceduralMesh.VertexPointer.Add(mainVertex, ProceduralMesh.VertexList.Count - 1);
	}
}

