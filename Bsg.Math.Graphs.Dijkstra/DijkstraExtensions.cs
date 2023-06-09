using System;
using System.Collections.Generic;

namespace Bsg.Math.Graphs.Dijkstra;

public static class DijkstraExtensions
{
	public static LinkedList<GraphVertex<T>> FindShortestPath<T>(this DijkstraAlgorithm<T> dijkstra, T startValue, T finishValue, Func<GraphVertex<T>, bool> filter = null) where T : IEquatable<T>
	{
		return dijkstra.FindShortestPath(dijkstra.Graph.FindRequiredVertex(startValue), dijkstra.Graph.FindRequiredVertex(finishValue), filter);
	}

	public static bool TryFindShortestPath<T>(this DijkstraAlgorithm<T> dijkstra, T startValue, T finishValue, out LinkedList<GraphVertex<T>> result, Func<GraphVertex<T>, bool> filter = null) where T : IEquatable<T>
	{
		if (dijkstra.Graph.TryFindVertex(startValue, out var _) && dijkstra.Graph.TryFindVertex(finishValue, out var _))
		{
			try
			{
				result = dijkstra.FindShortestPath(startValue, finishValue, filter);
				return true;
			}
			catch (DijkstraException)
			{
			}
		}
		result = null;
		return false;
	}

	public static LinkedList<GraphEdge<T>> FindShortestPathEdges<T>(this DijkstraAlgorithm<T> dijkstra, T startValue, T finishValue, Func<GraphVertex<T>, bool> filter = null) where T : IEquatable<T>
	{
		return dijkstra.FindShortestPathEdges(dijkstra.Graph.FindRequiredVertex(startValue), dijkstra.Graph.FindRequiredVertex(finishValue), filter);
	}

	public static bool TryFindShortestPathEdges<T>(this DijkstraAlgorithm<T> dijkstra, T startValue, T finishValue, out LinkedList<GraphEdge<T>> result, Func<GraphVertex<T>, bool> filter = null) where T : IEquatable<T>
	{
		if (dijkstra.Graph.TryFindVertex(startValue, out var _) && dijkstra.Graph.TryFindVertex(finishValue, out var _))
		{
			try
			{
				result = dijkstra.FindShortestPathEdges(startValue, finishValue, filter);
				return true;
			}
			catch (DijkstraException)
			{
			}
		}
		result = null;
		return false;
	}
}
