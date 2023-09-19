using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        MonsterStaticData ForMonster(MonsterTypeId TypeId);
        LevelStaticData ForLevel(string sceneKey);
        void LoadMonsters();
    }
}