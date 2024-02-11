using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Vector2 MoveInput { get; private set; }
    public bool DodgeInput { get; private set; }
    public bool AttackInput {  get; private set; }
    public bool InteractInput {  get; private set; }
    public bool InventoryInput { get; private set; }
    public bool CameraLeftInput {  get; private set; }
    public bool CameraRightInput {  get; private set; }
    public bool MenuOpenCloseInput { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _dodgeAction;
    private InputAction _attackAction;
    private InputAction _interactAction;
    private InputAction _inventoryAction;
    private InputAction _cameraLeftAction;
    private InputAction _cameraRightAction;
    private InputAction _menuOpenCloseAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _dodgeAction = _playerInput.actions["Dodge"];
        _attackAction = _playerInput.actions["Attack"];
        _interactAction = _playerInput.actions["Interact"];
        _inventoryAction = _playerInput.actions["Inventory"];
        _cameraLeftAction = _playerInput.actions["RotateCameraLeft"];
        _cameraRightAction = _playerInput.actions["RotateCameraRight"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }

    private void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        DodgeInput = _dodgeAction.WasPressedThisFrame();
        AttackInput = _attackAction.WasPressedThisFrame();
        InteractInput = _interactAction.WasPressedThisFrame();
        InventoryInput = _inventoryAction.WasPressedThisFrame();
        CameraLeftInput = _cameraLeftAction.WasPressedThisFrame();
        CameraRightInput = _cameraRightAction.WasPressedThisFrame();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}