using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;                           // Id спавнера
        public MonsterTypeId monsterTypeId;         // Id монстра
        public Vector3 Position;                    // Позиция спавнера
    }
}