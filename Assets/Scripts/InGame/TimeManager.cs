using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _timeCircle;
    [SerializeField] private float _time = 30.0f;
    public bool isGameFinish { get; set; } = true;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    
    // Start is called before the first frame update
    async void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _timeText.text = ((int)_time).ToString();
        await UniTask.Delay(TimeSpan.FromSeconds(3.0f), cancellationToken: _cancellationTokenSource.Token);
        isGameFinish = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameFinish) return;
        
        _time -= Time.deltaTime;
        _timeText.text = ((int)_time).ToString();
        _timeCircle.fillAmount = _time / 30.0f;
        
        if (_time <= 0) isGameFinish = true;
    }
}
