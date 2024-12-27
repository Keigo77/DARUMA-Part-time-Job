using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectExam : MonoBehaviour
{
    public static ExamType nowExamType = ExamType.sudordinate;    // ここに試験項目を代入
    private ExamType _gettedRole;

    [SerializeField] private GameObject _showDetailPanel;
    [SerializeField] private TextMeshProUGUI _VSwhoText;
    [SerializeField] private TextMeshProUGUI _howDifficultText;
    [SerializeField] private TextMeshProUGUI _winConsitionText;
    
    public enum ExamType    // バイト，部下，上司，部長，社長，ボスの順で昇格していく
    {
        parttime = 0,
        sudordinate = 1,
        boss = 2,
        leader = 3,
        president = 4,
        final = 5
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _gettedRole = ES3.Load<SelectExam.ExamType>("Role", defaultValue: ExamType.parttime);    // 現在の位を取得(初期の位はバイト)
        DeleteDetailPanel();
    }

    public void SelectExamButtonClick(int examType)     // 試験項目を代入してからシーン遷移
    {
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
                _winConsitionText.text = "<登場するだるま>\n通常の2つ目だるま\n<勝利条件>\n50コンボを達成せよ\n(1度50コンボを達成すれば合格)";
                break;
            case SelectExam.ExamType.boss:
                _VSwhoText.text = "VS　上司";
                _howDifficultText.text = "難易度：★★";
                _winConsitionText.text = "<登場するだるま>\n2つ目がないキメラだるま\n<勝利条件>\nスコア60000を達成せよ";
                break;
            case SelectExam.ExamType.leader:
                _VSwhoText.text = "VS　部長";
                _howDifficultText.text = "難易度：★★★";
                _winConsitionText.text = "<登場するだるま>\n3つ目がないキメラだるま\n<勝利条件>\nスコア70000を達成せよ";
                break;
            case SelectExam.ExamType.president:
                _VSwhoText.text = "VS　社長";
                _howDifficultText.text = "難易度：★★★★";
                _winConsitionText.text = "<登場するだるま>\n3全てのだるま\n<勝利条件>\n30コンボを達成せよ\n(1度30コンボを達成すれば合格)";
                break;
            case SelectExam.ExamType.final:
                _VSwhoText.text = "VS　ボス";
                _howDifficultText.text = "難易度：★★★★★";
                _winConsitionText.text = "<登場するだるま>\n全てのだるま\n<勝利条件>\n<color=#F0566E>50秒間</color>で50コンボを達成せよ\n(1度50コンボを達成すれば合格)";
                break;
            default:
                break;
        }
        _showDetailPanel.SetActive(true);
    }

    public void DeleteDetailPanel()
    {
        _showDetailPanel.SetActive(false);
    }
}
