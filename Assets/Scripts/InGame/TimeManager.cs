using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _timeCircle;
    private float _time = 30.0f;
    
    // カウントダウン
    private float _countDownTime = 4.0f;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private TextMeshProUGUI _countDownText; 
    public bool isGameFinish { get; set; } = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeText.text = ((int)_time).ToString();           // 時間をテキストに反映
        _countDownText.text = ((int)_countDownTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameFinish && _countDownTime > 1)
        {
            _countDownTime -= Time.deltaTime;
            _countDownText.text = ((int)_countDownTime).ToString();
        } else if (_countDownTime <= 1)
        {
            _startPanel.SetActive(false);
            isGameFinish = false;
        }
        
        _time -= Time.deltaTime;
        _timeText.text = ((int)_time).ToString();
        _timeCircle.fillAmount = _time / 30.0f;
        
        if (_time <= 0) isGameFinish = true;
    }
}
