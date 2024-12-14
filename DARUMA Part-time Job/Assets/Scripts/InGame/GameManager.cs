using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _darumaPrefab;
    private GameObject _daruma;     // 現在画面にいるダルマが入る
    private DarumaController DarumaControllerScript;     // 現在画面にいるダルマのスクリプト
    
    // スコア系
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;
    private float _score = 0;
    private int _combo = 0;
    private int _darumaCount = 0;       // 一定数ダルマを作ったならキメラダルマを登場させる
    
    // Start is called before the first frame update
    void Start()
    {
        AppearDaruma();
    }

    // Update is called once per frame


    public void AppearDaruma()     // ダルマを出現させる 
    {
        _daruma = Instantiate(_darumaPrefab, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
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
        Debug.Log("倍率：" + 100 * (_combo % 10 + 1.0f));
    }
}
