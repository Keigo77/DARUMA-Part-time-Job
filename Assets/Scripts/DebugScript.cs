using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("だるまの合計:"  + (GameManager._darumaCount + PlayerPrefs.GetInt("DarumaCount")));
        Debug.Log("ハイスコア:"  + PlayerPrefs.GetInt("HighScore"));
        Debug.Log("最新のスコア結果:"  + GameManager.score);
    }
}
