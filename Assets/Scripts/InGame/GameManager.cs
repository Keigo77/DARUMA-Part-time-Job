using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[]  _normalDarumaPrefabs;
    
    [SerializeField] private GameObject[] _c2DarumaPrefabs;     // 2つ目がないキメラだるま
    
    [SerializeField] private GameObject[] _c3DarumaPrefabs;     // 3つ目がないキメラだるま
    
    private GameObject _daruma;     // 現在画面にいるダルマが入る
    private float _yPosition = 0.8f;
    private DarumaController DarumaControllerScript;     // 現在画面にいるダルマのスクリプト
    
    // スコア系
    [SerializeField] private TextMeshProUGUI _comboText;
    public float score { get; set; } = 0.0f;
    public int combo { get; set; } = 0;
    private int _darumaCount = 0;       // 一定数ダルマを作ったならキメラダルマを登場させる，だるまの色を変える
    
    // UniTask
    private CancellationTokenSource _cancellationTokenSource;
    
    // ゲームが終わったかどうか
    [SerializeField] private TimeManager TimeManagerScript;
    
    // Start is called before the first frame update
    async void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            await UniTask.WaitUntil(() => TimeManagerScript.isGameStart, PlayerLoopTiming.Update, _cancellationTokenSource.Token);
        }
        catch
        {
            Debug.Log("キャンセル");
        }
        AppearDaruma();
    }

    public void AppearDaruma()     // ダルマを出現させる 
    {
        if (TimeManagerScript.isGameFinish || !TimeManagerScript.isGameStart) return;     // ゲーム中でないなら早期リターン
        
        int _kind = _darumaCount % 3;
        if (_darumaCount >= 20) 
            _daruma = Instantiate(_c3DarumaPrefabs[_kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else if (_darumaCount >= 10)
            _daruma = Instantiate(_c2DarumaPrefabs[_kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else 
            _daruma = Instantiate(_normalDarumaPrefabs[_kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        
        DarumaControllerScript = _daruma.GetComponent<DarumaController>();
        DarumaControllerScript.SetGameManager(this.gameObject);    // ダルマ側にこのスクリプト(GameManager)を渡す
        _darumaCount++;
    }
    
    public void ButtonClick(int directionNUm)
    {
        if (TimeManagerScript.isGameFinish || !TimeManagerScript.isGameStart) return;
        DarumaControllerScript.ButtonClick(directionNUm);
    }

    public void ResetCombo()
    {
        combo = 0;
        _comboText.text = combo.ToString() + "\nCombo!";
    }

    public void AddScoreCombo(float score)       // ダルマ側で実行される
    {
        this.score += score;
        combo++;
        _comboText.text = combo.ToString() + "\nCombo!";
        Debug.Log("倍率：" + (combo * 0.01f + 1.0f));
    }

    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }
}
