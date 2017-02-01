using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EntryPoint
{
	public class Tree
	{
		public Node root;
		public Tree(Vector2 pos)
		{
			// Create the root of the tree
			root = new Node { position = pos };
		}

		// Method that can be called from outside the calls, triggers the private method in the class
		public void insert(Vector2 position)
		{
			root.insert(position, 0);
		}
			
		public class Node 
		{
			public Vector2 position {get; set;}
			public Node leftSide {get; set;}
			public Node rightSide {get; set;}

			// Determine of an item is on the left or right side based on the level and the positions -> left = true | right = false
			public bool determineSide(Vector2 nodePosition, int level)
			{
				if (level % 2 == 0)
					if (position.X > nodePosition.X)
						return true;
					else
						return false;
				else
					if (position.Y > nodePosition.Y)
						return true;
					else
						return false;
			}

			// Add the new node to the tree, recursivly call the insert untill there is no children more, then create a new node
			public Node insert(Vector2 pos, int level)
			{
				// Left
				if (determineSide(position, level))
					if (leftSide == null)
						leftSide = new Node { position = pos };
					else
						leftSide = leftSide.insert(pos, level + 1);
				// Right
				else
					if (rightSide == null)
						rightSide = new Node { position = pos };
					else
						rightSide = rightSide.insert(pos, level + 1);

				return this;
			}

			// Get all the specialBuildings within the square around a house
			public List<Vector2> createPointsInSquareList(Vector2 minPosition, Vector2 maxPosition, Node node, List<Vector2> specialBuildingsList = null, int level = 0)
			{
				// Create the specialBuildingsList, happens on the first call
				if (specialBuildingsList == null)
					specialBuildingsList = new List<Vector2>();
				
				// If specialbuildings position is within the square, add it to the list
				if (minPosition.X < node.position.X &&
				   	minPosition.Y < node.position.Y &&
				   	maxPosition.X > node.position.X &&
				    maxPosition.Y > node.position.Y)
				{
					specialBuildingsList.Add(node.position);
				}

				// Search the tree, based on the value of the specialbuilding deside to go left or right
				if (determineSide(position, level) && node.leftSide != null)
				{
					// Search left
					createPointsInSquareList(minPosition, maxPosition, node.leftSide, specialBuildingsList, level+1);
				}
				if (!determineSide(position, level) && node.rightSide != null)
				{
					// Search right
					createPointsInSquareList(minPosition, maxPosition, node.rightSide, specialBuildingsList, level+1);
				}

				return specialBuildingsList;
			}
		}

		public IEnumerable<Vector2> getAllSpecialpointsInRangeOfHouse(Vector2 housePosition, float range)
		{
			// Calculate the vectors of the square around the house based on the house its position and the range
			Vector2 minPosition = housePosition - new Vector2(range, range);
			Vector2 maxPosition = housePosition + new Vector2(range, range);

			List<Vector2> specialBuildingsInSquare = root.createPointsInSquareList(minPosition, maxPosition, root);

			return pointsInCircel(specialBuildingsInSquare, housePosition, range);
		}

		// Filter out the specialBuildings that are in the square but not in the circel around a house
		public IEnumerable<Vector2> pointsInCircel(IEnumerable<Vector2> specialBuildingsList, Vector2 housePosition, float range)
		{
			List<Vector2> specialBuildingsInCircelList = new List<Vector2>();
			foreach (Vector2 specialBuilding in specialBuildingsList)
			{
				if (Math.Sqrt(Math.Pow((housePosition.X - specialBuilding.X), 2) + Math.Pow((housePosition.Y - specialBuilding.Y), 2)) < range)
				{
					specialBuildingsInCircelList.Add(specialBuilding);
				}
			}
			return specialBuildingsInCircelList;
		}
	}
}
