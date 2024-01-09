using Capstone.Animation;
using Capstone.Mediator;
using UnityEngine;

namespace Capstone.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Player")] 
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float sprintSpeed = 5.335f;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }
        public Vector2 MovementInput { get; set; }
        public bool Sprint { get; set; }
        public bool analogMovement;

        [Header("Smooth Rotation")] 
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        [SerializeField] private float speedChangeRate = 10.0f;

        [Header("Gravity Value")] [SerializeField]
        private float gravity = -15.0f;

        [Header("Jump Values")] 
        [SerializeField] private float jumpHeight = 1.2f;

        [SerializeField] private float jumpTimeout = 0.50f;
        [SerializeField] private float fallTimeout = 0.15f;

        [Header("Player Grounded")] 
        public bool grounded = true;
        [SerializeField] private float groundedOffset = -0.14f;
        [SerializeField] private float groundedRadius = 0.28f;
        [SerializeField] private LayerMask groundLayers;

        // rotation
        private bool _rotationOnMove = true;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private const float TerminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // references
        private CharacterController _controller;
        private PlayerAnimation _playerAnimation;
        private PlayerMediator _input;
        private GameObject _mainCamera;


        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerMediator>();
            _playerAnimation = GetComponent<PlayerAnimation>();

            // reset our timeouts on start
            _jumpTimeoutDelta = jumpTimeout;
            _fallTimeoutDelta = fallTimeout;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);

            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);

            // Ground Animation
            _playerAnimation.SetGrounded(grounded);
        }

        private void Move()
        {
            float targetSpeed = Sprint ? sprintSpeed : moveSpeed;

            if (MovementInput == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = analogMovement ? MovementInput.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Rotation();

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // Animation
            _playerAnimation.MoveSpeed(_animationBlend, inputMagnitude);
        }

        private void Rotation()
        {
            Vector3 inputDirection = new Vector3(MovementInput.x, 0.0f, MovementInput.y).normalized;

            if (MovementInput != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                if (_rotationOnMove)
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        public void JumpAndGravity()
        {
            if (grounded)
            {
                _fallTimeoutDelta = fallTimeout;

                _playerAnimation.JumpStart();

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    _playerAnimation.JumpAir();
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = jumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _playerAnimation.JumpFall();
                }

                _input.jump = false;
            }

            Gravity();
        }

        private void Gravity()
        {
            if (_verticalVelocity < TerminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }

        public void SetRotation(bool newRotation)
        {
            _rotationOnMove = newRotation;
        }
    }
}