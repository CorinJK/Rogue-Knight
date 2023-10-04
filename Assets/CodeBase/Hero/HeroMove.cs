using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private float MovementSpeed = 4.0f;

        private CharacterController _characterController;
        private IInputService _input;

        private void Awake()
        {
            // Получаем ссылку
            _input = AllServices.Container.Single<IInputService>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            // Если ввод отсутствует
            Vector3 movementVector = Vector3.zero;

            // Если есть ввод (по квадрату длины вектора)
            if (_input.Axis.sqrMagnitude > Constants.Epsilon)
            {
                // Чтобы трансформировать вектор из экранных координат в мировые (с помощью камеры)
                movementVector = Camera.main.transform.TransformDirection(_input.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                // Развернуть персонажа по направлению движения
                transform.forward = movementVector;
            }

            // Добавление гравитации
            movementVector += Physics.gravity;

            // Само перемещение
            _characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
        }

        // Запись в прогресс
        public void UpdateProgress(PlayerProgress progress) =>
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        // Загрузка из прогресса
        public void LoadProgress(PlayerProgress progress)
        {
            // Если совпал уровень, на котором ходим загрузиться
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                // Взять позицию из сохранения
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                    Warp(to: savedPosition);
            }
        }

        // CharacterController может забагать, поэтому отключаем на момент
        // Добавляем высоту по оси Y, чтобы не застряли ноги в земле
        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}