using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capstone.Movement
{
    public class ProjectileMovement : MonoBehaviour
    {
        [SerializeField] private float speed;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        [SerializeField] private bool shouldDestroyOnCollision;

        public bool ShouldDestroyOnCollision
        {
            get => shouldDestroyOnCollision;
            set => shouldDestroyOnCollision = value;
        }

        [SerializeField] private bool shouldDisableOnCollision;

        public bool ShouldDisableOnCollision
        {
            get => shouldDisableOnCollision;
            set => shouldDisableOnCollision = value;
        }

        [SerializeField] private float pushPower;

        private void Update()
        {
            var direction = transform.forward;
            var distance = speed * Time.deltaTime;
            var targetPosition = transform.position + direction * distance;

            if (Physics.Raycast(transform.position, direction, out var hit, distance))
            {
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(-hit.normal * (speed * pushPower), hit.point, ForceMode.Impulse);
                }

                if (ShouldDestroyOnCollision)
                {
                    Destroy(gameObject);
                }

                if (ShouldDisableOnCollision)
                {
                    enabled = false;
                }

                targetPosition = hit.point;
            }

            Debug.DrawLine(transform.position, targetPosition, Color.red);
            transform.position = targetPosition;
            Debug.DrawRay(transform.position, direction * distance, Color.blue);
        }
    }
}