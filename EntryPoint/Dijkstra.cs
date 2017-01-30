using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
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
			var startingIndex = -1;
			var destinationIndex = -1;

			var index = 0;
			foreach (var road in vertexes)
			{
				if (start == road.Item1)
					startingIndex = index;

				if (end == road.Item2)
					destinationIndex = index;

				index++;
			}

			var prev = DijkstraAlgorithm(startingIndex);

			var result = new List<Tuple<Vector2, Vector2>>();
			foreach (int i in prev[destinationIndex])
			{
				result.Add(vertexes[i]);
			}

			return result;
		}


		private List<int>[] DijkstraAlgorithm(int startingIndex)
		{
			int graphSize = graph.Length;
			int[] dist = new int[graphSize];
			List<int>[] prev = new List<int>[graphSize];
			var visited = new bool[graphSize];

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
				for (int i = 0; i < graphSize; i++)
				{
					if (visited[i] == false && dist[i] <= smallest)
					{
						smallest = dist[i];
						u = i;
					}
				}
				visited[u] = true;

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
			return prev;
		}
	}
}

