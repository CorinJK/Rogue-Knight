using CodeBase;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    public CharacterController CharacterController;
    public float MovementSpeed = 4.0f;
    private IInputService _inputService;
    private Camera _camera;

    private void Awake()
    {
        // Получаем ссылку
        _inputService = Game.InputService;
    }

    private void Start()
    {
        // Найти первую камеру на сцене
        _camera = Camera.main;

        // Заставить камеру следовать
        CameraFollow();
    }

    private void Update()
    {
        // Если ввод отсутствует
        Vector3 movementVector = Vector3.zero;

        // Если есть ввод (по квадрату длины вектора)
        if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
        {
            // Чтобы трансформировать вектор из экранных координат в мировые (с помощью камеры)
            movementVector = _camera.transform.TransformDirection(_inputService.Axis);
            movementVector.y = 0;
            movementVector.Normalize();

            // Развернуть персонажа по направлению движения
            transform.forward = movementVector;
        }

        // Добавление гравитации
        movementVector += Physics.gravity;

        // Само перемещение
        CharacterController.Move(MovementSpeed * movementVector * Time.deltaTime);
    }

    private void CameraFollow() =>
        _camera.GetComponent<CameraFollow>().Follow(gameObject);
}