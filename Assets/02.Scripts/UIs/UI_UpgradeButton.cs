using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeButton : MonoBehaviour
{
    [SerializeField] TowerType _towerType;
    Text _upgradeText;
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _upgradeText = GetComponentInChildren<Text>();

        switch (_towerType)
        {
            case TowerType.None:
                break;
            case TowerType.Diamond:
                _upgradeText.text = $"{UpgradeManager.instance.diamondUpgrade}\n{(UpgradeManager.instance.diamondUpgrade + 1) * 5}G";
                break;
            case TowerType.Hexagon:
                _upgradeText.text = $"{UpgradeManager.instance.hexagonUpgrade}\n{(UpgradeManager.instance.hexagonUpgrade + 1) * 5}G";
                break;
            case TowerType.Triangle:
                _upgradeText.text = $"{UpgradeManager.instance.triangleUpgrade}\n{(UpgradeManager.instance.triangleUpgrade + 1) * 5}G";
                break;
        }
    }

    private void Start()
    {
        switch (_towerType)
        {
            case TowerType.None:
                break;
            case TowerType.Diamond:
                _button.onClick.AddListener(() =>
                {
                    UpgradeManager.instance.UpgradeDiamondTower();
                    _upgradeText.text = $"{UpgradeManager.instance.diamondUpgrade}\n{(UpgradeManager.instance.diamondUpgrade + 1) * 5}G";
                });
                break;
            case TowerType.Hexagon:
                _button.onClick.AddListener(() =>
                {
                    UpgradeManager.instance.UpgradeHexagonTower();
                    _upgradeText.text = $"{UpgradeManager.instance.hexagonUpgrade}\n{(UpgradeManager.instance.hexagonUpgrade + 1) * 5}G";
                });
                break;
            case TowerType.Triangle:
                _button.onClick.AddListener(() =>
                {
                    UpgradeManager.instance.UpgradeTriangleTower();
                    _upgradeText.text = $"{UpgradeManager.instance.triangleUpgrade}\n{(UpgradeManager.instance.triangleUpgrade + 1) * 5}G";
                });
                break;
        }
    }
}