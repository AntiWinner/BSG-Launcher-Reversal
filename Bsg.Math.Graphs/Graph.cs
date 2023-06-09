using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bsg.Math.Graphs;

public class Graph<T> where T : IEquatable<T>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public T _E000;

		internal bool _E000(GraphVertex<T> v)
		{
			return v.Value.Equals(this._E000);
		}
	}

	private readonly HashSet<GraphVertex<T>> m__E000 = new HashSet<GraphVertex<T>>();

	public IReadOnlyCollection<GraphVertex<T>> Vertices => this._E000;

	public GraphVertex<T> AddVertex(T value, object tag = null)
	{
		GraphVertex<T> graphVertex = new GraphVertex<T>(value, tag);
		if (!this._E000.Add(graphVertex))
		{
			throw new GraphException(string.Format(_E05B._E000(2690), value));
		}
		return graphVertex;
	}

	public Graph<T> AddVertices(IEnumerable<T> values)
	{
		foreach (T value in values)
		{
			AddVertex(value);
		}
		return this;
	}

	public GraphVertex<T> FindVertex(T value)
	{
		return this._E000.FirstOrDefault((GraphVertex<T> v) => v.Value.Equals(value));
	}

	public Graph<T> TwoWayConnect(T firstValue, T secondValue, int weight)
	{
		GraphVertex<T> graphVertex = FindVertex(firstValue) ?? AddVertex(firstValue);
		GraphVertex<T> graphVertex2 = FindVertex(secondValue) ?? AddVertex(secondValue);
		graphVertex.AddEdge(graphVertex2, weight);
		graphVertex2.AddEdge(graphVertex, weight);
		return this;
	}

	public Graph<T> OneWayConnect(T firstValue, T secondValue, int weight, object tag = null)
	{
		GraphVertex<T> graphVertex = FindVertex(firstValue) ?? AddVertex(firstValue);
		GraphVertex<T> connectedVertex = FindVertex(secondValue) ?? AddVertex(secondValue);
		graphVertex.AddEdge(connectedVertex, weight, tag);
		return this;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (GraphVertex<T> item in this._E000)
		{
			stringBuilder.Append(item.Value);
			stringBuilder.Append(_E05B._E000(2794));
			stringBuilder.Append(string.Join(_E05B._E000(27867), item.Edges));
			stringBuilder.AppendLine(_E05B._E000(15327));
		}
		return stringBuilder.ToString();
	}
}
