using CodeBase.Data;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Constructor(WorldData worldData)
        {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot)
        {
            _loot = loot;
        }
        
        // Когда игрок наступает на лут
        void OnTriggerEnter(Collider other) => 
            Pickup();
        
        private void Pickup()
        {
            // Чтобы сработало только 1 раз
            if (_picked)
                return;

            _picked = true;

            _worldData.LootData.Collect(_loot);
        }
    }
}