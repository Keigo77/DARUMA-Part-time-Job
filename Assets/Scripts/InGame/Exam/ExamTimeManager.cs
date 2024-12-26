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

public class ExamTimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _timeCircle;
    [SerializeField] private float _time = 31.0f;
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
    [SerializeField] private GameObject _passPanel;
    [SerializeField] private GameObject _notPassPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        isHighScore = false;
        if (SelectExam.nowExamType == SelectExam.ExamType.final) _time = 51.0f;     // 最後の試験時間は，50秒
        _timeText.text = (_time - 1.0f).ToString();           // 時間をテキストに反映
        _countDownText.text = ((int)_countDownTime).ToString();
        _passPanel.SetActive(false);
        _notPassPanel.SetActive(false);
    }

    // Update is called once per frame
    async void Update()
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
        _timeCircle.fillAmount = (_time - 1.0f) / (_time - 1.0f);

        if (_time < 1.0f)
        {
            isGameFinish = true;
            SESingleton.seInstance.PlaySE(_finishSE);
            PanelControllerScript.ShowFinishPanel();
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: _cancellationTokenSource.Token);
            }
            catch
            {
                Debug.Log("キャンセル");
            }
            PanelControllerScript.DeleteFinishPanel();
            if (ExamManagerScript.CheckExam()) _passPanel.SetActive(true);
            else _notPassPanel.SetActive(true);
            
        }
    }
}
