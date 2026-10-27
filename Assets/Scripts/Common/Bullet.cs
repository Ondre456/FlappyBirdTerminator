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
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _maxDistance = 20f;

        private Vector3 _startPosition;
        private Vector2 _direction;
        private Action<Bullet> _release;
        private BulletOwner _owner;
        private Transform _ownerTransform;

        private bool _isReleased;
        private bool _hasHit;

        public void Initialize(
            Vector2 direction,
            Action<Bullet> release,
            BulletOwner owner,
            Transform ownerTransform)
        {
            _direction = direction.normalized;
            _startPosition = transform.position;
            _release = release;
            _owner = owner;
            _ownerTransform = ownerTransform;

            _isReleased = false;
            _hasHit = false;
        }

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

            var bird = other.GetComponentInParent<Bird>();
            var enemy = other.GetComponentInParent<Enemy>();

            switch (_owner)
            {
                case BulletOwner.Bird:
                    if (IsBirdHit(bird)) return;

                    if (IsEnemyHit(enemy))
                    {
                        _hasHit = true;
                        enemy.Die();
                        ReturnToPool();
                    }
                    break;

                case BulletOwner.Enemy:

                    if (IsBirdHit(bird))
                    {
                        _hasHit = true;
                        bird.Die();
                        ReturnToPool();
                    }

                    break;
            }
        }

        private bool IsBirdHit(Bird bird) => bird != null;

        private bool IsEnemyHit(Enemy enemy) => enemy != null;


        private void ReturnToPool()
        {
            if (_isReleased)
                return;

            _isReleased = true;
            _release?.Invoke(this);
        }
    }
}
