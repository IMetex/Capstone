using System;
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

        private void Awake()
        {
            _aimController = GetComponent<AimController>();
        }

        public void Shoot()
        {
            if (!CanShoot) return;

            if (_aimController.IsAming)
            {
                Vector3 aimDir = (_aimController.MouseWordPosition - spawnBulletPosition.position).normalized;
                
                Instantiate(bulletProjectile, spawnBulletPosition.position,
                    Quaternion.LookRotation(aimDir, Vector3.up));
            }
            else
            {
                Instantiate(bulletProjectile, spawnBulletPosition.position,spawnBulletPosition.rotation);
            }
            
            _lastShootTime = Time.time;
        }
    }
}