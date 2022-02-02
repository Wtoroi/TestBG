using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaster : MonoBehaviour
{
    [SerializeField] private GameObject Tile;
    [SerializeField] private GameObject Exit;
    [SerializeField] private GameObject PathDot;
    [SerializeField] private GameObject Trap;
    private byte[,] maze = new byte[15, 9];
    private int BestPathLenght = -1;
    public List<Dot> BestPath = new List<Dot>();

    public delegate void End();
    public static event End PlayerStart;

    void Start()
    {
        CreateMaze();

        for (int i = 0; i < BestPath.Count; i++)
            Instantiate(PathDot, new Vector3(BestPath[i].x, BestPath[i].y, 0), Quaternion.identity);

        for (int x = 0; x < maze.GetLength(0); x++)
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (maze[x, y] == 0)
                    Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
                if (maze[x, y] == 2)
                    Instantiate(Exit, new Vector3(x, y, 0), Quaternion.identity);
                if (maze[x, y] == 3)
                    Instantiate(Trap, new Vector3(x, y, 0), Quaternion.identity);
            }
    }

    private int SirchPath(int x, int y, int sum, List<Dot> Path)
    {
        if (maze[x, y] == 2)
        {
            if (BestPathLenght == -1 || BestPathLenght > sum)
            {
                BestPathLenght = sum;
                BestPath = Path;
            }
            return 0;
        }
        else
        {
            Dot dot = new Dot();

            if (x + 1 < maze.GetLength(0) - 1 && maze[x + 1, y] != 0)
            {
                dot.x = x + 1;
                dot.y = y;
                if (!Path.Contains(dot))
                {
                    Path.Add(dot);
                    return SirchPath(x + 1, y, sum + 1, Path);
                }
            }
            if (y + 1 < maze.GetLength(1) - 1 && maze[x, y + 1] != 0)
            {
                dot.x = x;
                dot.y = y + 1;
                if (!Path.Contains(dot))
                {
                    Path.Add(dot);
                    return SirchPath(x, y + 1, sum + 1, Path);
                }
            }
            if (y - 1 > 0 && maze[x, y + 1] != 0)
            {
                dot.x = x;
                dot.y = y - 1;
                if (!Path.Contains(dot))
                {
                    Path.Add(dot);
                    return SirchPath(x, y - 1, sum + 1, Path);
                }
            }

            return 0;
        }
    }

    private void CreateMaze()
    {
        while (BestPathLenght == -1)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
                for (int y = 0; y < maze.GetLength(1); y++)
                    maze[x, y] = 0;

            for (int x = 0; x < maze.GetLength(0); x++)
                for (int y = 0; y < maze.GetLength(1); y++)
                    maze[x, y] = 0;

            for (int x = 1; x < maze.GetLength(0); x += 2)
                for (int y = 1; y < maze.GetLength(1); y += 2)
                    maze[x, y] = 1;

            for (int x = 2; x < maze.GetLength(0) - 1; x += 2)
                for (int y = 2; y < maze.GetLength(1) - 1; y += 2)
                {
                    Path(x, y);
                }

            maze[maze.GetLength(0) - 2, maze.GetLength(1) - 2] = 2;

            SirchPath(1, 1, 0, new List<Dot>());

            for (int x = 1; x < maze.GetLength(0) - 1; x += 2)
                for (int y = 1; y < maze.GetLength(1) - 1; y += 2)
                {
                    if (maze[x, y] == 1)
                    {
                        SpawnTrap(x, y);
                    }
                }
        }
        PlayerStart();
    }

    private void SpawnTrap(int x, int y)
    {
        int chanse = Random.Range(0, 10);
        if (chanse < 3 && (x != 1 && y != 1))
            maze[x, y] = 3;
    }

    private void Path(int x, int y)
    {
        int max = Random.Range(1, 3);
        for (int i = 0; i <= max; i++)
            switch (Random.Range(0, 4))
            {
                case 0:
                    maze[x - 1, y] = 1;
                    break;
                case 1:
                    maze[x, y - 1] = 1;
                    break;
                case 2:
                    maze[x + 1, y] = 1;
                    break;
                case 3:
                    maze[x, y + 1] = 1;
                    break;
            }
    }

    void Update()
    {
        
    }

    public struct Dot
    {
        public int x;
        public int y;
    }
}
