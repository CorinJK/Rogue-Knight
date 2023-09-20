using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;                           // Id спавнера
        public MonsterTypeId MonsterTypeId;         // Id монстра
        public Vector3 Position;                    // Позиция спавнера

        public EnemySpawnerData(string id, MonsterTypeId monsterTypeId, Vector3 position)
        {
            Id = id;
            MonsterTypeId = monsterTypeId;
            Position = position;
        }
    }
}