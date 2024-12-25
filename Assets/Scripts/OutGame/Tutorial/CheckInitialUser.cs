using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInitialUser : MonoBehaviour
{
    public void CheckInitial()      // チュートリアルが終わっていないなら，名前登録後強制的にチュートリアルシーンに飛ばす
    {
        if (!ES3.Load<bool>("IsFinishTutorial"))
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }
}
