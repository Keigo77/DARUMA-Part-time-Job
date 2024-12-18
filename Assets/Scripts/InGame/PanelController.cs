using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _stopPanel;
    [SerializeField] private GameObject _finishPanel;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _stopPanel.SetActive(false);
        _finishPanel.SetActive(false);
    }

    public void DeleteStartPanel()
    {
        _clearPanel.SetActive(false);
        _startPanel.SetActive(false);
    }
    
    public void DeleteStopPanel()
    {
        _clearPanel.SetActive(false);
        _stopPanel.SetActive(false);
    }
    
    public void DeleteFinishPanel()
    {
        _clearPanel.SetActive(false);
        _finishPanel.SetActive(false);
    }

    public void ShowStopPanel()
    {
        _clearPanel.SetActive(true);
        _stopPanel.SetActive(true);
    }
    
    public async void ShowFinishPanel()
    {
        _clearPanel.SetActive(true);
        _finishPanel.SetActive(true);
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: _cancellationTokenSource.Token);
        }
        catch
        {
            Debug.Log("キャンセル");
        }
        SceneManager.LoadScene("ResultScene");
    }
    
    // 中断ボタンの処理
    public void RestartButtonClicked()      // 今のシーンをリロード
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButtonClicked()     // あとでメインシーンの名前にする！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        //_cancellationTokenSource.Cancel();
    }
}
