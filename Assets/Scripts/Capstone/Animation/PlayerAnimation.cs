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
        private int _animIDSelectGun;
        private int _animIDUnSelectGun;
        private int _animIDShoot;
        private int _animIDCrouchShoot;


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
            _animIDSelectGun = Animator.StringToHash("SelectGun");
            _animIDUnSelectGun = Animator.StringToHash("UnSelectGun");
            _animIDShoot = Animator.StringToHash("Shoot");
            _animIDCrouchShoot = Animator.StringToHash("CrouchShoot");
        }

        public void SetGrounded(bool grounded)
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, grounded);
            }
        }

        public void MoveSpeed(float animationBlend, float inputMagnitude)
        {
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, animationBlend);
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

        public void SetLayer(int layerIndex, float weight)
        {
            if (_hasAnimator)
            {
                _animator.SetLayerWeight(layerIndex, weight);
            }
        }

        public void SelectGun()
        {
            _animator.SetTrigger(_animIDSelectGun);
        }


        public void UnSelectGun()
        {
            if (_hasAnimator)
            {
                _animator.SetTrigger(_animIDUnSelectGun);
            }
        }

        public void ShootAnimation()
        {
            if (_hasAnimator)
            {
               _animator.SetTrigger(_animIDShoot);
            }
        }
        
        public void CrouchShootAnimation()
        {
            if (_hasAnimator)
            {
                _animator.SetTrigger(_animIDCrouchShoot);
            }
        }
    }
}