using Assets.Scripts;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Bird _bird;

    [Header("Horizontal Movement")]
    [SerializeField] private float _desiredDistance = 8f;
    [SerializeField] private float _distanceTolerance = 2f;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 5f;

    [Header("Vertical Movement")]
    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;
    [SerializeField] private float _verticalSpeed = 2f;
    [SerializeField] private float _verticalTolerance = 0.2f;

    [Header("Avoid Other Enemies")]
    [SerializeField] private float _avoidRadius = 2f;
    [SerializeField] private float _avoidStrength = 2f;
    [SerializeField] private LayerMask _enemyLayer;

    private float _horizontalSpeed;

    private void OnEnable()
    {
        _horizontalSpeed = _minSpeed;
    }

    public void Initialize(Bird bird)
    {
        _bird = bird;
    }

    private void Update()
    {
        if (_bird == null)
            return;

        MoveHorizontally();
        MoveVertically();
    }

    private void MoveHorizontally()
    {
        float distance =
            transform.position.x - _bird.transform.position.x;

        float targetSpeed = _horizontalSpeed;

        if (distance < _desiredDistance - _distanceTolerance)
            targetSpeed = _maxSpeed;
        else if (distance > _desiredDistance + _distanceTolerance)
            targetSpeed = _minSpeed;

        _horizontalSpeed = Mathf.MoveTowards(
            _horizontalSpeed,
            targetSpeed,
            _acceleration * Time.deltaTime
        );

        transform.position +=
            Vector3.right * _horizontalSpeed * Time.deltaTime;
    }

    private void MoveVertically()
    {
        float birdY = _bird.transform.position.y;
        float currentY = transform.position.y;

        float targetDirection = 0f;
        float differenceY = birdY - currentY;

        if (Mathf.Abs(differenceY) > _verticalTolerance)
            targetDirection = Mathf.Sign(differenceY);

        float avoidanceDirection = 0f;

        Collider2D[] neighbors = Physics2D.OverlapCircleAll(
            transform.position,
            _avoidRadius,
            _enemyLayer
        );

        foreach (Collider2D neighbor in neighbors)
        {
            EnemyMover other =
                neighbor.GetComponentInParent<EnemyMover>();

            if (other == null || other == this)
                continue;

            float verticalDifference =
                transform.position.y - other.transform.position.y;

            if (Mathf.Abs(verticalDifference) < 0.1f)
            {
                verticalDifference =
                    GetEntityId() > other.GetEntityId() ? 1f : -1f;
            }

            float distance = Vector2.Distance(
                transform.position,
                other.transform.position
            );

            float influence = 1f -
                Mathf.Clamp01(distance / _avoidRadius);

            avoidanceDirection +=
                Mathf.Sign(verticalDifference) * influence;
        }

        float verticalDirection = Mathf.Clamp(
            targetDirection + avoidanceDirection * _avoidStrength,
            -1f,
            1f
        );

        Vector3 position = transform.position;

        position.y += verticalDirection *
                      _verticalSpeed *
                      Time.deltaTime;

        position.y = Mathf.Clamp(position.y, _minY, _maxY);

        transform.position = position;
    }
}
