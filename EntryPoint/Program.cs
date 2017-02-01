using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
    [STAThread]
     static void Main()
	{
		var fullscreen = false;
		Console.WriteLine("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment");
		var level = Console.ReadLine();
		if (level == "1")
		{
			var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen);
			game.Run();
		}
		else if (level == "2")
		{
			var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen);
			game.Run();
		}
		else if (level == "3")
		{
			var game = VirtualCity.RunAssignment3(FindRoute, fullscreen);
			game.Run();
		}
		else if (level == "4")
		{
			var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen);
			game.Run();
		}
	}

    private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {
		// Set variable needed to call the Sort method
		int specialBuildingsLength = specialBuildings.Count() - 1;
		Vector2[] SpecialBuildings = specialBuildings.ToArray();
		// Sort the specialBuildings based on the distance from the house
		MergeSort.Sort(0, specialBuildingsLength, house, SpecialBuildings);
		return SpecialBuildings;
    }

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
			Tree tree = new Tree(specialBuildings.First());
			// Build the tree
			foreach (var specialBuidling in specialBuildings.Skip(1))
			{
				tree.insert(specialBuidling);
			}

			// Filter out the specialBuildings that are not in the range of a house
			foreach (Tuple<Vector2, float> houseAndDistance in housesAndDistances)
			{
				yield return tree.getAllSpecialpointsInRangeOfHouse(houseAndDistance.Item1, houseAndDistance.Item2);
			}
    }
	

		private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
		// Find the shortest path from the starting building to the destination building using roads only
		var dijkstra = new Dijkstra(startingBuilding, destinationBuilding, roads.ToList());
		return dijkstra.FindFastestPath();
    }

    private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
      IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
      foreach (var d in destinationBuildings)
      {
        var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
        List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
        var prevRoad = startingRoad;
        for (int i = 0; i < 30; i++)
        {
          prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
          fakeBestPath.Add(prevRoad);
        }
        result.Add(fakeBestPath);
      }
      return result;
    }
  }
#endif
}
