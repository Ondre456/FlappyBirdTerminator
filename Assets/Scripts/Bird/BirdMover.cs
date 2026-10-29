using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputService))]
    public class BirdMover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _tapForce;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxRotationZ;
        [SerializeField] private float _minRotationZ;

        private Rigidbody2D _rigidbody2D;
        private Quaternion _maxRotation;
        private Quaternion _minRotation;
        private InputService _inputService;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inputService = GetComponent<InputService>();

            _maxRotation = Quaternion.Euler(0, 0, _maxRotationZ);
            _minRotation = Quaternion.Euler(0, 0, _minRotationZ);
        }

        private void Update()
        {
            if (_inputService.JumpPressedThisFrame)
            {
                _rigidbody2D.linearVelocity =
                    new Vector2(_speed, _tapForce);

                transform.rotation = _maxRotation;
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                _minRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
