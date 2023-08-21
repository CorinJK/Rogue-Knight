﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

        // Загрузить монстра
        public void LoadMonsters()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(x => x.MonsterTypeId, x => x);
        }

        // Инкапсулированный доступ к StaticData
        public MonsterStaticData ForMonster(MonsterTypeId TypeId) =>
            _monsters.TryGetValue(TypeId, out MonsterStaticData staticData) ? staticData : null;
    }
}