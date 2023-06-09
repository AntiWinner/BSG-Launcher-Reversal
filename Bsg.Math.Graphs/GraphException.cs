using System;

namespace Bsg.Math.Graphs;

public class GraphException : Exception
{
	public GraphException(string message)
		: base(message)
	{
	}

	public GraphException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
