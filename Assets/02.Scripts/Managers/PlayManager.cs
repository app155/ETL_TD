using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PlayManager").AddComponent<PlayManager>();
            }

            return _instance;
        }
    }

    private static PlayManager _instance;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Debug.DrawRay(mousePos, Vector2.zero, Color.red, 1.0f);

            if (hit.collider != null)
            {
                Vector3Int tpos = MapManager.instance.tilemap.WorldToCell(hit.point);
                Debug.Log(tpos);
                Debug.Log($"12123123,{MapManager.instance.tilemap.size.y / 2 - tpos.y - 1}, {MapManager.instance.tilemap.size.x / 2 + tpos.x}");
                Debug.Log(MapManager.instance.map[MapManager.instance.tilemap.size.y / 2 - tpos.y - 1, MapManager.instance.tilemap.size.x / 2 + tpos.x].tilePos);

                MapManager.instance.selectedTile = MapManager.instance.map[MapManager.instance.tilemap.size.y / 2 - tpos.y - 1, MapManager.instance.tilemap.size.x / 2 + tpos.x];

                Debug.Log($"TileState : {MapManager.instance.selectedTile.tileState}");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0 && MapManager.instance.selectedTile.tileState == MapManager.TileState.None)
        {
            if (MapManager.instance.PathFindbyBFS(MapManager.instance.selectedTile, MapManager.instance.map))
            {
                SpawnManager.instance.SpawnWall(MapManager.instance.selectedTile);

                Debug.Log($"Wall Created in {MapManager.instance.selectedTile.tileIndex[0]}, {MapManager.instance.selectedTile.tileIndex[1]}");
            }

            else
            {
                Debug.Log("±Ê∏∑¿”");
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0 && MapManager.instance.selectedTile.tileState == MapManager.TileState.Wall)
        {
            SpawnManager.instance.SpawnDefaultTower(MapManager.instance.selectedTile);
        }

        if (Input.GetKeyDown(KeyCode.X) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0 && MapManager.instance.selectedTile.tileState == MapManager.TileState.Tower)
        {
            SpawnManager.instance.MergeTower(MapManager.instance.selectedTile.tower);
        }

        if (Input.GetKeyDown(KeyCode.Tab) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0)
        {
            SpawnManager.instance.DestroyObject(MapManager.instance.selectedTile);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(SpawnManager.instance.SpawnRoundEnemy());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(SpawnManager.instance.SpawnPathNotificator());
        }
    }
}
