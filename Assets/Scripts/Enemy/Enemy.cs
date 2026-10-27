using System;
using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyShooter))]
[RequireComponent(typeof(EnemyCollisionHandler))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _enemyMover;
    private EnemyShooter _enemyShooter;
    private EnemyCollisionHandler _collisionHandler;
    private bool _isDead;
    private Rigidbody2D _body2D;

    private Action<Enemy> _onDied;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
        _enemyShooter = GetComponent<EnemyShooter>();
        _collisionHandler = GetComponent<EnemyCollisionHandler>();
        _body2D = GetComponent<Rigidbody2D>();
        _body2D.isKinematic = true;

        _collisionHandler.CollisionDetected += ProcessCollision;
    }

    public void Initialize(Action<Enemy> onDied, Bird bird)
    {
        _onDied = onDied;
        _isDead = false;

        _enemyShooter.Initialize();
        _enemyMover.Initialize(bird);
    }

    private void ProcessCollision(Collider2D other)
    {
        if (_isDead)
            return;
    }

    public void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        if (_onDied != null)
        {
            _onDied.Invoke(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (_collisionHandler != null)
            _collisionHandler.CollisionDetected -= ProcessCollision;
    }
}
