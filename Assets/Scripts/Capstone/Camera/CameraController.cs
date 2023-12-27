using System;
using Capstone.Mediator;
using UnityEngine;

namespace Capstone
{
    public class CameraController : MonoBehaviour
    {
        // referance
        private PlayerMediator _input;
        public Vector2 LookInput { get; set;}
        
        [Header("Cinemachine")] 
        [SerializeField] private GameObject cinemachineCameraTarget;
        [SerializeField] private float topClamp = 70.0f;
        [SerializeField] private float bottomClamp = -30.0f;
        [SerializeField] private float cameraAngleOverride = 0.0f;
        [SerializeField] private bool lockCameraPosition = false;
        
        [SerializeField] private float sensitivity = 1f;
        
        [Header("Mouse Cursor Settings")] 
        public bool cursorLocked = true;
        
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private const float Threshold = 0.01f;

        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _input = GetComponent<PlayerMediator>();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (LookInput.sqrMagnitude >= Threshold && !lockCameraPosition)
            {
                _cinemachineTargetYaw += LookInput.x * sensitivity;
                _cinemachineTargetPitch += LookInput.y * sensitivity;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        public void SetSensitivity(float newSensitivity)
        {
            sensitivity = newSensitivity;
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
