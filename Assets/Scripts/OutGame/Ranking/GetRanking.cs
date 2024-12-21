using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class GetRanking : MonoBehaviour
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
        GetLeaderboard();
    }
    private void PlayFabAuthService_OnPlayFabError(PlayFabError error)
    {
        Debug.Log("ログイン失敗");
    }
    void Start()
    {
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "JobRanking"
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                string displayName = item.DisplayName;
                if (displayName == null)
                {
                    displayName = "NoName";
                }
                Debug.Log($"{item.Position + 1}位:{displayName} " + $"スコア {item.StatValue}");
            }
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}