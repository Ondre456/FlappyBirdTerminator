using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxSize = 30;

        private ObjectPool<Bullet> _pool;

        private void Awake()
        {
            if (_bulletPrefab == null)
            {
                enabled = false;

                return;
            }

            _pool = new ObjectPool<Bullet>(
                CreateBullet,
                null,
                OnReturnToPool,
                OnDestroyBullet,
                false,
                _defaultCapacity,
                _maxSize
            );
        }

        private void OnDestroy()
        {
            _pool?.Dispose();
        }

        public Bullet Spawn(
            Vector3 position,
            Quaternion rotation,
            Vector2 direction,
            BulletOwner bulletOwner,
            Transform owner
        )
        {
            if (_pool == null)
                return null;

            Bullet bullet = _pool.Get();

            bullet.transform.SetPositionAndRotation(position, rotation);

            bullet.Initialize(
                direction.normalized,
                ReleaseBullet,
                bulletOwner,
                owner
            );

            bullet.gameObject.SetActive(true);

            return bullet;
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);

            return bullet;
        }

        private void ReleaseBullet(Bullet bullet)
        {
            if (bullet == null || _pool == null)
                return;

            _pool.Release(bullet);
        }

        private void OnReturnToPool(Bullet bullet)
        {
            if (bullet != null)
                bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            if (bullet != null)
                Destroy(bullet.gameObject);
        }
    }
}
