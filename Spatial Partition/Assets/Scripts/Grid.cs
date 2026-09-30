using UnityEngine;
using System.Collections;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        // Need this to convert from world coordinate position to cell position
        int cellSize;
        
        // This is the actual grid, where a soldier is in each cell
        // Each individual soldier links to other soldiers in the same cell
        Soldier[,] cells;

        // Init the grid
        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;
            int numberOfCells = mapWidth / cellSize;
            cells = new Soldier[numberOfCells, numberOfCells];
        }

        // Add a unity to the grid
        public void Add(Soldier soldier)
        {
            // Determine which grid cell the soldier is in
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            // Add the soldier to the front of the list for the cell it's in
            soldier.previousSoldier = null;
            soldier.nextSoldier = cells[cellX, cellZ];

            // Associate this cell with this soldier
            cells[cellX, cellZ] = soldier;

            if(soldier.nextSoldier != null)
            {
                // Set this soldier to be the previous soldier of the next soldier of this soldier (linked lists ftw)
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        // Get the closest enemy from the grid
        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            // Determine which grid cell the friendly soldier is in
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            // Get the first enemy in grid
            Soldier enemy = cells[cellX, cellZ];

            // Find the closest soldier of all in the linked list
            Soldier closestSoldier = null;
            float bestDistSqr = Mathf.Infinity;

            // Loop through the linked list
            while(enemy != null)
            {
                // The distance sqr between the soldier and this enemy
                float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.positon).sqrMagnitude;

                // If this distance is better than the previous est distance, then we have found an enemy that's closer
                if(distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestSoldier = enemy;
                }
                // Get the next enemy in the list
                enemy = enemy.nextSoldier;
            }
        }
    }
}