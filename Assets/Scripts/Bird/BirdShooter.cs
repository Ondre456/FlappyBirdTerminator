using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BulletSpawner))]
    [RequireComponent(typeof(InputService))]
    public class BirdShooter : MonoBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private Transform _shootPoint;

        private BulletSpawner _bulletSpawner;
        private InputService _inputService;

        private void Awake()
        {
            _bulletSpawner = GetComponent<BulletSpawner>();
            _inputService = GetComponent<InputService>();
        }

        private void Update()
        {
            if (_inputService.ShootPressedThisFrame)
                Shoot();
        }

        private void Shoot()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera == null || _bulletSpawner == null)
                return;

            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(
                _inputService.PointerPosition
            );

            Vector2 direction =
                (Vector2)(mousePosition - _shootPoint.position);

            if (direction.sqrMagnitude < 0.0001f)
                return;

            _bulletSpawner.Spawn(
                _shootPoint.position,
                Quaternion.identity,
                direction.normalized,
                BulletOwner.Bird,
                transform
            );
        }
    }
}
