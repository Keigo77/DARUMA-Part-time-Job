using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    //　音量
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] private TextMeshProUGUI _bgmVolumeText;
    [SerializeField] private TextMeshProUGUI _seVolumeText;
    
    // フレームレート
    [SerializeField] private TextMeshProUGUI _framerateText;

    void Start()
    {
        //スライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);
        if (ES3.KeyExists("Framerate"))  _framerateText.text = $"現在のフレームレート：{ES3.Load<int>("Framerate")}fps";  // フレームレートが設定されたことがあれば，それを表示
        else _framerateText.text = "現在のフレームレート：60fps"; // 初期フレームレート
    }

    //BGM
    public void SetAudioMixerBGM(float value)
    {
        // 10段階補正
        value /= 10;
        _bgmVolumeText.text = bgmSlider.value.ToString();
        // -80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f,-80f,-3.0f);
        // audioMixerに代入
        audioMixer.SetFloat("BGM",volume);
        Debug.Log($"BGM:{volume}");
    }

    //SE
    public void SetAudioMixerSE(float value)
    {
        // 10段階補正
        value /= 10;
        _seVolumeText.text = _seVolumeText.text = bgmSlider.value.ToString();
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f,-80f,0f);
        //audioMixerに代入
        audioMixer.SetFloat("SE",volume);
        SESingleton.seInstance.PlayPushDecideButtonSound(); // 試しに音を鳴らす
        Debug.Log($"SE:{volume}");
    }

    public void ChangeFrameRate(int frameRate)      // フレームレートの変更
    {
        Application.targetFrameRate = frameRate;
        ES3.Save<int>("Framerate", frameRate);
        _framerateText.text = $"現在のフレームレート：{ES3.Load<int>("Framerate")}fps";
    }
}
