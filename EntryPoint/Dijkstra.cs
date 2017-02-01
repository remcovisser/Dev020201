using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	public class Dijkstra
	{
		public Vector2 start { get; }
		public Vector2 end { get; }
		public List<Tuple<Vector2, Vector2>> vertexes { get; }
		public int[][] graph { get; }

		public Dijkstra(Vector2 _start, Vector2 _end, List<Tuple<Vector2, Vector2>> _vertexes)
		{
			start = _start;
			end = _end;
			vertexes = _vertexes;
			graph = new int[vertexes.Count][];
			CreateAdjacancyMatrix();
		}

		// Create an adjancancy matrix, for each roads it sets its adjacent rows to 1, all other roads are 0
		private void CreateAdjacancyMatrix()
		{
			for (var i = 0; i < vertexes.Count; i++)
			{
				var tempArray = new int[vertexes.Count];
				var x = 0;

				foreach (Tuple<Vector2, Vector2> road in vertexes)
				{
					tempArray[x] = 0;
					if (i != x)
					{
						// If either the beginning or the end of the road and the current vertex adjace -> set to 1 else 0
						if(vertexes[i].Item1 == road.Item1 || vertexes[i].Item2 == road.Item2)
							tempArray[x] = 1;
						else
							tempArray[x] = 0;
					}
					x++;
				}

				graph[i] = tempArray;
			}
		}

		public List<Tuple<Vector2, Vector2>> FindFastestPath()
		{
			// Create the 2 indexing values, set a default value of -1
			int startingIndex = -1;
			int destinationIndex = -1;

			// Find the starting and destination index in the vertexes
			for (int i = 0; i < vertexes.Count; i++)
			{
				if (start == vertexes[i].Item1)
					startingIndex = i;

				if (end == vertexes[i].Item2)
					destinationIndex = i;
			}

			// Find the fastest route, transform the result of the algoritm into a list of vectors
			List<int> prev = DijkstraAlgorithm(startingIndex, destinationIndex);
			List<Tuple<Vector2, Vector2>> result = new List<Tuple<Vector2, Vector2>>();
			foreach (int i in prev)
			{
				result.Add(vertexes[i]);
			}

			return result;
		}


		private List<int> DijkstraAlgorithm(int startingIndex, int destinationIndex)
		{
			int graphSize = graph.Length;
			int[] dist = new int[graphSize];
			List<int>[] prev = new List<int>[graphSize];
			var visited = new bool[graphSize];

			// Set default values
			for (int i = 0; i < graphSize; i++)
			{
				dist[i] = int.MaxValue;
				prev[i] = new List<int>();
				visited[i] = false;
			}

			dist[startingIndex] = 0;

			for (int count = 0; count < graphSize - 1; count++)
			{
				int smallest = int.MaxValue;
				int u = -1;

				// Find the closest node
				for (int i = 0; i < graphSize; i++)
				{
					if (visited[i] == false && dist[i] <= smallest)
					{
						smallest = dist[i];
						u = i;
					}
				}
				visited[u] = true;

				// Find the shortest path
				for (int i = 0; i < graphSize; i++)
				{
					if (!visited[i] && graph[u][i] != 0 && dist[u] != int.MaxValue && dist[u] + graph[u][i] < dist[i])
					{
						dist[i] = dist[u] + graph[u][i];
						prev[i].AddRange(prev[u]);
						prev[i].Add(u);
					}
				}
			}

			// TODO: temp fix, find the closest path to the destination of no path is found for some reason
			for (int i = destinationIndex; i < (graphSize-1); i++) 
			{
				if (prev[destinationIndex].Count == 0)
					destinationIndex++;
			}
			return prev[destinationIndex];
		}
	}
}

