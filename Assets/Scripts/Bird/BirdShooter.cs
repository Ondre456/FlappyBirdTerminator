using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BirdShooter : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxSize = 30;

        private Rigidbody2D _rigidbody2D;
        private ObjectPool<Bullet> _pool;
        private Mouse _mouse;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _mouse = Mouse.current;

            if (_bulletPrefab == null || _shootPoint == null)
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

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            return bullet;
        }

        private void Update()
        {
            if (_mouse != null && _mouse.leftButton.wasPressedThisFrame)
                Shoot();
        }

        private void Shoot()
        {
            if (_pool == null)
                return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

            Vector2 direction = (
                (Vector2)mousePosition - (Vector2)_shootPoint.position
            ).normalized;

            Bullet bullet = _pool.Get();

            bullet.transform.SetPositionAndRotation(
                _shootPoint.position,
                Quaternion.identity
            );

            bullet.Initialize(
                direction,
                ReleaseBullet,
                BulletOwner.Bird,
                transform
            );

            bullet.gameObject.SetActive(true);
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

        private void OnDestroy()
        {
            _pool?.Dispose();
        }
    }
}
