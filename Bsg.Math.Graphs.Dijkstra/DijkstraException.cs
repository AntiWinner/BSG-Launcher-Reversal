using System;

namespace Bsg.Math.Graphs.Dijkstra;

public class DijkstraException : GraphException
{
	public DijkstraException(string message)
		: base(message)
	{
	}

	public DijkstraException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
