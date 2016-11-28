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
      //return specialBuildings.OrderBy(v => Vector2.Distance(v, house));

			// House -> Vector2
			// SpecialBuildings -> List(50) of Vector2
		

			int[] numbers = { 8, 3, 2, 9, 7, 1, 5, 4 };
			mergeSort(numbers, 0, numbers.Length - 1);

			return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
    }

	static public void mergeSort(int[] numbers, int start, int end)
	{
		if (start < end)
		{
			int middle = (start + end) / 2;
			mergeSort(numbers, start, middle);
			mergeSort(numbers, (middle + 1), end);

			merge(numbers, start, (middle + 1), end);
		}
	}


    static public void merge(int[] numbers, int start, int middle, int end)
	{
			int i;
			for (i = 0; i < numbers.Length; i++)
			{
				if (numbers[start] > numbers[end])
				{
					int tempEnd = numbers[end];
					numbers[end] = numbers[start];
					numbers[start] = tempEnd;
				}
			}
			// Output 38 29 17 45

			/* 
			 * Working implementation from internet
			 * 
				int[] temp = new int[numbers.Length];
				int i, left_end, num_elements, tmp_pos;

				left_end = (middle - 1);
				tmp_pos = start;
				num_elements = (end - start + 1);

				while ((start <= left_end) && (middle <= end))
				{
					if (numbers[start] <= numbers[middle])
						temp[tmp_pos++] = numbers[start++];
					else
						temp[tmp_pos++] = numbers[middle++];
				}

				while (start <= left_end)
					temp[tmp_pos++] = numbers[start++];

				while (middle <= end)
					temp[tmp_pos++] = numbers[middle++];

				for (i = 0; i < num_elements; i++)
				{
					numbers[end] = temp[end];
					end--;
				}
		*/
	}


    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
      return
          from h in housesAndDistances
          select
            from s in specialBuildings
            where Vector2.Distance(h.Item1, s) <= h.Item2
            select s;
    }

    private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      var prevRoad = startingRoad;
      for (int i = 0; i < 30; i++)
      {
        prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
        fakeBestPath.Add(prevRoad);
      }
      return fakeBestPath;
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
