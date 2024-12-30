using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public TextMeshProUGUI dotweenText;
    public float dotweenInterval;
    private Tween _doTween;
    private LoadScene LoadSceneSript;

    void Start()
    {
        LoadSceneSript = this.GetComponent<LoadScene>();
        _doTween = dotweenText.DOFade(0.0f, dotweenInterval)   // アルファ値を0にしていく
            .SetLoops(-1, LoopType.Yoyo);    // 行き来を無限に繰り返す
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SESingleton.seInstance.PlayPushDecideButtonSound();
            LoadSceneSript.SceneLoad();
        }
    }

    private void OnDestroy()
    {
        _doTween.Kill();
    }
}
