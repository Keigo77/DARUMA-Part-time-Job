using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectExam : MonoBehaviour
{
    public static ExamType nowExamType = ExamType.sudordinate;    // ここに試験項目を代入
    private ExamType gettedRole;
    public enum ExamType    // バイト，部下，上司，部長，社長，ボスの順で昇格していく
    {
        parttime,
        sudordinate,
        boss,
        leader,
        president,
        final
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gettedRole = ES3.Load<SelectExam.ExamType>("Role", defaultValue: ExamType.parttime);    // 現在の位を取得(初期の位はバイト)
    }

    public void SelectExamButtonClick(SelectExam.ExamType examType)     // 試験項目を代入してからシーン遷移
    {
        SelectExam.nowExamType = examType;
    }
}
