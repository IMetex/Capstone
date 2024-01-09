using System;
using UnityEngine;
using UnityEngine.AI;

namespace Capstone.Enemy
{
    public class AILocomotion : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _maxTime = 1.0f;
        [SerializeField] private float _maxDistance = 1.0f;

        private NavMeshAgent _agent;
        private Animator _animator;
        private float _timer = 0f;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer < 0.0f)
            {
                float sqrDistance = (_playerTransform.position - _agent.destination).sqrMagnitude;
                if (sqrDistance > _maxDistance * _maxDistance)
                {
                    _agent.destination = _playerTransform.position;
                }

                _timer = _maxTime;
            }

            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }
}