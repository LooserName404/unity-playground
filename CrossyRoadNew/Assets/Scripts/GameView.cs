using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{

    private float _score;
    private int _maxDistance = 0;
    private int _currentDistance = 0;

    [SerializeField] private TextMeshProUGUI _scoreTxt;

    public Action<int> onScoreIncreased;

    private Player _player;
    private void Awake() {
        _player = FindObjectOfType<Player>();
        _player.onMove += OnPlayerMove;
        SetScore();
    }

    private void OnPlayerMove(Vector2Int dir) {
        _currentDistance += dir.y;
        if (_currentDistance > _maxDistance) {
            _maxDistance = _currentDistance;
            SetScore();
            if (onScoreIncreased != null) {
                onScoreIncreased(_maxDistance);
            }
        }
    }

    private void SetScore() {
        _scoreTxt.text = _maxDistance.ToString();
    }
}
