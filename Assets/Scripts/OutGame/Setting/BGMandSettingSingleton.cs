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
    [SerializeField] private AudioClip _selectExamSound;
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
        if (SceneName == "GameScene")   AudioSet(_playingSound);
        else if (SceneName == "SelectExamScene") AudioSet(_selectExamSound);
        else if (SceneName == "ResultScene" || SceneName == "FinalExamScene")   // 音を止める
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        } 
        else if (SceneName == "ExamScene")
        {
            // ExamManagerで再生させる
        }
        else AudioSet(_mainSound);
    }

    public void AudioSet(AudioClip nextClip)
    {
        if (_audioSource.clip != nextClip)
        {
            _audioSource.clip = nextClip;
            _audioSource.Play();
        }
    }
}
