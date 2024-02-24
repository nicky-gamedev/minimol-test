using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _scoreEndText;
    [SerializeField] private TextMeshProUGUI _endGameTitle;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _scoreFrame;
    
    void Awake()
    {
        GameManager.Instance.OnScoreUpdate += OnScoreUpdate;
        GameManager.Instance.OnGameEnd += EndGame;
    }

    private void EndGame(bool isWin)
    {
        _endGameTitle.text = isWin ? "You Win" : "You Lose";
        _scoreEndText.text = "Score: " + GameManager.Instance.TotalScore;
        _endGamePanel.gameObject.SetActive(true);
        _scoreFrame.gameObject.SetActive(false);
    }
    
    private void OnScoreUpdate()
    {
        _scoreText.text = "Score: " + GameManager.Instance.TotalScore;
    }
}
