using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    private bool _canInputAction => GameManager.instance.gamePhase == GamePhase.BuildPhase ||
                                    GameManager.instance.gamePhase == GamePhase.DefensePhase;

    void Update()
    {
        InputAction();
    }

    void InputAction()
    {
        if (!_canInputAction)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Vector3Int tpos = MapManager.instance.tilemap.WorldToCell(hit.point);
                Debug.Log($"tpos : {tpos}");
                Debug.Log($"origin : {MapManager.instance.tilemap.origin}");
                Debug.Log($"12123123,{MapManager.instance.tilemap.size.y - (tpos.y - MapManager.instance.tilemap.origin.y + 1)}, {tpos.x - MapManager.instance.tilemap.origin.x}");

                MapManager.instance.selectedTile = MapManager.instance.map[MapManager.instance.tilemap.size.y - (tpos.y - MapManager.instance.tilemap.origin.y + 1), tpos.x - MapManager.instance.tilemap.origin.x];

                Debug.Log($"{MapManager.instance.selectedTile.tileIndex[0]} {MapManager.instance.selectedTile.tileIndex[1]}");
                Debug.Log(MapManager.instance.selectedTile.tilePos);

                Debug.Log($"TileState : {MapManager.instance.selectedTile.tileState}");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MapManager.instance.selectedTile = null;
        }

        if (Input.GetKeyDown(KeyCode.Q) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0)
        {
            if (MapManager.instance.selectedTile.tileState == TileState.None)
            {
                if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
                {
                    SpawnManager.instance.SpawnUITextNotification("���� �غ� �ܰ迡���� �Ǽ� �����մϴ�.");
                    return;
                }

                if (GameManager.instance.gold < 5)
                {
                    SpawnManager.instance.SpawnUITextNotification("�ڿ��� �����մϴ�.");
                    return;
                }

                if (MapManager.instance.PathFindbyBFS(MapManager.instance.selectedTile, MapManager.instance.map))
                {
                    SpawnManager.instance.SpawnWall(MapManager.instance.selectedTile);
                }

                else
                {
                    SpawnManager.instance.SpawnUITextNotification("���� ���� ��ġ�� ���� �Ǽ��� �� �����ϴ�.");
                }
            }

            else
            {
                SpawnManager.instance.SpawnUITextNotification("���� �� Ÿ�Ͽ��� �Ǽ� �����մϴ�.");
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0)
        {
            SpawnManager.instance.SpawnDefaultTower(MapManager.instance.selectedTile);
        }

        if (Input.GetKeyDown(KeyCode.E) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0 && MapManager.instance.selectedTile.tileState == TileState.Tower)
        {
            SpawnManager.instance.MergeTower(MapManager.instance.selectedTile.tower);
        }

        if (Input.GetKeyDown(KeyCode.R) && Comparer.Default.Compare(MapManager.instance.selectedTile, default) != 0)
        {
            SpawnManager.instance.DestroyObject(MapManager.instance.selectedTile);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(SpawnManager.instance.SpawnPathNotificatorRoutine());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpgradeManager.instance.UpgradeDiamondTower();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpgradeManager.instance.UpgradeHexagonTower();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpgradeManager.instance.UpgradeTriangleTower();
        }
    }
}
