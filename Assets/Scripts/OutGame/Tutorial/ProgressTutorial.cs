using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTutorial : MonoBehaviour
{
    [SerializeField] private TextAsset _textFile;
    [SerializeField] private TextMeshProUGUI _tutorialText;
    [SerializeField] private Image _progressImage;
    private int _textProgress = 0;
    
    // 写真を表示するための変数
    [SerializeField] private Image[] _TutorialMaterialImages;
    [SerializeField] private int[] _whenShowMaterialNumbers;
    private string[] _textLine;
    private int _materialProgress = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        string textData = _textFile.text;
        _textLine = textData.Split('\n');
        
        _tutorialText.text = _textLine[_textProgress];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_textProgress + 1 >= _textLine.Length) return;   // 範囲外アクセスになるなら早期リターン
            _textProgress++;
            _tutorialText.text = _textLine[_textProgress];
            
            if (_textProgress + 1 >= _textLine.Length) _progressImage.enabled = false;

            if (_materialProgress >= _TutorialMaterialImages.Length) return;  // 範囲外アクセスの防止
            if (_textProgress == _whenShowMaterialNumbers[_materialProgress])   // 写真を表示するタイミングなら写真を表示
            {
                _TutorialMaterialImages[Math.Max(0, _materialProgress - 1)].enabled = false;    // 一つ前の写真を非表示
                _TutorialMaterialImages[_materialProgress].enabled = true;
                _materialProgress++;
            }
        }
    }
}
