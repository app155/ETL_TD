using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TileOutliner : MonoBehaviour
{
    void Start()
    {
        MapManager.instance.onSelectedTileChanged += () =>
        {
            if (MapManager.instance.selectedTile is null)
            {
                gameObject.SetActive(false);
            }

            else
            {
                gameObject.SetActive(true);
                transform.position = MapManager.instance.selectedTile.tilePos;
            }
        };

        gameObject.SetActive(false);
    }
}
