using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bsg.Math.Graphs;

public static class GraphExtensions
{
	[CompilerGenerated]
	private sealed class _E000<_E001> where _E001 : IEquatable<_E001>, IComparable<_E001>
	{
		public List<GraphVertex<_E001>> _E000;

		public Func<GraphEdge<_E001>, bool> _E001;

		internal bool _E000(GraphVertex<_E001> v)
		{
			return !this._E000.Contains(v) && v.Edges.All((GraphEdge<_E001> e) => this._E000.Contains(e.TargetVertex));
		}

		internal bool _E000(GraphEdge<_E001> e)
		{
			return this._E000.Contains(e.TargetVertex);
		}
	}

	[CompilerGenerated]
	private sealed class _E001<_E001> where _E001 : IEquatable<_E001>, IComparable<_E001>
	{
		public GraphVertex<_E001> _E000;

		internal bool _E000(GraphEdge<_E001> e)
		{
			return e.TargetVertex.Value.CompareTo(this._E000.Value) < 0;
		}
	}

	public static bool TryFindVertex<T>(this Graph<T> graph, T vertexValue, out GraphVertex<T> vertex) where T : IEquatable<T>
	{
		vertex = graph.FindVertex(vertexValue);
		return vertex != null;
	}

	public static GraphVertex<T> FindRequiredVertex<T>(this Graph<T> graph, T vertexValue) where T : IEquatable<T>
	{
		return graph.FindVertex(vertexValue) ?? throw new GraphException(string.Format(_E05B._E000(2812), vertexValue));
	}

	public static GraphVertex<T> FindLatestVertex<T>(this Graph<T> graph) where T : IEquatable<T>, IComparable<T>
	{
		GraphVertex<T> graphVertex = (from v in graph.Vertices
			where !v.Edges.Any()
			orderby v.Value descending
			select v).FirstOrDefault();
		if (graphVertex == null)
		{
			List<GraphVertex<T>> list = graph.FindDowngradedVertices().ToList();
			graphVertex = (from v in graph.Vertices
				where !list.Contains(v) && v.Edges.All((GraphEdge<T> e) => list.Contains(e.TargetVertex))
				orderby v.Value descending
				select v).FirstOrDefault() ?? throw new GraphException(_E05B._E000(2353));
		}
		return graphVertex;
	}

	public static IEnumerable<GraphVertex<T>> FindDowngradedVertices<T>(this Graph<T> graph) where T : IEquatable<T>, IComparable<T>
	{
		return graph.Vertices.Where((GraphVertex<T> v) => v.Edges.Any() && v.Edges.All((GraphEdge<T> e) => e.TargetVertex.Value.CompareTo(v.Value) < 0));
	}
}
