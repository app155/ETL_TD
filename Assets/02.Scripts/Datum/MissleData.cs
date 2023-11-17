using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Datum
{
    public class MissleData : MonoBehaviour
    {
        public static MissleData instance => _instance;

        private static MissleData _instance;

        public List<MissleDataSO> missleDataList;

        void Awake()
        {
            _instance = this;
        }
    }
}


