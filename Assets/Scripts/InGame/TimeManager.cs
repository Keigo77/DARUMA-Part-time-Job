using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LayerLab.GUIScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _timeCircle;
    [SerializeField] private float _time = 31.0f;
    
    // カウントダウン
    private float _countDownTime = 4.0f;
    [SerializeField] private TextMeshProUGUI _countDownText; 
    [SerializeField] private PanelController PanelControllerScript;
    
    public bool isGameStart { get; set; } = false;
    public bool isGameFinish { get; set; } = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeText.text = (_time - 1.0f).ToString();           // 時間をテキストに反映
        _countDownText.text = ((int)_countDownTime).ToString();
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
            isGameStart = true;
        }

        if (isGameFinish || !isGameStart) return;

        _time -= Time.deltaTime;
        _timeText.text = ((int)_time).ToString();
        _timeCircle.fillAmount = (_time - 1.0f) / 30.0f;

        if (_time < 1.0f)
        {
            isGameFinish = true;
            PanelControllerScript.ShowFinishPanel();
        }
    }
}
