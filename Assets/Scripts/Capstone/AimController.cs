using System;
using Capstone.Mediator;
using Capstone.Movement;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;

        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

        [SerializeField] private Transform debugTransform;
        
        [SerializeField] private bool isAiming;

        public bool IsAming
        {
            get => isAiming;
            set => isAiming = value;
        }

        private CharacterMovement _characterMovement;
        private CameraController _camera;
        
        private Vector3 _mouseWorldPosition;
        public Vector3 MouseWordPosition
        {
            get => _mouseWorldPosition;
            set => _mouseWorldPosition = value;
        }
        
        private void Awake()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            _camera = GetComponent<CameraController>();
        }
        
        private void PerformRaycast()
        {
            _mouseWorldPosition = Vector3.zero;
            
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                debugTransform.position = raycastHit.point;
                _mouseWorldPosition = raycastHit.point;
            }
        }

        public void ControlAimCamera(bool input)
        {
            PerformRaycast();
            
            if (input)
            {
                isAiming = true;
                aimCamera.gameObject.SetActive(true);
                _characterMovement.SetRotation(false);
                _camera.SetSensitivity(aimSensitivity);

                Vector3 wordAimTarget = _mouseWorldPosition;
                wordAimTarget.y = transform.position.y;
                Vector3 aimDirection = (wordAimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                isAiming = false;
                aimCamera.gameObject.SetActive(false);
                _characterMovement.SetRotation(true);
                _camera.SetSensitivity(normalSensitivity);
            }
        }
    }
}