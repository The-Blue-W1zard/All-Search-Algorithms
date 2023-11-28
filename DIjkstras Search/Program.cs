namespace DIjkstras_Search
{
    using System.Drawing;
    using System.Runtime.Versioning;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        static List<Point> DijkstrasAlgorithm(int[,] maze, Point start, Point end)
        {
            PriorityQueue<Point,int> Q = new PriorityQueue<Point,int>();
            Dictionary<Point, int> dist = new Dictionary<Point, int>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

            for (int row = 0; row < maze.GetLength(0); row++) 
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    dist.Add(new Point(row, col), Int32.MaxValue);
                    //cameFrom.Add(new Point(row, col), null);
                }
            }

            dist[start] = 0;
            Q.Enqueue(start, 0);
            Point currentCell = new Point(-1,-1);

            do
            {
                currentCell = Q.Dequeue();

                if(currentCell == end)
                {
                    break;
                }

                foreach(Point nextCell in Neighbours(maze, currentCell))
                {
                    int newDistance = dist[currentCell] + 1;
                    if (newDistance < dist[nextCell])
                    {
                        dist[nextCell] = newDistance;
                        cameFrom[nextCell] = currentCell;
                        Q.Enqueue(nextCell, newDistance);
                    }
                }



            } while (Q.Count > 0);    
        }

        
        static List<Point> Neighbours(int[,] maze, Point current)
        {
            int[] checksRows = { 0, 0, 1, -1 };
            int[] checksCols = { 1, -1, 0, 0 };
            int row = current.Y;
            int col = current.X;
            List<Point> neighbours = new List<Point>();

            for (int t = 0; t < 4; t++)
            {
                try
                {
                    if (maze[row + checksRows[t], col + checksCols[t]] != 1)
                    {
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch { }
            }
            return neighbours;
        }
    }
}
