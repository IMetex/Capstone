using UnityEngine;

namespace Capstone.Animation
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        private bool _hasAnimator;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;
        

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hasAnimator = TryGetComponent(out _animator);

            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDCrouch = Animator.StringToHash("Crouch");
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
        }

        public void SetGrounded(bool grounded)
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, grounded);
            }
        }

        public void MoveSpeed(float _animationBlend, float inputMagnitude)
        {
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public void JumpStart()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }
        }

        public void JumpAir()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, true);
            }
        }

        public void JumpFall()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDFreeFall, true);
            }
        }

        public void Crouch(bool isPressButton)
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDCrouch, isPressButton);
            }
        }
    }
}