using System.Collections;
using System.Collections.Generic;
using PlayFab.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.Audio;


public class BGMandSettingSingleton : MonoBehaviour
{
    public static BGMandSettingSingleton bgmInstance;  // シングルトン
    [SerializeField] private AudioClip _mainSound;
    [SerializeField] private AudioClip _playingSound;
    [SerializeField] private AudioMixer _audioMixer;
    
    private AudioSource _audioSource;
    
    private ReactiveProperty<string> nowSceneName = new ReactiveProperty<string>("MainScene");

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();    // Awakeで読み込まないと間に合わない時があった
        // Don't Destroy Object に登録する
        if (bgmInstance == null)
        {
            bgmInstance = this;
            _audioMixer.SetFloat("BGM", ES3.Load<float>("BGM", defaultValue: -3.0f));
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = ES3.Load<int>("Framerate", defaultValue:60);   // 初期のフレームレートは60
        float value = ES3.Load<float>("BGM", defaultValue: -3.0f) / 10.0f;  // bgmの設定
        _audioMixer.SetFloat("BGM", Mathf.Clamp(Mathf.Log10(value) * 20f,-80f,-3.0f));
        nowSceneName.Subscribe(nowSceneName=> ChangeSound(nowSceneName));
        _audioSource.clip = _mainSound;
        _audioSource.Play();
    }

    void Update()
    {
        nowSceneName.Value = SceneManager.GetActiveScene().name;
    }

    private void ChangeSound(string SceneName)
    {
        if (SceneName == "GameScene")
        {
            if (_audioSource.clip != _playingSound) // 同じBGMでなければ変更
            {
                _audioSource.clip = _playingSound;
                _audioSource.Play();
            }
        }
        else if (SceneName == "ResultScene")
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        } 
        else 
        {
            if (_audioSource.clip != _mainSound) // 同じBGMでなければ変更
            {
                _audioSource.clip = _mainSound;
                _audioSource.Play();
            }
        }
    }
}
