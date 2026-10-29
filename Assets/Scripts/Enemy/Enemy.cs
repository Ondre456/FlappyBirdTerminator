using System;
using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyShooter))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _enemyMover;
    private EnemyShooter _enemyShooter;
    private bool _isDead;
    private Rigidbody2D _body2D;

    private Action<Enemy> _onDied;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
        _enemyShooter = GetComponent<EnemyShooter>();
        _body2D = GetComponent<Rigidbody2D>();
        _body2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Initialize(Action<Enemy> onDied, Bird bird)
    {
        _onDied = onDied;
        _isDead = false;

        _enemyMover.Initialize(bird);
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
}
