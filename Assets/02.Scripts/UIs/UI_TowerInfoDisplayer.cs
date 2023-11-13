using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TowerInfoDisplayer : MonoBehaviour
{
    [SerializeField] private TowerController _selectedtower;
    [SerializeField] private Image _towerImage;
    [SerializeField] private Text _towerNameText;
    [SerializeField] private Text _towerAtkTypeText;
    [SerializeField] private Text _towerAtkText;
    [SerializeField] private Text _towerAtkRangeText;
    [SerializeField] private Text _towerAtkSpeedText;


    private void Start()
    {
        MapManager.instance.onSelectedTileChanged += () =>
        {
            if (MapManager.instance.selectedTile.tower != null)
            {
                _selectedtower = MapManager.instance.selectedTile.tower;
                _towerImage.sprite = TowerData.instance.towerDataList[_selectedtower.id].sprite;
                _towerImage.color = TowerData.instance.towerDataList[_selectedtower.id].color;
                _towerNameText.text = $"lv.{_selectedtower.level} {_selectedtower.towerType}";
                _towerAtkTypeText.text = TowerData.instance.towerDataList[_selectedtower.id].attackType.ToString();
                _towerAtkText.text = $"{_selectedtower.atk - _selectedtower.upgrade * _selectedtower.upgradeGain} + {_selectedtower.upgrade * _selectedtower.upgradeGain}";
                _towerAtkRangeText.text = TowerData.instance.towerDataList[_selectedtower.id].atkRange.ToString();
                _towerAtkSpeedText.text = TowerData.instance.towerDataList[_selectedtower.id].atkTime.ToString();
            }

            else
            {
                _selectedtower = null;
                _towerImage.sprite = null;
                _towerImage.color = Color.white;
                _towerNameText.text = "";
                _towerAtkTypeText.text = "";
                _towerAtkText.text = "";
                _towerAtkRangeText.text = "";
                _towerAtkSpeedText.text = "";
            }
        };

        UpgradeManager.instance.onUpgradeDone += () =>
        {
            if (_selectedtower != null)
                _towerAtkText.text = $"{_selectedtower.atk - _selectedtower.upgrade * _selectedtower.upgradeGain} + {_selectedtower.upgrade * _selectedtower.upgradeGain}";
        };

        SpawnManager.instance.onTowerSpawned += () =>
        {
            _selectedtower = MapManager.instance.selectedTile.tower;
            _towerImage.sprite = TowerData.instance.towerDataList[_selectedtower.id].sprite;
            _towerImage.color = TowerData.instance.towerDataList[_selectedtower.id].color;
            _towerNameText.text = $"lv.{_selectedtower.level} {_selectedtower.towerType}";
            _towerAtkTypeText.text = TowerData.instance.towerDataList[_selectedtower.id].attackType.ToString();
            _towerAtkText.text = $"{_selectedtower.atk - _selectedtower.upgrade * _selectedtower.upgradeGain} + {_selectedtower.upgrade * _selectedtower.upgradeGain}";
            _towerAtkRangeText.text = TowerData.instance.towerDataList[_selectedtower.id].atkRange.ToString();
            _towerAtkSpeedText.text = TowerData.instance.towerDataList[_selectedtower.id].atkTime.ToString();
        };

        SpawnManager.instance.onTowerDestroyed += () =>
        {
            _selectedtower = null;
            _towerImage.sprite = null;
            _towerImage.color = Color.white;
            _towerNameText.text = "";
            _towerAtkTypeText.text = "";
            _towerAtkText.text = "";
            _towerAtkRangeText.text = "";
            _towerAtkSpeedText.text = "";
        };
    }
}
