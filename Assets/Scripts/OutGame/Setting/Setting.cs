using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Setting : MonoBehaviour
{
    //　音量
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _seSlider;
    [SerializeField] private TextMeshProUGUI _bgmVolumeText;
    [SerializeField] private TextMeshProUGUI _seVolumeText;
    
    // フレームレート
    [SerializeField] private TextMeshProUGUI _framerateText;

    void Start()
    {
        //スライダーを動かした時の処理を登録
        _bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        _seSlider.onValueChanged.AddListener(SetAudioMixerSE);
        _framerateText.text = $"現在のフレームレート：{ES3.Load<int>("Framerate", defaultValue: 60)}fps";  // フレームレートが設定されたことがあれば，それを表示
        _bgmSlider.value = ES3.Load<float>("BGM", defaultValue: 10);
        _seSlider.value = ES3.Load<float>("SE", defaultValue: 10);
        _bgmVolumeText.text = _bgmSlider.value.ToString();
        _seVolumeText.text = _seSlider.value.ToString();
    }

    //BGM
    public void SetAudioMixerBGM(float value)
    {
        // 10段階補正
        value /= 10;
        _bgmVolumeText.text = _bgmSlider.value.ToString();
        ES3.Save<float>("BGM", _bgmSlider.value);
        // -80~-3.0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f,-80f,-3.0f);
        // audioMixerに代入
        _audioMixer.SetFloat("BGM",volume);
        Debug.Log($"BGM:{volume}");
    }

    //SE
    public void SetAudioMixerSE(float value)
    {
        // 10段階補正
        value /= 10;
        _seVolumeText.text = _seSlider.value.ToString();
        ES3.Save<float>("SE", _seSlider.value);
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f,-80f,0f);
        //audioMixerに代入
        _audioMixer.SetFloat("SE",volume);
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
