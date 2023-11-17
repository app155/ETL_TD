using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Datum
{
    public class TowerData : MonoBehaviour
    {
        public static TowerData instance => _instance;

        private static TowerData _instance;

        public List<TowerDataSO> towerDataList;

        void Awake()
        {
            _instance = this;
        }
    }
}

