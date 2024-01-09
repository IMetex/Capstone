using System;
using Capstone.Animation;
using Capstone.Mediator;
using UnityEngine;

namespace Capstone
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private float fireRate = 0.5f;

        private float _lastShootTime;
        public bool CanShoot => Time.time > _lastShootTime + fireRate;
        
        [SerializeField] private GameObject bulletProjectile;
        [SerializeField] private Transform spawnBulletPosition;
        
        private AimController _aimController;
        private PlayerMediator _mediator;
        private PlayerAnimation _playerAnimation;
        private void Awake()
        {
            _aimController = GetComponent<AimController>();
            _mediator = GetComponent<PlayerMediator>();
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        public void Shoot()
        {
            if (!CanShoot) return;

            if (_aimController.IsAiming && _mediator.IsSelectGun)
            {
                Vector3 aimDir = (_aimController.MouseWordPosition - spawnBulletPosition.position).normalized;
                Instantiate(bulletProjectile, spawnBulletPosition.position,
                    Quaternion.LookRotation(aimDir, Vector3.up));
                _playerAnimation.ShootAnimation();
                _playerAnimation.CrouchShootAnimation();
                
            }
            
            _lastShootTime = Time.time;
        }
    }
}