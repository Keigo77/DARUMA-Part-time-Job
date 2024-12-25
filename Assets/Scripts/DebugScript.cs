using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"だるまの合計:{ES3.Load<int>("DarumaCount")}");
        Debug.Log($"ハイスコア:{ES3.Load<int>("HighScore")}");
        Debug.Log("最新のスコア結果:"  + GameManager.score);
    }
}
