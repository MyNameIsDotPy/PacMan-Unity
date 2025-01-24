using System;
using System.Collections.Generic;
using UnityEngine;
public class ScrDijkstra
{
    public ScrDijkstra(int gridWidth, int gridHeight)
    {
        _gridWidth = gridWidth;
        _gridHeight = gridHeight;
    }

    private int _gridWidth;
    private int _gridHeight;
    
    private int[,] _grid;
    private bool[,] _visited;
    private double[,] _distance;
    private Queue<int> _queue = new Queue<int>();
    private List<int[]> _path = new List<int[]>();
    private int[,][] _previous;
    
    private void CreateGrid()
    {
        _grid = new int[_gridWidth, _gridHeight];
        _visited = new bool[_gridWidth, _gridHeight];
        _distance = new double[_gridWidth, _gridHeight];
        _previous = new int[_gridWidth, _gridHeight][];
        _queue = new Queue<int>();
        _path = new List<int[]>();
        
        for (int y = 0; y < _gridHeight; y++)
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                _visited[x, y] = false;
                _distance[x, y] = Mathf.Infinity;
                _previous[x, y] = null;

                Vector3 pos = new Vector3(x, -1, y);

                if (Physics.Raycast(pos, Vector3.up, out RaycastHit hit, 2))
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        _grid[x, y] = 1;
                    }
                }
            }
        }
    }
    public List<int[]> CalculatePath(int[] start, int[] goal)
    {
        // Debug.Log("From: " + start[0] + ", " + start[1]);
        // Debug.Log("To: " + goal[0] + ", " + goal[1]);
        
        CreateGrid();
        _distance[start[0], start[1]] = 0;
        _queue.Enqueue(start[0] + start[1] * _gridWidth);

        while (_queue.Count > 0)
        {
            int current = _queue.Dequeue();
            int[] pos = { current % _gridWidth, current / _gridWidth };

            if (_visited[pos[0], pos[1]])
            {
                continue;
            }
            _visited[pos[0], pos[1]] = true;

            if (pos[0] == goal[0] && pos[1] == goal[1])
                break;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) != 1)
                    {
                        continue;
                    }

                    int nx = pos[0] + dx;
                    int ny = pos[1] + dy;

                    if (nx >= 0 && nx < _gridWidth && ny >= 0 && ny < _gridHeight)
                    {
                        if (!_visited[nx, ny] && _grid[nx, ny] != 1)
                        {
                            double cost = _distance[pos[0], pos[1]] + 1;
                            if (cost < _distance[nx, ny])
                            {
                                _distance[nx, ny] = cost;
                                _previous[nx, ny] = pos;
                                _queue.Enqueue(nx + ny * _gridWidth);
                            }
                        }
                    }
                }
            }
        }

        return ReconstructPath(goal);
    }
    private List<int[]> ReconstructPath(int[] goal)
    {
        _path.Clear();
        int[] currentPos = goal;
        while (currentPos != null)
        {
            _path.Add(currentPos);
            currentPos = _previous[currentPos[0], currentPos[1]];
        }
        
        _path.Reverse();
        return _path;
    }
}