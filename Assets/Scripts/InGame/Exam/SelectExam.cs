using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectExam : MonoBehaviour
{
    public static ExamType nowExamType = ExamType.sudordinate;    // ここに試験項目を代入
    private ExamType _gettedRole;

    [SerializeField] private GameObject _selectPanel;
    [SerializeField] private GameObject _showDetailPanel;
    [SerializeField] private TextMeshProUGUI _nowRoleText;
    [SerializeField] private TextMeshProUGUI _VSwhoText;
    [SerializeField] private TextMeshProUGUI _howDifficultText;
    [SerializeField] private TextMeshProUGUI _winConsitionText;
    [SerializeField] private Image _lockImage;  // 最終試験へのボタンの鍵マーク
    
    public enum ExamType    // バイト，部下，上司，部長，社長，ボスの順で昇格していく
    {
        parttime = 0,
        sudordinate = 1,
        boss = 2,
        leader = 3,
        president = 4,
        final = 5
    }

    private void Awake()
    {
        _gettedRole = ES3.Load<SelectExam.ExamType>("Role", defaultValue: ExamType.parttime);    // 現在の位を取得(初期の位はバイト)
        if ((int)_gettedRole >= 4)  _lockImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

        ShowNowRole();
        _showDetailPanel.SetActive(false);
    }

    public void SelectExamButtonClick(int examType)     // 試験項目を代入してからシーン遷移
    {
        SESingleton.seInstance.PlayPushDecideButtonSound();
        SelectExam.nowExamType = (ExamType)examType;
        ShowDetailPanel();
    }

    private void ShowDetailPanel()
    {
        switch (SelectExam.nowExamType)
        {
            case SelectExam.ExamType.sudordinate:
                _VSwhoText.text = "VS　部下";
                _howDifficultText.text = "難易度：★";
                _winConsitionText.text = "<登場するだるま>\n通常の2つ目だるま\n<勝利条件>\n25コンボを達成せよ\n(1度25コンボを達成すれば合格)";
                break;
            case SelectExam.ExamType.boss:
                _VSwhoText.text = "VS　上司";
                _howDifficultText.text = "難易度：★★";
                _winConsitionText.text = "<登場するだるま>\n2つ目がないキメラだるま\n<勝利条件>\nスコア50000を達成せよ";
                break;
            case SelectExam.ExamType.leader:
                _VSwhoText.text = "VS　部長";
                _howDifficultText.text = "難易度：★★★";
                _winConsitionText.text = "<登場するだるま>\n3つ目がないキメラだるま\n<勝利条件>\nスコア65000を達成せよ";
                break;
            case SelectExam.ExamType.president:
                _VSwhoText.text = "VS　社長";
                _howDifficultText.text = "難易度：★★★★";
                _winConsitionText.text = "<登場するだるま>\n全てのだるま\n<勝利条件>\n45コンボを達成せよ\n(1度45コンボを達成すれば合格)";
                break;
            case SelectExam.ExamType.final:
                _VSwhoText.text = "VS　ボス";
                _howDifficultText.text = "難易度：★★★★★";
                _winConsitionText.text = "<登場するだるま>\n全てのだるま\n<勝利条件>\n<color=#F0566E>50秒間</color>で75コンボを達成せよ\n(1度75コンボを達成すれば合格)";
                break;
            default:
                break;
        }
        _showDetailPanel.SetActive(true);
        _selectPanel.SetActive(false);
    }

    private void ShowNowRole()
    {
        _nowRoleText.text = "現在の役職：";
        Debug.Log((int)_gettedRole);
        switch (_gettedRole)
        {
            case SelectExam.ExamType.sudordinate:
                _nowRoleText.text += "部下";
                break;
            case SelectExam.ExamType.boss:
                _nowRoleText.text += "上司";
                break;
            case SelectExam.ExamType.leader:
                _nowRoleText.text += "部長";
                break;
            case SelectExam.ExamType.president:
                _nowRoleText.text += "社長";
                break;
            case SelectExam.ExamType.final:
                _nowRoleText.text += "ボス";
                break;
            default:
                _nowRoleText.text += "バイト";
                break;
        }
    }

    public void DeleteDetailPanel()
    {
        SESingleton.seInstance.PlayPushCancellButtonSound();
        _showDetailPanel.SetActive(false);
        _selectPanel.SetActive(true);
    }
}
