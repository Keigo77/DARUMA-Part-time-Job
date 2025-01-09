using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] private Button _shareButton;
    [SerializeField] private AudioClip _resultSound;
    [SerializeField] private AudioClip _showScoreSound;

    private int _showScore = 0;
    private Tween _tween;

    private void Awake()
    {
        if (GameManager.isEyeEnd) _showScore = -1000000;
        else _showScore = (int)GameManager.score;
    }

    // Start is called before the first frame update
    void Start()
    {
        SESingleton.seInstance.PlaySE(_resultSound);
        _highScoreText.enabled = false;
        _retryButton.interactable = false;
        _backButton.interactable = false;
        _shareButton.interactable = false;
        
        float _initialScore = 0;

        // DOTweenアニメーションを作成
        _tween = DOTween.To(
            () => _initialScore,
            x =>
            {
                _initialScore = x;
                _scoreText.text = ((int)_initialScore).ToString();
            },
            _showScore,
            GameManager.score / 20000 // アニメーション時間
        ).OnComplete(() =>
        {
            SESingleton.seInstance.PlaySE(_showScoreSound);
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
        if (TimeManager.isHighScore) _highScoreText.enabled = true;
        _skipButton.SetActive(false);       // スキップボタンを消す
        _retryButton.interactable = true;   // 画面遷移を許可
        _backButton.interactable = true;
        _shareButton.interactable = true;
    }
    
    //シェア機能
    public void Share()
    {
        StartCoroutine(ShareCoroutine());
    }
    public IEnumerator ShareCoroutine()
    {
        const string fileName = "image.png";
        string imgPath = Path.Combine(Application.persistentDataPath, fileName);
        // 前回のデータを削除
        if (File.Exists(imgPath)) File.Delete(imgPath);
        //スクリーンショットを撮影
        ScreenCapture.CaptureScreenshot(fileName);
        // 撮影画像の保存が完了するまで待機
        while (true)
        {
            if (File.Exists(imgPath)) break;
            yield return null;
        }
        // 投稿する
        string tweetText = $"今回のスコアは{GameManager.score}でした！\n#だる目　で色んなだるまを完成させよう！";
        string tweetURL = "https://apps.apple.com/jp/app/%E3%81%A0%E3%82%8B%E7%9B%AE/id6739993039";
        try
        {
            SocialConnector.SocialConnector.Share(tweetText, tweetURL, imgPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnDisable()
    { 
        if (_tween != null) _tween.Kill();
    }
}
