using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts
{
    public class EnemyShooter : MonoBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _shootInterval = 2f;

        [Header("Bullet Pool")]
        [SerializeField] private int _defaultCapacity = 5;
        [SerializeField] private int _maxSize = 20;

        private ObjectPool<Bullet> _pool;
        private Bullet _uniqueBulletPrefab;
        private Transform _owner;
        private float _timer;
        private bool _initialized;

        public void Initialize()
        {
            if (_initialized)
                return;

            _owner = transform;
            _uniqueBulletPrefab = Instantiate(_bulletPrefab);
            _uniqueBulletPrefab.gameObject.SetActive(false);
            _uniqueBulletPrefab.name = $"{transform.name}_BulletPrefab";

            _pool = new ObjectPool<Bullet>(
                CreateBullet,
                null,
                OnReturnToPool,
                OnDestroyBullet,
                false,
                _defaultCapacity,
                _maxSize
            );

            _initialized = true;
            _timer = 0f;
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_uniqueBulletPrefab);
            bullet.gameObject.SetActive(false);
            return bullet;
        }

        private void Update()
        {
            if (!_initialized || _pool == null)
                return;

            _timer += Time.deltaTime;

            if (_timer >= _shootInterval)
            {
                Shoot();
                _timer = 0f;
            }
        }

        private void Shoot()
        {
            if (_uniqueBulletPrefab == null || _pool == null)
                return;

            Bullet bullet = _pool.Get();

            bullet.transform.SetPositionAndRotation(
                transform.position,
                transform.rotation
            );

            bullet.Initialize(
                Vector2.left,
                ReleaseBullet,
                BulletOwner.Enemy,
                _owner
            );

            bullet.gameObject.SetActive(true);
        }

        private void ReleaseBullet(Bullet bullet)
        {
            if (_pool != null && bullet != null)
                _pool.Release(bullet);
        }

        private void OnReturnToPool(Bullet bullet)
        {
            bullet?.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet?.gameObject);
        }

        private void OnDestroy()
        {
            if (_bulletPrefab != null)
                _pool?.Dispose();
        }
    }
}