using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bsg.Math.Graphs.Dijkstra;

public class GraphVertexInfo<T> where T : IEquatable<T>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphVertex<T> _E000;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool _E001;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int _E002;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private GraphVertex<T> _E003;

	public GraphVertex<T> Vertex
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public bool IsVisited
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

	public int EdgesWeightSum
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public GraphVertex<T> PreviousVertex
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public GraphVertexInfo(GraphVertex<T> vertex)
	{
		_E000 = vertex;
		EdgesWeightSum = int.MaxValue;
	}
}
