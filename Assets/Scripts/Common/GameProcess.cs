using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Bird _bird;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameObject _gameOverPanel;

        private bool _isGameOver;

        private void Awake()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (_bird != null)
                _bird.GameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            if (_bird != null)
                _bird.GameOver -= HandleGameOver;
        }

        private void HandleGameOver()
        {
            if (_isGameOver)
                return;

            _isGameOver = true;

            Debug.Log("Game Over!");

            if (_enemySpawner != null)
                _enemySpawner.enabled = false;

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);
        }

        public void RestartGame()
        {
            Debug.Log("asdasd");
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );

            Debug.Log("asdasd");
        }
    }
}
