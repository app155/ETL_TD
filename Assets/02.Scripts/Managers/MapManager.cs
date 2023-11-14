using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public enum TileState
    {
        None,
        Wall,
        Tower,
    }

    public class TileInfo
    {
        public int[] tileIndex;
        public Vector3 tilePos;
        public TileState tileState;
        public Wall wall;
        public TowerController tower;
    }

    struct PathChecker
    {
        public bool visited;
        public int prevY;
        public int prevX;
    }

    public static MapManager instance
    {
        get
        {
            return _instance;
        }
    }

    private static MapManager _instance;

    public Tilemap tilemap;
    private Vector3 _cellSize;
    public TileInfo[,] map;
    public TileInfo selectedTile
    {
        get { return _selectedTile; }
        set
        {
            if (value == _selectedTile)
                return;

            _selectedTile = value;
            onSelectedTileChanged?.Invoke();
        }
    }

    private TileInfo _selectedTile;
    public List<int[]> path;
    public Action onSelectedTileChanged;

    private void Awake()
    {
        _instance = this;

        tilemap = GetComponent<Tilemap>();
        _cellSize = tilemap.cellSize;

        map = new TileInfo[tilemap.size.y, tilemap.size.x];
        path = new List<int[]>();

        for (int i = 0; i < tilemap.size.y; i += (int)_cellSize.y)
        {
            for (int j = 0; j < tilemap.size.x; j += (int)_cellSize.x)
            {
                Vector3Int pos = new Vector3Int(tilemap.origin.x + j, tilemap.origin.y - i + tilemap.size.y, 0);

                map[i, j] = new TileInfo()
                {
                    tileIndex = new int[] { i, j },
                    tilePos = pos + new Vector3(0.5f, -0.5f, 0.0f),
                    tileState = 0
                };
            }
        }

        Debug.Log(tilemap.origin);

        PathFindbyBFS(map[0, 0], map, true);
    }

    private void Update()
    {

    }

    public bool PathFindbyBFS(TileInfo tileToTry, TileInfo[,] map, bool isInit = false)
    {
        List<int[]> originPath = path;

        if (isInit == false &&
            ((tileToTry.tileIndex[0] == 0 && tileToTry.tileIndex[1] == 0) ||
            (tileToTry.tileIndex[0] == map.GetLength(0) - 1 && tileToTry.tileIndex[1] == map.GetLength(1) - 1)))
        {
            return false;
        }

        if (isInit == false)
        {
            map[tileToTry.tileIndex[0], tileToTry.tileIndex[1]].tileState = TileState.Wall;
        }

        int[] dirY = { -1, 0, 1, 0 };
        int[] dirX = { 0, -1, 0, 1 };

        int mapHeight = map.GetLength(0);
        int mapWidth = map.GetLength(1);

        PathChecker[,] checker = new PathChecker[mapHeight, mapWidth];
        checker[0, 0].visited = true;
        checker[0, 0].prevY = 0;
        checker[0, 0].prevX = 0;

        Queue<int[]> q = new Queue<int[]>();
        q.Enqueue(new int[] { 0, 0 });

        while (q.Count > 0 && checker[mapHeight - 1, mapWidth - 1].visited == false)
        {
            int[] now = q.Dequeue();
            int nowY = now[0];
            int nowX = now[1];

            for (int i = 0; i < 4; i++)
            {
                int nextY = nowY + dirY[i];
                int nextX = nowX + dirX[i];

                if (nextY < 0 || nextY >= mapHeight || nextX < 0 || nextX >= mapWidth)
                {
                    continue;
                }

                if (checker[nextY, nextX].visited)
                {
                    continue;
                }

                if (map[nextY, nextX].tileState > 0)
                {
                    continue;
                }

                q.Enqueue(new int[] { nextY, nextX });
                checker[nextY, nextX].visited = true;
                checker[nextY, nextX].prevY = nowY;
                checker[nextY, nextX].prevX = nowX;
            }
        }

        path = MakePath(checker);

        if (checker[mapHeight - 1, mapWidth - 1].visited == false)
        {
            map[tileToTry.tileIndex[0], tileToTry.tileIndex[1]].tileState = TileState.None;
            path = originPath;
            return false;
        }

        return true;
    }

    List<int[]> MakePath(PathChecker[,] checker)
    {
        List<int[]> newPath = new List<int[]>();

        int nowY = checker.GetLength(0) - 1;
        int nowX = checker.GetLength(1) - 1;

        newPath.Add(new int[] { nowY, nowX });

        while (!(nowY == 0 && nowX == 0))
        {
            PathChecker now = checker[nowY, nowX];
            newPath.Add(new int[] { now.prevY, now.prevX });

            int tmpY = nowY;
            int tmpX = nowX;

            nowY = checker[tmpY, tmpX].prevY;
            nowX = checker[tmpY, tmpX].prevX;
        }

        newPath.Reverse();

        return newPath;
    }
}
