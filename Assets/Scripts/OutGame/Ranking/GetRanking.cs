using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class GetRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _rankTexts;
    [SerializeField] private TextMeshProUGUI[] _userNameTexts;
    [SerializeField] private TextMeshProUGUI[] _userScoreTexts;
    [SerializeField] private TextMeshProUGUI _myRankText;
    [SerializeField] private TextMeshProUGUI _myHighScoreText;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private GameObject _loadingDaruma;
    
    private int[] _getRanks = new int[200];
    private string[] _getNames = new string[200];
    private int[] _getScores = new int[200];
    private int _startRank = 0;
    private int _rankLength = 0;        // 自分の順位表示に使用
    
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
        _loadingDaruma.SetActive(false);
        _loadingText.text = "読み込みに失敗しました．\nインターネット接続などを確認してください．";
    }
    void Start()
    {
        _goldImage.enabled = false;
        _silverImage.enabled = false;
        _bronzeImage.enabled = false;
        if (!ES3.KeyExists("HighScore")) _myHighScoreText.text = "";
        else _myHighScoreText.text = $"あなたのハイスコア：{ES3.Load<int>("HighScore", defaultValue: 0)}";
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }

    public void GetLeaderboard()
    {
        int index = 0;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "JobRanking",
            StartPosition = _startRank,
            MaxResultsCount = 50
        }, result =>
        {
            foreach (var item in result.Leaderboard)
            {
                Debug.Log(result.Leaderboard.Count);
                if (index >= 200) return;   // 200人以上のデータは取得しない
                string displayName = item.DisplayName;
                if (displayName == null)
                {
                    displayName = "NoName";
                }

                if (item.StatValue == 0) break;     // スコア0のプレイヤーはランキングに含めない
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
            _loadingDaruma.SetActive(false);
            _loadingText.text = "読み込みに失敗しました．\nインターネット接続などを確認してください．";
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void ShowRank()
    {
        int index = 0;
        // ランクをテキストに反映
        for (int i = 0; i < 50; index++)
        {
            if (_getScores[i] != 0)
            {
                if (_getRanks[i] + 1 == 1) _goldImage.enabled = true;
                else if (_getRanks[i] + 1 == 2) _silverImage.enabled = true;
                else if (_getRanks[i] + 1 == 3) _bronzeImage.enabled = true;
                _rankTexts[index].text = (_getRanks[i] + 1).ToString();
                _userNameTexts[index].text = _getNames[i];
                _userScoreTexts[index].text = _getScores[i].ToString();
            }
            else break;
            i++;
        }
        GetLeaderboardAroundPlayer();   // 自分の順位を取得
        index = 0;
    }
    
    public void GetLeaderboardAroundPlayer() {
        if (!ES3.KeyExists("HighScore"))
        {
            _myRankText.text = "あなたのデータはありません";
            _loadingPanel.SetActive(false);
            return;
        }
        //GetLeaderboardAroundPlayerRequestのインスタンスを生成
        var request = new GetLeaderboardAroundPlayerRequest{
            StatisticName   = "JobRanking", //ランキング名(統計情報名)
            MaxResultsCount = 1                  //自分を含め前後何件取得するか
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

            _loadingPanel.SetActive(false); // ここで初めてロードパネルを削除
        }
        else
        {
            Debug.LogWarning("ランキングデータがありません");
            _myRankText.text = "ランキングのデータがありません";
        }
    }


    //自分の順位周辺のランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error){
        Debug.LogError($"自分の順位周辺のランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    
    public void ShowNextRank()  // 次の50位を表示(未使用)
    {
        if (_getScores[(_startRank + 1) * 50] == 0) return; // データがないため無効
        _startRank++;
        GetLeaderboard();
    }
    
    public void ShowBeforeRank()  // 前の50位を表示(未使用)
    {
        if (_startRank - 1 < 0) return; // データがないため無効
        _startRank--;
        GetLeaderboard();
    }
}
