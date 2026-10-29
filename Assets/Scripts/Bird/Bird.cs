using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BirdMover))]
    [RequireComponent(typeof(BirdCollizionHandler))]
    [RequireComponent(typeof(BirdShooter))]
    public class Bird : MonoBehaviour
    {
        public event Action GameOver;

        private BirdMover _birdMover;
        private BirdCollizionHandler _birdCollizionHandler;
        private BirdShooter _birdShooter;

        private bool _isDead;

        private void Awake()
        {
            _birdMover = GetComponent<BirdMover>();
            _birdCollizionHandler = GetComponent<BirdCollizionHandler>();
            _birdShooter = GetComponent<BirdShooter>();

            _birdCollizionHandler.CollizionDetected += ProcessCollizion;
        }

        private void OnDestroy()
        {
            if (_birdCollizionHandler != null)
                _birdCollizionHandler.CollizionDetected -= ProcessCollizion;
        }

        public void Die()
        {
            if (_isDead)
                return;

            _isDead = true;

            Debug.Log("ѕтица погибла: " + gameObject.name);

            GameOver?.Invoke();

            _birdMover.enabled = false;
            _birdShooter.enabled = false;
        }

        private void ProcessCollizion(BoxCollider2D box)
        {
            Die();
        }
    }
}
