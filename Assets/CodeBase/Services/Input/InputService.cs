using UnityEngine;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire";

        // Получаем из плагина вектор
        public abstract Vector2 Axis { get; }

        // Получаем нажатую кнопку атаки
        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(Button);

        // Ввод с джойстика
        protected static Vector2 SimpleInputAxis() =>
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}