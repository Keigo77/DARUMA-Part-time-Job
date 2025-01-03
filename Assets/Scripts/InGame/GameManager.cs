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
    [SerializeField] private TextMeshProUGUI _text;
    public static float score = 0.0f;
    public int combo { get; set; } = 0;
    public static int _darumaCount = 0;       // 一定数ダルマを作ったならキメラダルマを登場させる，だるまの色を変える
    
    // UniTask
    private CancellationTokenSource _cancellationTokenSource;
    
    // ゲームが終わったかどうか
    [SerializeField] private TimeManager TimeManagerScript;
    
    // Start is called before the first frame update
    async void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        score = 0.0f;   // ここで毎回スコアをリセット
        _darumaCount = 0;       // だるまの合計を取得
        
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

      //-------------------PC対応のコード-----------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))    ButtonClick(0);
        if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))    ButtonClick(1);
        if (Input.GetKeyDown(KeyCode.UpArrow)|| (Input.GetKeyDown(KeyCode.W)))    ButtonClick(2);
        if (Input.GetKeyDown(KeyCode.DownArrow)|| (Input.GetKeyDown(KeyCode.S)))    ButtonClick(3);
    }
    

    public void AppearDaruma()     // ダルマを出現させる 
    {
        if (TimeManagerScript.isGameFinish || !TimeManagerScript.isGameStart) return;     // ゲーム中でないなら早期リターン
        
        int kind = _darumaCount % 3;
        if (score >= 52000) 
            _daruma = Instantiate(_c3DarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else
        if (score >= 29000)
            _daruma = Instantiate(_c2DarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else 
            _daruma = Instantiate(_normalDarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        
        DarumaControllerScript = _daruma.GetComponent<DarumaController>();
        DarumaControllerScript.SetGameManager(this.gameObject);    // ダルマ側にこのスクリプト(GameManager)を渡す
    }
    
    public void ButtonClick(int directionNUm)
    {
        if (TimeManagerScript.isGameFinish || !TimeManagerScript.isGameStart) return;
        DarumaControllerScript.ButtonClick(directionNUm);
    }

    public void ResetCombo()
    {
        combo = 0;
        _comboText.text = "";
        _text.text = "";
        GameManager.score -= 1500;  // 連打防止のため，ミスしたら原点
    }

    public void AddScoreCombo(float score)       // ダルマ側で実行される
    {
        _darumaCount++; // 完成させただるまをカウント
        GameManager.score += score; // スコア加点，コンボも増加
        combo++;
        _comboText.text = combo.ToString();
        _text.text = "こんぼ！";
        Debug.Log("倍率：" + (combo * 0.1f + 1.0f));
        Debug.Log(GameManager.score);
    }

    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }
}
