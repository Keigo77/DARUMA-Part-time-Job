using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SESingleton : MonoBehaviour
{
    public static SESingleton seInstance;  // シングルトン
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _pushDecideButtonSound;
    [SerializeField] private AudioClip _pushCancellButtonSound;
    // SE
    [SerializeField] private AudioClip _writeSE;
    [SerializeField] private AudioClip _completeSE;
    [SerializeField] private AudioClip _missSE;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();    // Awakeで読み込まないと間に合わない時があった
        // Don't Destroy Object に登録する
        if (seInstance == null)
        {
            seInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayPushDecideButtonSound()
    {
        _audioSource.PlayOneShot(_pushDecideButtonSound);
    }
    
    public void PlayPushCancellButtonSound()
    {
        _audioSource.PlayOneShot(_pushCancellButtonSound);
    }

    public void PlaySE(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlayWriteSE()
    {
        _audioSource.PlayOneShot(_writeSE);
    }
    
    public void PlayCompleteSE()
    {
        _audioSource.PlayOneShot(_completeSE);
    }
    
    public void PlayMissSE()
    {
        _audioSource.PlayOneShot(_missSE);
    }
}
