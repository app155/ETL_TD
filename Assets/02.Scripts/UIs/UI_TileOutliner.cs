using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TileOutliner : MonoBehaviour
{
    void Start()
    {
        MapManager.instance.onSelectedTileChanged += () =>
        {
            if (Comparer.Default.Compare(MapManager.instance.selectedTile, default) == 0)
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
