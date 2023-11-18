using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class UI_TileAction : MonoBehaviour
    {
        [SerializeField] private Button _createWallButton;
        [SerializeField] private Text _createWallButtonText;
        [SerializeField] private Button _destroyWallButton;
        [SerializeField] private Text _destroyWallButtonText;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private Text _createTowerButtonText;
        [SerializeField] private Button _destroyTowerButton;
        [SerializeField] private Text _destroyTowerButtonText;
        [SerializeField] private Button _mergeTowerButton;
        [SerializeField] private Text _mergeTowerButtonText;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Text _cancelButtonText;

        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();

            _createWallButton.onClick.AddListener(() =>
            {
                SpawnManager.instance.SpawnWall(MapManager.instance.selectedTile);
            });

            _destroyWallButton.onClick.AddListener(() =>
                SpawnManager.instance.DestroyObject(MapManager.instance.selectedTile));

            _createTowerButton.onClick.AddListener(() =>
                SpawnManager.instance.SpawnDefaultTower(MapManager.instance.selectedTile));

            _destroyTowerButton.onClick.AddListener(() =>
                SpawnManager.instance.DestroyObject(MapManager.instance.selectedTile));

            _mergeTowerButton.onClick.AddListener(() =>
                SpawnManager.instance.MergeTower(MapManager.instance.selectedTile.tower));

            _cancelButton.onClick.AddListener(() =>
            {
                MapManager.instance.selectedTile = null;
                gameObject.SetActive(false);
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.instance.onGoldChanged += () =>
            {
                if (GameManager.instance.gold < 5)
                {
                    _createWallButtonText.color = Color.red;
                    _destroyWallButtonText.color = Color.red;
                    _createTowerButtonText.color = Color.red;
                    _mergeTowerButtonText.color = Color.red;
                    _destroyTowerButtonText.color = Color.red;
                }

                else
                {
                    _createWallButtonText.color = Color.black;
                    _destroyWallButtonText.color = Color.black;
                    _createTowerButtonText.color = Color.black;
                    _mergeTowerButtonText.color = Color.black;
                    _destroyTowerButtonText.color = Color.black;
                }
            };

            MapManager.instance.onSelectedTileChanged += () =>
            {
                if (MapManager.instance.selectedTile is null)
                {
                    gameObject.SetActive(false);
                }

                else
                {
                    switch (MapManager.instance.selectedTile.tileState)
                    {
                        case TileState.None:
                            {
                                _createWallButton.gameObject.SetActive(true);
                                //if (GameManager.instance.gold < 5)
                                //{
                                //    _createWallButtonText.color = Color.red;
                                //}

                                //else
                                //{
                                //    _createWallButtonText.color = Color.black;
                                //}

                                _destroyWallButton.gameObject.SetActive(false);
                                _createTowerButton.gameObject.SetActive(false);
                                _mergeTowerButton.gameObject.SetActive(false);
                                _destroyTowerButton.gameObject.SetActive(false);
                            }
                            break;
                        case TileState.Wall:
                            {
                                _createWallButton.gameObject.SetActive(false);
                                _destroyWallButton.gameObject.SetActive(true);
                                //if (GameManager.instance.gold < 5)
                                //{
                                //    _destroyWallButtonText.color = Color.red;
                                //}

                                //else
                                //{
                                //    _destroyWallButtonText.color = Color.black;
                                //}

                                _createTowerButton.gameObject.SetActive(true);
                                //if (GameManager.instance.gold < 5)
                                //{
                                //    _createTowerButtonText.color = Color.red;
                                //}

                                //else
                                //{
                                //    _createTowerButtonText.color = Color.black;
                                //}

                                _mergeTowerButton.gameObject.SetActive(false);
                                _destroyTowerButton.gameObject.SetActive(false);
                            }
                            break;
                        case TileState.Tower:
                            {
                                _createWallButton.gameObject.SetActive(false);
                                _destroyWallButton.gameObject.SetActive(false);
                                _createTowerButton.gameObject.SetActive(false);
                                _mergeTowerButton.gameObject.SetActive(true);
                                //if (GameManager.instance.gold < 5)
                                //{
                                //    _mergeTowerButtonText.color = Color.red;
                                //}

                                //else
                                //{
                                //    _mergeTowerButtonText.color = Color.black;
                                //}

                                _destroyTowerButton.gameObject.SetActive(true);
                                //if (GameManager.instance.gold < 5)
                                //{
                                //    _destroyTowerButtonText.color = Color.red;
                                //}

                                //else
                                //{
                                //    _destroyTowerButtonText.color = Color.black;
                                //}
                            }
                            break;
                    }

                    gameObject.SetActive(true);

                    float leftBound = MapManager.instance.tilemap.origin.x;
                    float rightBound = MapManager.instance.tilemap.origin.x + MapManager.instance.tilemap.size.x;
                    float downBound = MapManager.instance.tilemap.origin.y;
                    float upBound = MapManager.instance.tilemap.origin.y + MapManager.instance.tilemap.size.y;

                    float sizeX = _rect.sizeDelta.x * _rect.localScale.x;
                    float sizeY = _rect.sizeDelta.y * _rect.localScale.y;

                    Vector3 pos = MapManager.instance.selectedTile.tilePos + Vector3.up * (sizeY / 2.0f + MapManager.instance.tilemap.cellSize.y / 2.0f);

                    if (pos.x - sizeX / 2.0f <= leftBound)
                    {
                        pos = pos + Vector3.right * (sizeX / 2.0f - MapManager.instance.tilemap.cellSize.x / 2.0f);
                    }

                    else if (pos.x + sizeX / 2.0f >= rightBound)
                    {
                        pos = pos + Vector3.left * (sizeX / 2.0f - MapManager.instance.tilemap.cellSize.x / 2.0f);
                    }

                    if (pos.y + sizeY / 2.0f >= upBound)
                    {
                        pos = pos + Vector3.down * (sizeY + MapManager.instance.tilemap.cellSize.y);
                    }

                    //else
                    //{
                    //    pos = pos + Vector3.up * (sizeY / 2.0f + MapManager.instance.tilemap.cellSize.y / 2.0f);
                    //}

                    transform.position = pos;
                }
            };

            gameObject.SetActive(false);
        }
    }
}

