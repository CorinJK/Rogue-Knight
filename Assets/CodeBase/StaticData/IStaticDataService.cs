using CodeBase.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadMonsters();
        MonsterStaticData ForMonster(MonsterTypeId TypeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId shop);
    }
}