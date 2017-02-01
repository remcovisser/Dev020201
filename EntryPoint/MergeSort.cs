using System;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	class MergeSort
	{
		public static void Sort(int start, int end, Vector2 house, Vector2[] specialBuildings)
		{
			if (start < end)
			{
				int middle = (start + end) / 2;

				Sort(start, middle, house, specialBuildings);
				Sort(middle + 1, end, house, specialBuildings);
				Merge(start, middle, end, house, specialBuildings);
			}
		}

		public static void Merge(int start, int middle, int end, Vector2 house, Vector2[] specialBuildings)
		{
			// Calculate the length of both arrays
			int lenghtLeftSide = middle - start + 1;
			int lengthRightSide = end - middle;

			Vector2[] leftSide = new Vector2[lenghtLeftSide];
			Vector2[] rightSide = new Vector2[lengthRightSide];

			// Fill the left side array with the correct values
			for (int i = 0; i < lenghtLeftSide; i++)
			{
				leftSide[i] = specialBuildings[start + i];
			}
			//  +1 Because the left side stops at the middle, the right starts after the middle
			for (int i = 0; i < lengthRightSide; i++)
			{
				rightSide[i] = specialBuildings[middle + i + 1];
			}

			// Create indexing variable
			int index = start;
			int indexLeft = 0;
			int indexRight = 0;

			// As long as the program has not reached one of arrays end
			while (indexLeft < lenghtLeftSide && indexRight < lengthRightSide)
			{
				// Add the lowest distance to the result
				if (Distance(leftSide[indexLeft], house) < Distance(rightSide[indexRight], house))
				{
					specialBuildings[index] = leftSide[indexLeft];
					indexLeft++;
				}
				else {
					specialBuildings[index] = rightSide[indexRight];
					indexRight++;
				}
				index++;
			}

			// As long as there are values in the array, add them to the end of the result
			while (indexLeft < lenghtLeftSide)
			{
				specialBuildings[index] = leftSide[indexLeft];
				indexLeft++;
				index++;
			}

			while (indexRight < lengthRightSide)
			{
				specialBuildings[index] = rightSide[indexRight];
				indexRight++;
				index++;
			}
		}

		// Calculate the distance from the house to the specialbuilding
		public static float Distance(Vector2 specialBuilding, Vector2 house)
		{
			return (float)Math.Sqrt(Math.Pow(specialBuilding.X - house.X, 2) + Math.Pow(specialBuilding.Y - house.Y, 2));
		}
	}
}