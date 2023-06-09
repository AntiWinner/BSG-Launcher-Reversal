using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bsg.Math.Graphs.Dijkstra;

public class DijkstraAlgorithm<T> where T : IEquatable<T>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GraphVertex<T> _E000;

		public Func<GraphEdge<T>, bool> _E001;

		internal bool _E000(GraphEdge<T> e)
		{
			return e.TargetVertex == this._E000;
		}
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly Graph<T> m__E000;

	public Graph<T> Graph
	{
		[CompilerGenerated]
		get
		{
			return this._E000;
		}
	}

	public DijkstraAlgorithm(Graph<T> graph)
	{
		this._E000 = graph;
	}

	public LinkedList<GraphVertex<T>> FindShortestPath(GraphVertex<T> startVertex, GraphVertex<T> finishVertex, Func<GraphVertex<T>, bool> filter = null)
	{
		IDictionary<T, GraphVertexInfo<T>> verticesMap = _E000(startVertex, filter);
		return this._E000(verticesMap, startVertex, finishVertex);
	}

	public LinkedList<GraphEdge<T>> FindShortestPathEdges(GraphVertex<T> startVertex, GraphVertex<T> finishVertex, Func<GraphVertex<T>, bool> filter = null)
	{
		IDictionary<T, GraphVertexInfo<T>> verticesMap = _E000(startVertex, filter);
		return this._E000(verticesMap, startVertex, finishVertex);
	}

	private IDictionary<T, GraphVertexInfo<T>> _E000(GraphVertex<T> startVertex, Func<GraphVertex<T>, bool> filter = null)
	{
		Dictionary<T, GraphVertexInfo<T>> dictionary = Graph.Vertices.Select((GraphVertex<T> v) => new GraphVertexInfo<T>(v)).ToDictionary((GraphVertexInfo<T> k) => k.Vertex.Value);
		GraphVertexInfo<T> graphVertexInfo = _E000(dictionary, startVertex);
		graphVertexInfo.EdgesWeightSum = 0;
		while (true)
		{
			GraphVertexInfo<T> graphVertexInfo2 = _E000(dictionary);
			if (graphVertexInfo2 == null)
			{
				break;
			}
			graphVertexInfo2.IsVisited = true;
			if (filter != null && filter(graphVertexInfo2.Vertex))
			{
				break;
			}
			_E000(dictionary, graphVertexInfo2);
		}
		return dictionary;
	}

	private GraphVertexInfo<T> _E000(IDictionary<T, GraphVertexInfo<T>> verticesMap, GraphVertex<T> v)
	{
		if (!verticesMap.TryGetValue(v.Value, out var value))
		{
			throw new DijkstraException(string.Format(_E05B._E000(2370), v));
		}
		return value;
	}

	private GraphVertexInfo<T> _E000(IDictionary<T, GraphVertexInfo<T>> verticesMap)
	{
		int num = int.MaxValue;
		GraphVertexInfo<T> result = null;
		foreach (GraphVertexInfo<T> value in verticesMap.Values)
		{
			if (!value.IsVisited && value.EdgesWeightSum < num)
			{
				result = value;
				num = value.EdgesWeightSum;
			}
		}
		return result;
	}

	private void _E000(IDictionary<T, GraphVertexInfo<T>> verticesMap, GraphVertexInfo<T> info)
	{
		foreach (GraphEdge<T> edge in info.Vertex.Edges)
		{
			GraphVertexInfo<T> graphVertexInfo = _E000(verticesMap, edge.TargetVertex);
			int num = info.EdgesWeightSum + edge.EdgeWeight;
			if (num < graphVertexInfo.EdgesWeightSum)
			{
				graphVertexInfo.EdgesWeightSum = num;
				graphVertexInfo.PreviousVertex = info.Vertex;
			}
		}
	}

	private LinkedList<GraphVertex<T>> _E000(IDictionary<T, GraphVertexInfo<T>> verticesMap, GraphVertex<T> startVertex, GraphVertex<T> endVertex)
	{
		LinkedList<GraphVertex<T>> linkedList = new LinkedList<GraphVertex<T>>();
		linkedList.AddFirst(endVertex);
		while (startVertex != endVertex)
		{
			endVertex = _E000(verticesMap, endVertex).PreviousVertex ?? throw new DijkstraException(string.Format(_E05B._E000(2471), startVertex, endVertex));
			linkedList.AddFirst(endVertex);
		}
		return linkedList;
	}

	private LinkedList<GraphEdge<T>> _E000(IDictionary<T, GraphVertexInfo<T>> verticesMap, GraphVertex<T> startVertex, GraphVertex<T> endVertex)
	{
		LinkedList<GraphEdge<T>> linkedList = new LinkedList<GraphEdge<T>>();
		while (startVertex != endVertex)
		{
			GraphVertex<T> graphVertex = _E000(verticesMap, endVertex).PreviousVertex ?? throw new DijkstraException(string.Format(_E05B._E000(2471), startVertex, endVertex));
			linkedList.AddFirst(graphVertex.Edges.FirstOrDefault((GraphEdge<T> e) => e.TargetVertex == endVertex) ?? throw new DijkstraException(string.Format(_E05B._E000(2555), graphVertex, endVertex)));
			endVertex = graphVertex;
		}
		return linkedList;
	}
}
