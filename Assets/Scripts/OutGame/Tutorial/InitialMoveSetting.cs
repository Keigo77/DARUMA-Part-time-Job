using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMoveSetting : MonoBehaviour
{
    // 1番初めは設定シーンに飛ぶ->名前登録->チュートリアルシーン
    void Start()
    {
        // チュートリアルが終わっていないなら，チュートリアル開始
        if (!ES3.Load<bool>("IsFinishTutorial", defaultValue: false))
        {
            SceneManager.LoadScene("SettingScene");
        }
    }
}
