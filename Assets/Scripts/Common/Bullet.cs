using UnityEngine;
using System;

namespace Assets.Scripts
{
    public enum BulletOwner
    {
        Bird,
        Enemy
    }

    public class Bullet : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _maxDistance = 20f;

        [Header("Collision")]
        [SerializeField] private LayerMask _targetLayer;

        private Vector3 _startPosition;
        private Vector2 _direction;
        private Action<Bullet> _release;

        private bool _isReleased;
        private bool _hasHit;

        private void Update()
        {
            if (_isReleased)
                return;

            transform.position +=
                (Vector3)(_direction * _speed * Time.deltaTime);

            if (Vector2.Distance(_startPosition, transform.position)
                >= _maxDistance)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || _isReleased || _hasHit)
                return;

            if ((_targetLayer.value & (1 << other.gameObject.layer)) == 0)
                return;

            Bird bird = other.GetComponentInParent<Bird>();

            if (bird != null)
            {
                _hasHit = true;
                bird.Die();
                ReturnToPool();

                return;
            }

            Enemy enemy = other.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                _hasHit = true;
                enemy.Die();
                ReturnToPool();
            }
        }

        public void Initialize(
            Vector2 direction,
            Action<Bullet> release,
            BulletOwner owner,
            Transform ownerTransform)
        {
            _direction = direction.normalized;
            _startPosition = transform.position;
            _release = release;
            _isReleased = false;
            _hasHit = false;
        }

        private void ReturnToPool()
        {
            if (_isReleased)
                return;

            _isReleased = true;
            _release?.Invoke(this);
        }
    }
}
