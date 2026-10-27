using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int _scorePerEnemy = 5;

    private int _score;

    public Action<int> ScoreChanged;

    public int Score => _score;

    public void AddScore()
    {
        _score += _scorePerEnemy;
        ScoreChanged?.Invoke(_score);
    }
}