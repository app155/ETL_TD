using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTest : MonoBehaviour
{
    enum TileState
    {
        None,
        Wall,
        Tower,
    }

    struct TileInfo
    {
        public int[] tileIndex;
        public Vector3Int tilePos;
        public TileState tileState;
    }

    private Tilemap _tilemap;
    private Vector3 _cellSize;
    [SerializeField] private TileInfo[,] _map;
    [SerializeField] private TileInfo selectedTile;
    [SerializeField] private GameObject _testWall;

    private Queue<TileInfo> path;
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _cellSize = _tilemap.cellSize;

        _map = new TileInfo[_tilemap.size.y, _tilemap.size.x];

        for (int i = 0; i < _tilemap.size.y; i++)
        {
            for (int j = 0; j < _tilemap.size.x; j++)
            {
                Vector3Int pos = new Vector3Int(j - _tilemap.size.x / 2, _tilemap.size.y / 2 - i, 0);

                _map[i, j] = new TileInfo()
                {
                    tileIndex = new int[] { i, j },
                    tilePos = pos,
                    tileState = 0
                };
            }
        }

        Debug.Log(_tilemap.origin);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            Debug.DrawRay(mousePos, Vector2.zero, Color.red, 1.0f);

            if (hit.collider != null)
            {
                Vector3Int tpos = _tilemap.WorldToCell(hit.point);
                Debug.Log(tpos);
                Debug.Log($"12123123,{_tilemap.size.y / 2 - tpos.y - 1}, {_tilemap.size.x / 2 + tpos.x}");
                Debug.Log(_map[_tilemap.size.y / 2 - tpos.y - 1, _tilemap.size.x / 2 + tpos.x].tilePos);

                selectedTile = _map[_tilemap.size.y / 2 - tpos.y - 1, _tilemap.size.x / 2 + tpos.x];

                Debug.Log(selectedTile.tileState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Comparer.Default.Compare(selectedTile, default) != 0 && selectedTile.tileState == 0)
        {
            if (PathFindbyBFS(selectedTile, _map))
            {
                //Instantiate(_testWall, selectedTile.tilePos + new Vector3(0.5f, -0.5f, 0.0f), Quaternion.identity);
                PoolManager.instance.Get(0);
                selectedTile.tileState = TileState.Wall;
                Debug.Log($"Wall Created in {selectedTile.tileIndex[0]}, {selectedTile.tileIndex[1]}");
            }

            else
            {
                Debug.Log("±æ¸·ÀÓ");
            }
        }
    }

    bool PathFindbyBFS(TileInfo tileToTry, TileInfo[,] map)
    {
        if (tileToTry.tileIndex[0] == 0 && tileToTry.tileIndex[1] == 0)
            return false;

        TileInfo[,] expectedMap = (TileInfo[,])map.Clone();
        expectedMap[tileToTry.tileIndex[0], tileToTry.tileIndex[1]].tileState = TileState.Wall;

        int[] dirY = { -1, 0, 1, 0 };
        int[] dirX = { 0, -1, 0, 1 };

        int mapHeight = expectedMap.GetLength(0);
        int mapWidth = expectedMap.GetLength(1);

        bool[,] visited = new bool[mapHeight, mapWidth];
        visited[0, 0] = true;

        Queue<int[]> q = new Queue<int[]>();
        q.Enqueue(new int[] { 0, 0 });

        while (q.Count > 0 && visited[mapHeight - 1, mapWidth - 1] == false)
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

                if (visited[nextY, nextX])
                {
                    continue;
                }

                if (expectedMap[nextY, nextX].tileState > 0)
                {
                    continue;
                }

                q.Enqueue(new int[] { nextY, nextX });
                visited[nextY, nextX] = true;
            }
        }

        if (visited[mapHeight - 1, mapWidth - 1] == false)
        {
            return false;
        }

        _map = expectedMap;
        return true;
    }

}
