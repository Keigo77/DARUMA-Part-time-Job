using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;


public class SendScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _errorText;
    
    private void OnEnable()
    {
        PlayFabAuthService.OnLoginSuccess += PlayFabAuthService_OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += PlayFabAuthService_OnPlayFabError;
    }

    private void OnDisable()
    {
        PlayFabAuthService.OnLoginSuccess -= PlayFabAuthService_OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError -= PlayFabAuthService_OnPlayFabError;
    }

    private void PlayFabAuthService_OnLoginSuccess(LoginResult success)
    {
        Debug.Log("ログイン成功");
        SubmitScore();
        SubmitDarumaCount();
    }
    private void PlayFabAuthService_OnPlayFabError(PlayFabError error)
    {
        Debug.Log("ログイン失敗");
        _errorText.text = "ハイスコアの送信ができませんでした．\n次回プレイ時に再度送信を行います．";
        Debug.Log(error.ToString());    // 原因など
    }

    // ------------------------------------------------------------------------------ログイン処理後にしたい処理↓
    
    void Start()
    {
        ES3.Save<int>("DarumaCount", (GameManager._darumaCount + ES3.Load<int>("DarumaCount", defaultValue: 0)));  // ダルマを完成させた数を合計して保存
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }
    
    // スコアの送信
    public void SubmitScore()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "JobRanking",
                    Value = ES3.Load<int>("HighScore", defaultValue: 0)
                }
            }
        }, result =>
        {
            if (GameManager.isEyeEnd) _errorText.text = "会社が墨だらけになったため\n本日の業務は中止です";
            Debug.Log($"スコア {ES3.Load<int>("HighScore", defaultValue:0)} 送信完了！");
        }, error =>
        {
            if (GameManager.isEyeEnd) _errorText.text = "会社が墨だらけになったため\n本日の業務は中止です";
            else _errorText.text = "ハイスコアの送信ができませんでした．\n次回プレイ時に再度送信を行います";
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    public void SubmitDarumaCount()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "SumDarumaCount",
                    Value = ES3.Load<int>("DarumaCount", defaultValue: 0)
                }
            }
        }, result =>
        {
            Debug.Log($"だるまの合計 {ES3.Load<int>("DarumaCount", defaultValue: 0)} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}