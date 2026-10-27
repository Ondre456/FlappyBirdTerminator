using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyCollisionHandler : MonoBehaviour
{
    public event Action<Collider2D> CollisionDetected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionDetected?.Invoke(other);
    }
}