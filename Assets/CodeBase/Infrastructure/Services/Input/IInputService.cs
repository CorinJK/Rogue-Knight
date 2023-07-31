using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }

        // Нажата ли кнопка атаки
        bool IsAttackButtonUp();
    }
}