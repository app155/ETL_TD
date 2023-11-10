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
                Vector3Int pos = new Vector3Int(j - tilemap.size.x / 2, tilemap.size.y / 2 - i, 0);

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
        // !Move to other Script!

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //    Debug.DrawRay(mousePos, Vector2.zero, Color.red, 1.0f);

        //    if (hit.collider != null)
        //    {
        //        Vector3Int tpos = _tilemap.WorldToCell(hit.point);
        //        Debug.Log(tpos);
        //        Debug.Log($"12123123,{_tilemap.size.y / 2 - tpos.y - 1}, {_tilemap.size.x / 2 + tpos.x}");
        //        Debug.Log(_map[_tilemap.size.y / 2 - tpos.y - 1, _tilemap.size.x / 2 + tpos.x].tilePos);

        //        selectedTile = _map[_tilemap.size.y / 2 - tpos.y - 1, _tilemap.size.x / 2 + tpos.x];

        //        Debug.Log($"TileState : {selectedTile.tileState}");
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Space) && Comparer.Default.Compare(selectedTile, default) != 0 && selectedTile.tileState == 0)
        //{
        //    if (PathFindbyBFS(selectedTile, _map))
        //    {
        //        SpawnManager.instance.SpawnWall(selectedTile.tileIndex[0], selectedTile.tileIndex[1]);

        //        Debug.Log($"Wall Created in {selectedTile.tileIndex[0]}, {selectedTile.tileIndex[1]}");
        //    }

        //    else
        //    {
        //        Debug.Log("±æ¸·ÀÓ");
        //    }
        //}
    }

    public bool PathFindbyBFS(TileInfo tileToTry, TileInfo[,] map, bool isInit = false)
    {
        //if (tileToTry.tileIndex[0] == 0 && tileToTry.tileIndex[1] == 0 && isInit == false)
        //{
        //    return false;
        //}

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

        MakePath(checker);

        if (checker[mapHeight - 1, mapWidth - 1].visited == false)
        {
            map[tileToTry.tileIndex[0], tileToTry.tileIndex[1]].tileState = TileState.None;
            return false;
        }

        return true;
    }

    void MakePath(PathChecker[,] checker)
    {
        path.Clear();

        int nowY = checker.GetLength(0) - 1;
        int nowX = checker.GetLength(1) - 1;

        path.Add(new int[] { nowY, nowX });

        while (!(nowY == 0 && nowX == 0))
        {
            PathChecker now = checker[nowY, nowX];
            path.Add(new int[] { checker[nowY, nowX].prevY, checker[nowY, nowX].prevX });

            int tmpY = nowY;
            int tmpX = nowX;

            nowY = checker[tmpY, tmpX].prevY;
            nowX = checker[tmpY, tmpX].prevX;
        }

        path.Reverse();
    }
}
