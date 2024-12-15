using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _redDarumaPrefab;
    [SerializeField] private GameObject _blueDarumaPrefab;
    [SerializeField] private GameObject _blackDarumaPrefab;
    
    [SerializeField] private GameObject _c2RedDarumaPrefab;     // 2つ目がないキメラだるま
    [SerializeField] private GameObject _c2BlueDarumaPrefab;
    [SerializeField] private GameObject _c2BlackDarumaPrefab;
    
    [SerializeField] private GameObject _c3RedDarumaPrefab;     // 3つ目がないキメラだるま
    [SerializeField] private GameObject _c3BlueDarumaPrefab;
    [SerializeField] private GameObject _c3BlackDarumaPrefab;
    
    private GameObject _daruma;     // 現在画面にいるダルマが入る
    private float _yPosition = 0.8f;
    private DarumaController DarumaControllerScript;     // 現在画面にいるダルマのスクリプト
    
    // スコア系
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;
    private float _score = 0;
    public int combo { get; set; } = 0;
    private int _darumaCount = 0;       // 一定数ダルマを作ったならキメラダルマを登場させる，だるまの色を変える
    
    // ゲームが終わったかどうか
    [SerializeField] private TimeManager TimeManagerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        AppearDaruma();
    }

    // Update is called once per frame


    public void AppearDaruma()     // ダルマを出現させる 
    {
        if (TimeManagerScript.isGameFinish) return;     // ゲーム中でないなら早期リターン
        
        switch (_darumaCount % 3)
        {
            case 0:
                if (_darumaCount >= 20) _daruma = Instantiate(_c3RedDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else if (_darumaCount >= 10) _daruma = Instantiate(_c2RedDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else _daruma = Instantiate(_redDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                break;
            case 1:
                if (_darumaCount >= 20) _daruma = Instantiate(_c3BlueDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else if (_darumaCount >= 10) _daruma = Instantiate(_c2BlueDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else _daruma = Instantiate(_blueDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                break;
            case 2:
                if (_darumaCount >= 20) _daruma = Instantiate(_c3BlackDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else if (_darumaCount >= 10) _daruma = Instantiate(_c2BlackDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                else _daruma = Instantiate(_blackDarumaPrefab, new Vector3(0.0f, _yPosition, 0.0f), Quaternion.identity);
                break;
            default:
                Debug.Log("darumaCountのエラー");
                break;
        }

        DarumaControllerScript = _daruma.GetComponent<DarumaController>();
        DarumaControllerScript.SetGameManager(this.gameObject);    // ダルマ側にこのスクリプト(GameManager)を渡す
        _darumaCount++;
    }
    
    public void ButtonClick(int directionNUm)
    {
        DarumaControllerScript.ButtonClick(directionNUm);
    }

    public void ResetCombo()
    {
        combo = 0;
        _comboText.text = combo.ToString();
    }

    public void AddScoreCombo(float score)       // ダルマ側で実行される
    {
        _score += score;
        combo++;
        _scoreText.text = _score.ToString();
        _comboText.text = combo.ToString();
        Debug.Log("倍率：" + (combo * 0.01f + 1.0f));
    }
}
