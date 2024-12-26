using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class GetSumDarumaCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _myCountText;
    [SerializeField] private TextMeshProUGUI _sumCountText;
    private int _worldDarumaCount = 0;
    
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private GameObject _loadingDaruma;
    
    
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
        _loadingDaruma.SetActive(false);
        _loadingText.text = "読み込みに失敗しました．\nインターネット接続などを確認してください．";
    }
    void Start()
    {
        _myCountText.text = $"あなたが完成させただるまの数：{ES3.Load<int>("DarumaCount", defaultValue: 0)}個";
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "SumDarumaCount",
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                _worldDarumaCount += item.StatValue;
                Debug.Log($"{item.Position + 1}位:" + $"スコア {item.StatValue}");
            }
             // 取得したらテキストに反映
             _sumCountText.text = $"{_worldDarumaCount}個";
             _loadingPanel.SetActive(false);
             
             
             
        }, error =>
        {
            _loadingDaruma.SetActive(false);
            _loadingText.text = "読み込みに失敗しました．\nインターネット接続などを確認してください．";
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
