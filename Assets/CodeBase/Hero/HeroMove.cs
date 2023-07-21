using CodeBase;
using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 4.0f;
    private CharacterController _characterController;
    private IInputService _input;
    private Camera _camera;

    private void Awake()
    {
        // Получаем ссылку
        _input = Game.InputService;
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
}