using System;
using Capstone.Animation;
using Capstone.Input;
using Capstone.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Capstone.Mediator
{
    public class PlayerMediator : MonoBehaviour
    {
        private GameInput _gameInput;
        
        // references
        private PlayerAnimation _animation;
        private CharacterMovement _characterMovement;
        private CameraController _cameraController;
        private Shooter _shooter;
        private AimController _aimController;

        [Header("Character Input Values")] 
        public bool jump;
        
        [Header("Mouse Cursor Settings")] 
        public bool cursorInputForLook = true;

        private bool _isCrouchButtonDown = false;


        private void Awake()
        {
            _gameInput = new GameInput();

            _animation = GetComponent<PlayerAnimation>();
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
        }


        private void OnDisable()
        {
            _gameInput.Disable();
            
            _gameInput.Player.Crouch.performed -= OnCrouchRequest;
            _gameInput.Player.Shoot.performed -= OnShootRequest;
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
        
        
        private void OnCrouchRequest(InputAction.CallbackContext obj)
        {
            if (_characterMovement.grounded)
            {
                _isCrouchButtonDown = !_isCrouchButtonDown;

                if (_isCrouchButtonDown)
                {
                    _animation.Crouch(true);
                }
                else
                {
                    _animation.Crouch(false);
                }
            }
        }
        
        private void OnShootRequest(InputAction.CallbackContext obj)
        {
            _shooter.Shoot();
        }
    }
}