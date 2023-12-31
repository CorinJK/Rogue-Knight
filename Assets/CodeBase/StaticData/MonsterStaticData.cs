﻿using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 100)]
        public int Hp;

        [Range(1f, 30f)]
        public float Damage;

        public int MaxLoot;
        
        public int MinLoot;
        
        [Range(0.5f, 1f)]
        public float EffectiveDistance = 0.666f;

        [Range(0.5f, 1f)]
        public float CLeavage;

        [Range(0, 10)]
        public float MoveSpeed = 3;

        public AssetReferenceGameObject PrefabReference;
    }
}