using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;


public class ExamManager : MonoBehaviour
{
    private SelectExam.ExamType examType;
    [SerializeField] private GameObject[]  _normalDarumaPrefabs;
    
    [SerializeField] private GameObject[] _c2DarumaPrefabs;     // 2つ目がないキメラだるま
    
    [SerializeField] private GameObject[] _c3DarumaPrefabs;     // 3つ目がないキメラだるま
    
    private GameObject _daruma;     // 現在画面にいるダルマが入る
    private float _yPosition = 0.8f;
    private ExamDarumaController DarumaControllerScript;     // 現在画面にいるダルマのスクリプト
    
    // スコア系
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _text;
    private float _score = 0.0f;
    public int combo { get; set; } = 0;
    private int _darumaCount = 0;       // だるまの色を変える
    
    // UniTask
    private CancellationTokenSource _cancellationTokenSource;
    
    // 音楽BGM
    [SerializeField] private AudioClip _examSound;

    [SerializeField] private AudioClip _finalSound;
    
    // ゲームが終わったかどうか
    [SerializeField] private ExamTimeManager ExamTimeManagerScript;
    private int _missCount = 0;     // ミスをした数
    // 試験
    private int _maxCombo = 0;
    private SelectExam.ExamType _nowExamType;
    [SerializeField] private TextMeshProUGUI _passScoreText;
    [SerializeField] private TextMeshProUGUI _passMaxComboText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxComboText;
    
    // Start is called before the first frame update
    async void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        // BGMを流す
        _nowExamType = SelectExam.nowExamType;
        if (_nowExamType == SelectExam.ExamType.final)  BGMandSettingSingleton.bgmInstance.AudioSet(_finalSound);
        else  BGMandSettingSingleton.bgmInstance.AudioSet(_examSound);
        
        try
        {
            await UniTask.WaitUntil(() => ExamTimeManagerScript.isGameStart, PlayerLoopTiming.Update, _cancellationTokenSource.Token);
        }
        catch
        {
            Debug.Log("キャンセル");
        }
        AppearDaruma();
    }

      //-------------------PC対応のコード-----------------------
      /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))    ButtonClick(0);
        if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))    ButtonClick(1);
        if (Input.GetKeyDown(KeyCode.UpArrow)|| (Input.GetKeyDown(KeyCode.W)))    ButtonClick(2);
        if (Input.GetKeyDown(KeyCode.DownArrow)|| (Input.GetKeyDown(KeyCode.S)))    ButtonClick(3);
    }
    */

    public void AppearDaruma()     // ダルマを出現させる 
    {
        if (ExamTimeManagerScript.isGameFinish || !ExamTimeManagerScript.isGameStart) return;     // ゲーム中でないなら早期リターン
        
        int kind = _darumaCount % 3;
        int randomDaruma = Random.Range(0, 3);
        
        // 試験ごとに出現するだるまを限定する
        if  (_nowExamType == SelectExam.ExamType.leader || 
             ((_nowExamType == SelectExam.ExamType.final || _nowExamType == SelectExam.ExamType.president) && _score >= 52000))   // 部長試験か，社長，ボス試験の終盤はキメラだるま
            _daruma = Instantiate(_c3DarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else if  (_nowExamType == SelectExam.ExamType.boss || 
            ((_nowExamType == SelectExam.ExamType.final || _nowExamType == SelectExam.ExamType.president) && _score >= 29000))   // 上司試験か，社長，ボス試験の中盤は2つ目キメラだるま
            _daruma = Instantiate(_c2DarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        else if (_nowExamType == SelectExam.ExamType.sudordinate || 
            ((_nowExamType == SelectExam.ExamType.final || _nowExamType == SelectExam.ExamType.president)))   // 部下試験か，社長，ボス試験で序盤はノーマルだるま
            _daruma = Instantiate(_normalDarumaPrefabs[kind], new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
        
        DarumaControllerScript = _daruma.GetComponent<ExamDarumaController>();
        DarumaControllerScript.SetGameManager(this.gameObject);    // ダルマ側にこのスクリプト(GameManager)を渡す
    }
    
    public void ButtonClick(int directionNUm)
    {
        if (ExamTimeManagerScript.isGameFinish || !ExamTimeManagerScript.isGameStart) return;
        DarumaControllerScript.ButtonClick(directionNUm);
    }

    public void ResetCombo()
    {
        _maxCombo = Math.Max(_maxCombo, combo);   // 最大コンボ数を保存
        combo = 0;
        _comboText.text = "";
        _text.text = "";
        _score -= 1500;  // 連打防止のため，ミスしたら原点
        _missCount++;
        if (_missCount >= 18)   ExamTimeManagerScript.ExamFinish();
    }

    public void AddScoreCombo(float score)       // ダルマ側で実行される
    {
        _darumaCount++; // 完成させただるまをカウント
        _score += score; // スコア加点，コンボも増加
        combo++;
        _comboText.text = combo.ToString();
        _text.text = "こんぼ！";
        _missCount = 0;
    }

    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }

    public bool CheckExam()    // 各試験項目ごとの合格基準を満たしているのか確認，合格ならそれを保存し，合格パネルを表示
    {
        _maxCombo = Math.Max(_maxCombo, combo);     // 最大コンボを代入
        _passScoreText.text = $"スコア：{((int)_score)}";
        _passMaxComboText.text = $"最大コンボ数：{_maxCombo}";
        switch (SelectExam.nowExamType)
        {
            case SelectExam.ExamType.sudordinate when _maxCombo >= 25:
                ES3.Save<SelectExam.ExamType>("Role", SelectExam.ExamType.sudordinate);
                return true;
            case SelectExam.ExamType.boss when _score >= 50000:
                ES3.Save<SelectExam.ExamType>("Role", SelectExam.ExamType.boss);
                return true;
            case SelectExam.ExamType.leader when _score >= 65000:
                ES3.Save<SelectExam.ExamType>("Role", SelectExam.ExamType.leader);
                return true;
            case SelectExam.ExamType.president when _maxCombo >= 45:
                ES3.Save<SelectExam.ExamType>("Role", SelectExam.ExamType.president);
                return true;
            case SelectExam.ExamType.final when _maxCombo >= 75:
                ES3.Save<SelectExam.ExamType>("Role", SelectExam.ExamType.final);
                return true;
            default:
                _scoreText.text = $"スコア：{((int)_score)}";
                _maxComboText.text = $"最大コンボ数：{_maxCombo}";
                return false;
        }
    }
}
