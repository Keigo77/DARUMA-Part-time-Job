using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine.UI;

public class GetRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _rankTexts;
    [SerializeField] private TextMeshProUGUI[] _userNameTexts;
    [SerializeField] private TextMeshProUGUI[] _userScoreTexts;
    [SerializeField] private TextMeshProUGUI _myRankText;
    
    private int[] _getRanks = new int[200];
    private string[] _getNames = new string[200];
    private int[] _getScores = new int[200];
    private int _startRank = 0;
    private int _index = 0;
    private int _rankLength = 0;
    
    // 1位〜3位の画像
    [SerializeField] private Image _goldImage;
    [SerializeField] private Image _silverImage;
    [SerializeField] private Image _bronzeImage;
    
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
        _goldImage.enabled = false;
        _silverImage.enabled = false;
        _bronzeImage.enabled = false;
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    public void GetLeaderboard()
    {
        int index = 0;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "JobRanking"
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                if (index >= 200) return;   // 200人以上のデータは取得しない
                string displayName = item.DisplayName;
                if (displayName == null)
                {
                    displayName = "NoName";
                }
                _getRanks[index] = item.Position;   // 配列に全て入れる
                _getNames[index] = displayName;
                _getScores[index] = item.StatValue;
                index++;
                _rankLength++;
                
                Debug.Log($"{item.Position + 1}位:{displayName} " + $"スコア {item.StatValue}");
            }
            ShowRank(); // 取得したらテキストに反映
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void ShowRank()
    {
        // ランクをテキストに反映
        for (int i = (50 * _startRank); i < (_startRank + 1 ) * 50; _index++)
        {
            if (_getScores[i] != 0)
            {
                if (_getRanks[i] + 1 == 1) _goldImage.enabled = true;
                else if (_getRanks[i] + 1 == 2) _silverImage.enabled = true;
                else if (_getRanks[i] + 1 == 3) _bronzeImage.enabled = true;
                _rankTexts[_index].text = (_getRanks[i] + 1).ToString();
                _userNameTexts[_index].text = _getNames[i];
                _userScoreTexts[_index].text = _getScores[i].ToString();
            }
            else   // データがない時は，空欄を表示
            {
                _rankTexts[_index].text = "";
                _userNameTexts[_index].text = "";
                _userScoreTexts[_index].text = "";
            }
            i++;
        }
        GetLeaderboardAroundPlayer();   // 自分の順位を取得
        _index = 0;
    }
    
    public void GetLeaderboardAroundPlayer() { 
        //GetLeaderboardAroundPlayerRequestのインスタンスを生成
        var request = new GetLeaderboardAroundPlayerRequest{
            StatisticName   = "JobRanking", //ランキング名(統計情報名)
            MaxResultsCount = 0                  //自分を含め前後何件取得するか
        };

        //自分の順位周辺のランキング(リーダーボード)を取得
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, OnGetLeaderboardAroundPlayerFailure);
    }
  
// 自分の順位周辺のランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得に成功しました");

        if (result.Leaderboard != null && result.Leaderboard.Count > 0)
        {
            // 自分の順位情報を取得 (Leaderboard の最初のエントリが自分)
            var entry = result.Leaderboard[0];
            _myRankText.text = $"あなたの順位 : {entry.Position + 1}位/{_rankLength}位"; // Position は 0 始まりのため +1
            Debug.Log($"あなたの順位 : {entry.Position + 1}位/{_rankLength}位");
        }
        else
        {
            Debug.LogWarning("ランキングデータがありません。");
            _myRankText.text = "ランキングデータがありません。";
        }
    }


    //自分の順位周辺のランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error){
        Debug.LogError($"自分の順位周辺のランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    public void ShowNextRank()  // 次の50位を表示
    {
        if (_getScores[(_startRank + 1) * 50] == 0) return; // データがないため無効
        _startRank++;
        ShowRank();
    }
    
    public void ShowBeforeRank()  // 前の50位を表示
    {
        if (_startRank - 1 < 0) return; // データがないため無効
        _startRank--;
        ShowRank();
    }
}
