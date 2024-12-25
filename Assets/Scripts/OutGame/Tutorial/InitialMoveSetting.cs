using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMoveSetting : MonoBehaviour
{
    // 1番初めは設定シーンに飛ぶ->名前登録->チュートリアルシーン
    void Start()
    {
        if(!ES3.KeyExists("HighScore")) ES3.Save<int>("HighScore", 0);    // 初期データの用意
        if(!ES3.KeyExists("DarumaCount")) ES3.Save<int>("DarumaCount", 0);
        if(!ES3.KeyExists("IsFinishTutorial")) ES3.Save<bool>("IsFinishTutorial", false);
        //if(!ES3.Load<bool>("IsFinishTutorial")) SceneManager.LoadScene("SettingScene");
        // --------------------↑後でコメント外す！！---------------
    }
}
