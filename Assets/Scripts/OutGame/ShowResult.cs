using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShowResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    
    [SerializeField] private GameObject _skipButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _backButton;
    private Tween _tween;
    
    // Start is called before the first frame update
    void Start()
    {
        _highScoreText.enabled = false;
        _retryButton.interactable = false;
        _backButton.interactable = false;
        
        float _initialScore = 0;

        // DOTweenアニメーションを作成
        _tween = DOTween.To(
            () => _initialScore,
            x =>
            {
                _initialScore = x;
                _scoreText.text = ((int)_initialScore).ToString();
            },
            ((int)GameManager.score),
            GameManager.score / 12000 // アニメーション時間
        ).OnComplete(() =>
        {
            CheckHighScore();
        });
    }

    public void SkipButtonClicked()
    {
        if (_tween != null) _tween.Complete();
        _skipButton.SetActive(false);
    }

    private void CheckHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") < (int)GameManager.score)
        {
            GameManager.highScore = (int)GameManager.score;
            PlayerPrefs.SetInt("HighScore", GameManager.highScore);
            _highScoreText.enabled = true;
        }
        GameManager.score = 0;      // 今回のスコアはリセット
        
        _skipButton.SetActive(false);       // スキップボタンを消す
        _retryButton.interactable = true;   // 画面遷移を許可
        _backButton.interactable = true;
    }

    private void OnDisable()
    { 
        if (_tween != null) _tween.Kill();
    }
}
