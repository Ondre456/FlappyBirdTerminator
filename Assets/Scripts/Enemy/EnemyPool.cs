using Assets.Scripts;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ScoreCounter))]
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Bird _bird;

    [Header("Spawn Position")]
    [SerializeField] private float _distanceFromBird = 10f;
    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;

    [Header("Spawn Settings")]
    [SerializeField] private int _maxEnemies = 3;
    [SerializeField] private float _spawnInterval = 2f;

    private ObjectPool<Enemy> _enemyPool;
    private float _spawnTimer;
    private int _activeEnemies;
    private ScoreCounter _scoreCounter;
    private bool _isDisposed;

    private void Awake()
    {
        _isDisposed = false;

        _enemyPool = new ObjectPool<Enemy>(
            CreateEnemy,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyEnemy,
            false,
            _maxEnemies,
            _maxEnemies
        );

        _scoreCounter = GetComponent<ScoreCounter>();
    }


    private void Update()
    {
        if (_bird == null || _activeEnemies >= _maxEnemies)
            return;

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = _enemyPool.Get();

        Vector3 birdPosition = _bird.transform.position;

        enemy.transform.position = new Vector3(
            birdPosition.x + _distanceFromBird,
            Random.Range(_minY, _maxY),
            birdPosition.z
        );

        enemy.Initialize(OnEnemyDied, _bird);

        _activeEnemies++;
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab);
        enemy.gameObject.SetActive(false);

        return enemy;
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void OnEnemyDied(Enemy enemy)
    {
        if (_isDisposed || enemy == null)
            return;

        _scoreCounter?.AddScore();

        if (_activeEnemies > 0)
            _activeEnemies--;

        _enemyPool.Release(enemy);
    }

    private void OnDestroy()
    {
        _isDisposed = true;
        _enemyPool?.Dispose();
    }
}