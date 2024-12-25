using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class SendScore : MonoBehaviour
{
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
        Debug.Log(error.ToString());    // 原因など
    }

    // ------------------------------------------------------------------------------ログイン処理後にしたい処理↓
    
    void Start()
    {
        PlayerPrefs.SetInt("DarumaCount", (GameManager._darumaCount + PlayerPrefs.GetInt("DarumaCount")));  // ダルマを完成させた数を合計して保存
        PlayerPrefs.SetInt("HighScore", (int)GameManager.score);
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
                    Value = PlayerPrefs.GetInt("HighScore")
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {PlayerPrefs.GetInt("HighScore")} 送信完了！");
        }, error =>
        {
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
                    Value = PlayerPrefs.GetInt("DarumaCount")
                }
            }
        }, result =>
        {
            Debug.Log($"だるまの合計 {PlayerPrefs.GetInt("DarumaCount")} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}