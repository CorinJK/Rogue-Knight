namespace CodeBase.Infrastructure.Services
{
    // Получение реализации по запросу какого-либо интерфейса
    public class AllServices
    {
        private static AllServices _instanse;

        public static AllServices Container => _instanse ?? (_instanse = new AllServices());

        // Регистрация единоразовой реализации
        // Ограничение: where TService : IService 
        public void RegisterSingle<TService>(TService implementation) where TService : IService => 
            Implementation<TService>.ServiceInstance = implementation;

        // Где надо взять
        public TService Single<TService>() where TService : IService => 
            Implementation<TService>.ServiceInstance;

        // Специально вложенный класс
        // Имеет дженерик параметр и для каждого случая, где он используется генериться отдельный класс
        // Но, недостаток,  она будет жить весь рантайм
        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}