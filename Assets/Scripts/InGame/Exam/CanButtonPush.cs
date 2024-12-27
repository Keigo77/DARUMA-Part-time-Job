using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanButtonPush : MonoBehaviour
{
    [SerializeField] private SelectExam.ExamType _needExamType;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // 必要な位以上なら，ボタンを押せるようにする
        if ((int)ES3.Load<SelectExam.ExamType>("Role", defaultValue: SelectExam.ExamType.parttime) >=(int)_needExamType)
            this.GetComponent<Button>().interactable = true;
    }

}
