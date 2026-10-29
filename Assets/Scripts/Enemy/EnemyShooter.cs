using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BulletSpawner))]
    public class EnemyShooter : MonoBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private float _shootInterval = 2f;
        [SerializeField] private Vector2 _shootDirection = Vector2.left;

        private float _timer;
        private BulletSpawner _bulletSpawner;

        private void Awake()
        {
            _bulletSpawner = GetComponent<BulletSpawner>();
            _timer = 0f;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer < _shootInterval)
                return;

            Shoot();
            _timer = 0f;
        }

        private void Shoot()
        {
            if (_bulletSpawner == null)
                return;

            _bulletSpawner.Spawn(
                transform.position,
                transform.rotation,
                _shootDirection,
                BulletOwner.Enemy,
                transform
            );
        }
    }
}
