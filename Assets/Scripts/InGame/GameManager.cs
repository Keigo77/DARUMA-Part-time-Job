using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _redDarumaPrefab;
    [SerializeField] private GameObject _blueDarumaPrefab;
    [SerializeField] private GameObject _blackDarumaPrefab;
    private GameObject _daruma;     // 現在画面にいるダルマが入る
    private DarumaController DarumaControllerScript;     // 現在画面にいるダルマのスクリプト
    
    // スコア系
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;
    private float _score = 0;
    private int _combo = 0;
    private int _darumaCount = 0;       // 一定数ダルマを作ったならキメラダルマを登場させる，だるまの色を変える
    
    // Start is called before the first frame update
    void Start()
    {
        AppearDaruma();
    }

    // Update is called once per frame


    public void AppearDaruma()     // ダルマを出現させる 
    {
        switch (_darumaCount % 3)
        {
            case 0:
                _daruma = Instantiate(_redDarumaPrefab, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
                break;
            case 1:
                _daruma = Instantiate(_blueDarumaPrefab, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
                break;
            case 2:
                _daruma = Instantiate(_blackDarumaPrefab, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
                break;
            default:
                Debug.Log("darumaCountのエラー");
                break;
        }

        DarumaControllerScript = _daruma.GetComponent<DarumaController>();
        DarumaControllerScript.SetGameManager(this.gameObject);    // ダルマ側にこのスクリプト(GameManager)を渡す
        _darumaCount++;
    }

    public void RightButtonClick()
    {
        DarumaControllerScript.RightButtonClick();
    }

    public void LeftButtonClick()
    {
        DarumaControllerScript.LeftButtonClick();
    }

    public void ResetCombo()
    {
        _combo = 0;
        _comboText.text = _combo.ToString();
    }

    public int GetCombo()
    {
        return _combo;
    }

    public void AddScoreCombo(float score)       // ダルマ側で実行される
    {
        _score += score;
        _combo++;
        _scoreText.text = _score.ToString();
        _comboText.text = _combo.ToString();
        Debug.Log("倍率：" + (_combo * 0.01f + 1.0f));
    }

    public int GetDarumaCount()
    {
        return _darumaCount;
    }
}

// 1->1.01  2->1.02  10 -> 1.1
