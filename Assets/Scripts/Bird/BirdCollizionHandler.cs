using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Bird))]
    [RequireComponent(typeof(Collider2D))]
    public class BirdCollizionHandler : MonoBehaviour
    {
        public Action<BoxCollider2D> CollizionDetected;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out BoxCollider2D interacatable))
            {
                CollizionDetected?.Invoke(interacatable);
            }
        }
    }
}
