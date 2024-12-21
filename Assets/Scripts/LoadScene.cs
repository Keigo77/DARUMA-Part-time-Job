using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    //FadeCanvas取得
    [SerializeField] private Fade fade;

    [SerializeField] private bool isFadeOut = false;

    //フェード時間取得（秒）
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.2f;


    
    void Start()
    {
        if (isFadeOut) fade.FadeOut(fadeOutTime);
    }
    
    public void SceneLoad()
    {
        fade.FadeIn(fadeTime,() =>
        {
            SceneManager.LoadScene(sceneName);
        });

    }
}
