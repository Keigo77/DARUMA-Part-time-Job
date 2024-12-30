using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LayerLab.GUIScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ExamTimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _timeCircle;
    [SerializeField] private float _initialTime = 30.0f;
    [SerializeField] private float _time = 31.0f;
    [SerializeField] private Button _stopButton;
    // カウントダウン
    private float _countDownTime = 4.02f;
    [SerializeField] private TextMeshProUGUI _countDownText; 
    [SerializeField] private PanelController PanelControllerScript;
    public bool isGameStart { get; set; } = false;
    public bool isGameFinish { get; set; } = false;
    public static bool isHighScore = false;
    // 音楽
    [SerializeField] private AudioClip _startSE;
    [SerializeField] private AudioClip _finishSE;
    // UniTask
    private CancellationTokenSource _cancellationTokenSource;
    //試験
    [SerializeField] private ExamManager ExamManagerScript;
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private GameObject _passPanel;
    [SerializeField] private GameObject _notPassPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        isHighScore = false;
        if (SelectExam.nowExamType == SelectExam.ExamType.final) _initialTime = 50.0f;     // 最後の試験時間は，50秒
        _time = _initialTime;
        _timeText.text =_initialTime.ToString();           // 時間をテキストに反映
        _countDownText.text = ((int)_countDownTime).ToString();
        _passPanel.SetActive(false);
        _notPassPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_countDownTime > 1)
        {
            _countDownTime -= Time.deltaTime;
            _countDownText.text = ((int)_countDownTime).ToString();
        } else if (!isGameStart && _countDownTime <= 1)
        {
            PanelControllerScript.DeleteStartPanel();
            SESingleton.seInstance.PlaySE(_startSE);
            isGameStart = true;
        }

        if (isGameFinish || !isGameStart) return;

        _time -= Time.deltaTime;
        _timeText.text = ((int)_time).ToString();
        _timeCircle.fillAmount = ((_time - 1.0f) / _initialTime);
        Debug.Log($"{_time}，{_initialTime}，{_timeCircle.fillAmount}");

        if (_time < 1.0f)
        {
            isGameFinish = true;
            _stopButton.interactable = false;
            SESingleton.seInstance.PlaySE(_finishSE);
            ShowFinishPanel();
        }
    }

    async void ShowFinishPanel()
    {
        _finishPanel.SetActive(true);
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: _cancellationTokenSource.Token);
        }
        catch
        {
            Debug.Log("キャンセル");
        }
        _finishPanel.SetActive(false);
        if (ExamManagerScript.CheckExam()) _passPanel.SetActive(true);
        else _notPassPanel.SetActive(true);
    }
}
