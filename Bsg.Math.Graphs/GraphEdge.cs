using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bsg.Math.Graphs;

public class GraphEdge<T> where T : IEquatable<T>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphVertex<T> _E000;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphVertex<T> _E001;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int _E002;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private object _E003;

	public GraphVertex<T> SourceVertex
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public GraphVertex<T> TargetVertex
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public int EdgeWeight
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public object Tag
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

	internal GraphEdge(GraphVertex<T> sourceVertex, GraphVertex<T> targetVertex, int weight, object tag)
	{
		_E000 = sourceVertex;
		_E001 = targetVertex;
		_E002 = weight;
		Tag = tag;
	}

	public override string ToString()
	{
		return string.Format(_E05B._E000(2799), SourceVertex, TargetVertex, (Tag == null) ? "" : string.Format(_E05B._E000(2811), Tag));
	}
}
