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
                _upgradeText.text = $"Diamond\n{UpgradeManager.instance.diamondUpgrade} ¡æ {UpgradeManager.instance.diamondUpgrade + 1}\n{(UpgradeManager.instance.diamondUpgrade + 1) * 5} Gold";
                break;
            case TowerType.Hexagon:
                _upgradeText.text = $"Hexagon\n{UpgradeManager.instance.hexagonUpgrade} ¡æ {UpgradeManager.instance.hexagonUpgrade + 1}\n{(UpgradeManager.instance.hexagonUpgrade + 1) * 5} Gold";
                break;
            case TowerType.Triangle:
                _upgradeText.text = $"Triangle\n{UpgradeManager.instance.triangleUpgrade} ¡æ {UpgradeManager.instance.triangleUpgrade + 1}\n{(UpgradeManager.instance.triangleUpgrade + 1) * 5} Gold";
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
                    _upgradeText.text = $"Diamond\n{UpgradeManager.instance.diamondUpgrade} ¡æ {UpgradeManager.instance.diamondUpgrade + 1}\n{(UpgradeManager.instance.diamondUpgrade + 1) * 5} Gold";
                 });
                break;
            case TowerType.Hexagon:
                _button.onClick.AddListener(() => {
                    UpgradeManager.instance.UpgradeHexagonTower();
                    _upgradeText.text = $"Hexagon\n{UpgradeManager.instance.hexagonUpgrade} ¡æ {UpgradeManager.instance.hexagonUpgrade + 1}\n{(UpgradeManager.instance.hexagonUpgrade + 1) * 5} Gold";
                });
                break;
            case TowerType.Triangle:
                _button.onClick.AddListener(() => {
                    UpgradeManager.instance.UpgradeTriangleTower();
                    _upgradeText.text = $"Triangle\n{UpgradeManager.instance.triangleUpgrade} ¡æ {UpgradeManager.instance.triangleUpgrade + 1}\n{(UpgradeManager.instance.triangleUpgrade + 1) * 5} Gold";
                });
                break;
        }
    }
}