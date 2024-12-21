using System.Collections;
using System.Collections.Generic;
using PlayFab.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;


public class BGMandSettingSingleton : MonoBehaviour
{
    public static BGMandSettingSingleton bgmInstance;  // シングルトン
    [SerializeField] private AudioClip _mainSound;
    [SerializeField] private AudioClip _playingSound;
    
    private AudioSource _audioSource;
    public int framelate { get; set; } = 60;
    private ReactiveProperty<string> nowSceneName = new ReactiveProperty<string>("MainScene");

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();    // Awakeで読み込まないと間に合わない時があった
        // Don't Destroy Object に登録する
        if (bgmInstance == null)
        {
            bgmInstance = this;
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
        nowSceneName.Subscribe(nowSceneName=> ChangeSound(nowSceneName));
        _audioSource.clip = _mainSound;
        _audioSource.Play();
        Application.targetFrameRate = framelate;
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
