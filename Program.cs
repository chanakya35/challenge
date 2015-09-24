namespace Islands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /* Given a grid (like Battleship), write a function which counts the number of “islands,” where an island could consist of 
     * adjacent squares either vertically, horizontally, or diagonally, e.g.:  

           0  1  2  3  4  5  6  7  8  9
        0  X  X           X
        1  X
        2                X  X  X 
        3     X              X 
        4     X  X  X           X
        5        X  
        6
        7  X  X           X  X     X
        8        X                    X
        9                          X

     * (this would be seven islands)
     */
    
    static class Program
    {
        static void Main(string[] args)
        {
            var testGrid = new bool[10, 10];
            // initialize values, args by row
            SetValues(testGrid, 
                        0, 0, 0, 1, 0, 5,
                        1, 0,
                        2, 5, 2, 6, 2, 7,
                        3, 1, 3, 6,
                        4, 1, 4, 2, 4, 3, 4, 7,
                        5, 2,
                        7, 0, 7, 1, 7, 5, 7, 6, 7, 8,
                        8, 2, 8, 9,
                        9, 8);
            PrintArray(testGrid);
            int islandCount = CountIslands(testGrid);
            Console.WriteLine();
            Console.WriteLine("The grid contains {0} islands. {1}", islandCount, Environment.NewLine); 
            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        private static int CountIslands(bool[,] testGrid)
        {
            var islands = new List<List<Tuple<int,int>>>();
            var corner = Tuple.Create(0, 0);
            for (int i = 0; i < testGrid.GetLength(0); i++)
            {
                for (int j = 0; j < testGrid.GetLength(1); j++)
                {
                    if (testGrid[i, j] == true && islands.SelectMany(isle => isle).Contains(Tuple.Create(i,j)) == false)
                    {
                        islands.Add(GetNeighbors(Tuple.Create(i, j), testGrid, new List<Tuple<int,int>>()));
                    }
                }
            }
            return islands.Count();
        }

        private static List<Tuple<int, int>> GetNeighbors(Tuple<int, int> currPoint, bool[,] grid, List<Tuple<int, int>> foundNeighbors)
        {
            var surroundingPoints = grid.SurroundingPoints(currPoint);
            if (NoNewNeighbors(surroundingPoints, foundNeighbors))
            {
                return new List<Tuple<int,int>>() { currPoint }; 
            }
            else
            {
                foundNeighbors.AddRange(surroundingPoints);
                foreach(Tuple<int,int> point in surroundingPoints) 
                {
                    foundNeighbors.AddRange(GetNeighbors(point, grid, foundNeighbors));
                }
                return foundNeighbors;
            }
        }

        private static bool NoNewNeighbors(List<Tuple<int,int>> surroundingPoints, List<Tuple<int,int>> foundNeighbors) 
        {
            return surroundingPoints.Except(foundNeighbors).Any() == false;
        }

        private static List<Tuple<int, int>> SurroundingPoints(this Array grid, Tuple<int, int> point)
        {
            int xMin = point.Item1 == 0 ? 0 : point.Item1 - 1;
            int xMax = point.Item1 == (grid.GetLength(0) - 1) ? grid.GetLength(0) - 1 : point.Item1 + 1;
            int yMin = point.Item2 == 0 ? 0 : point.Item2 - 1;
            int yMax = point.Item2 == (grid.GetLength(1) - 1) ? grid.GetLength(1) - 1 : point.Item2 + 1;

            var points = new List<Tuple<int, int>>();
            for(int i = xMin; i <= xMax; i++) 
            {
                for(int j = yMin; j <= yMax; j++) 
                {
                    if ((i == point.Item1 && j == point.Item2) == false && (grid as bool[,])[i,j] == true)
                    {
                        points.Add(Tuple.Create(i, j));
                    }
                }
            }
            return points;
        }

        private static void PrintArray(bool[,] testGrid)
        {
            PrintXAxis(testGrid);
            Console.WriteLine();
            for (int i = 0; i < testGrid.GetLength(0); i++)
            {
                PrintYAxis(i);
                for(int j = 0; j < testGrid.GetLength(1); j++)
                {
                    Console.Write("  " + (testGrid[i,j] ? "X" : " "));
                }
                Console.WriteLine();
            }
        }

        private static void PrintYAxis(int i)
        {
            Console.Write(i);
        }

        private static void PrintXAxis(bool[,] testGrid)
        {
            Console.Write(" ");
            for (int j = 0; j < testGrid.GetLength(1); j++)
            {
                Console.Write("  " + j);
            }
        }

        private static void SetValues(bool[,] testGrid, params int[] coords)
        {
            if (coords.Length == 0 || coords.Length % 2 == 1)
            {
                throw new Exception("Invalid coordinate set!");
            }
            if (testGrid == null || testGrid.Length == 0)
            {
                throw new Exception("Array is null or of size zero!");
            }

            for (int i = 0; i < coords.Length; i+=2)
            {
                testGrid[coords[i], coords[i + 1]] = true;
            }            
        } 
    }
}