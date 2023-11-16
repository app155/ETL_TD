using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Datum
{
    public class RoundEnemyData : MonoBehaviour
    {
        public static RoundEnemyData instance
        {
            get { return _instance; }
        }

        private static RoundEnemyData _instance;

        public List<EnemySpawnData> enemyDataList;

        private void Awake()
        {
            //if (_instance != null)
            //{
            //    Destroy(gameObject);
            //}
            //_instance = this;

            //DontDestroyOnLoad(gameObject);

            _instance = this;
        }
    }

}
