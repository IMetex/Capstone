using System;
using System.Collections;
using Capstone.Animation;
using Capstone.Input;
using Capstone.Movement;
using Capstone.Shoot;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Capstone.Mediator
{
    public class PlayerMediator : MonoBehaviour
    {
        [SerializeField] private GameObject _backContainerVisibilty;
        [SerializeField] private GameObject _handGunVisibilty;
        
        private GameInput _gameInput;
        
        // references
        private PlayerAnimation _playerAnimation;
        private CharacterMovement _characterMovement;
        private CameraController _cameraController;
        private Shooter _shooter;
        private AimController _aimController;

        [Header("Character Input Values")] public bool jump;

        [Header("Mouse Cursor Settings")] public bool cursorInputForLook = true;

        private bool _isCrouchButtonDown = false;
        private bool _isSelectGun = false;

        public bool IsSelectGun => _isSelectGun;
        

        private void Awake()
        {
            _gameInput = new GameInput();

            _playerAnimation = GetComponent<PlayerAnimation>();
            _characterMovement = GetComponent<CharacterMovement>();
            _shooter = GetComponent<Shooter>();
            _aimController = GetComponent<AimController>();
            _cameraController = GetComponent<CameraController>();
        }

        private void OnEnable()
        {
            _gameInput.Enable();

            _gameInput.Player.Crouch.performed += OnCrouchRequest;
            _gameInput.Player.Shoot.performed += OnShootRequest;
            _gameInput.Player.GunSelect.performed += OnSelectGunRequest;
        }


        private void OnDisable()
        {
            _gameInput.Disable();

            _gameInput.Player.Crouch.performed -= OnCrouchRequest;
            _gameInput.Player.Shoot.performed -= OnShootRequest;
            _gameInput.Player.GunSelect.performed -= OnSelectGunRequest;
        }


        private void Update()
        {
            HandleMovementInput();
            HandleSprintInput();

            HandleJumpInput();

            HandleLookInput();
            HandleAim();
        }

        private void HandleMovementInput()
        {
            var movementInput = _gameInput.Player.Move.ReadValue<Vector2>();
            _characterMovement.MovementInput = movementInput;
        }

        private void HandleSprintInput()
        {
            if (_isCrouchButtonDown) return;

            var sprintInput = _gameInput.Player.Sprint.IsPressed();
            _characterMovement.Sprint = sprintInput;
        }

        private void HandleLookInput()
        {
            if (cursorInputForLook)
            {
                var lookInput = _gameInput.Player.Look.ReadValue<Vector2>();
                _cameraController.LookInput = lookInput;
            }
        }

        private void HandleJumpInput()
        {
            if (_isCrouchButtonDown) return;

            var jumpInput = _gameInput.Player.Jump.IsPressed();
            jump = jumpInput;
        }

        private void HandleAim()
        {
            var aimInput = _gameInput.Player.Aim.IsPressed();

            _aimController.ControlAimCamera(aimInput);
        }

        private void OnSelectGunRequest(InputAction.CallbackContext obj)
        {
            if (_characterMovement.grounded)
            {
                _isSelectGun = !_isSelectGun;

                if (_isSelectGun)
                {
                    _playerAnimation.SetLayer(1, 1f);
                    _playerAnimation.SelectGun();
                    SetGunVisibility(true,false);
                }
                else
                {
                    _playerAnimation.SetLayer(1, 0f);
                    _playerAnimation.UnSelectGun();
                    SetGunVisibility(false,true);
                }
            }
        }


        private void OnCrouchRequest(InputAction.CallbackContext obj)
        {
            if (_characterMovement.grounded)
            {
                _isCrouchButtonDown = !_isCrouchButtonDown;

                if (_isCrouchButtonDown)
                {
                    _playerAnimation.Crouch(true);
                }
                else
                {
                    _playerAnimation.Crouch(false);
                }
            }
        }

        private void OnShootRequest(InputAction.CallbackContext obj)
        {
            _shooter.Shoot();
        }

        private void SetGunVisibility(bool handGun,bool backGun)
        {
            _handGunVisibilty.SetActive(handGun);
            _backContainerVisibilty.SetActive(backGun);
        }
        
    }
}