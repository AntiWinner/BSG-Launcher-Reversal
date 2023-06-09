using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bsg.Math.Graphs;

public class GraphVertex<T> where T : IEquatable<T>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly T _E000;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private object _E001;

	private readonly HashSet<GraphEdge<T>> _E002;

	public T Value
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public object Tag
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public IReadOnlyCollection<GraphEdge<T>> Edges => _E002;

	internal GraphVertex(T value, object tag)
	{
		_E000 = value;
		Tag = tag;
		_E002 = new HashSet<GraphEdge<T>>();
	}

	public void AddEdge(GraphVertex<T> connectedVertex, int edgeWeight, object tag = null)
	{
		GraphEdge<T> graphEdge = new GraphEdge<T>(this, connectedVertex, edgeWeight, tag);
		if (_E002.Add(graphEdge))
		{
			return;
		}
		throw new GraphException(string.Format(_E05B._E000(2335), graphEdge, this));
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}
