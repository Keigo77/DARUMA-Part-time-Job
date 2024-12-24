using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランキングのサンプル
/// </summary>
public class ChangeUserName : MonoBehaviour {
    
    [SerializeField] private InputField _inputField;
    [SerializeField] private GameObject _DeletePanelButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _nameRegisterPanel;
    private bool isSuccessLogin = false;

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
        isSuccessLogin = true;
    }
    private void PlayFabAuthService_OnPlayFabError(PlayFabError error)
    {
        Debug.Log("ログイン失敗");
        isSuccessLogin = false;
    }
    
    void Start()
    {
        _DeletePanelButton.SetActive(false);
        if (ES3.KeyExists("PlayerName"))    // 名前を保存済みなら，設定画面を表示(最初は強制的に名前を登録させる)
        {
            _nameRegisterPanel.SetActive(false);
            _inputField.text = ES3.Load<string>("PlayerName");
        }
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }
    
    public void UpdateUserName()
    {
        if (!isSuccessLogin)
        {
            _text.text = "ネットワークに接続できませんでした．\n設定画面から再度設定してください．";
            return;    // ログインできていないならエラー
        }
        SESingleton.seInstance.PlayPushDecideButtonSound();
        _DeletePanelButton.SetActive(true);
        if (_inputField.text.Length > 10 || _inputField.text.Length <= 0)       // ユーザー名が10文字より長いなら中止
        {
            // エラー表示
            _text.text = "文字数が10文字以上か，入力されていません．\n設定画面から再度設定してください．";
            return;
        }
        
        //ユーザ名を指定して、UpdateUserTitleDisplayNameRequestのインスタンスを生成
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = _inputField.text
        };
        
        //ユーザ名の更新
        Debug.Log($"ユーザ名の更新開始");
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserNameSuccess, OnUpdateUserNameFailure);
    }
  
    //ユーザ名の更新成功
    private void OnUpdateUserNameSuccess(UpdateUserTitleDisplayNameResult result){
        //result.DisplayNameに更新した後のユーザ名が入ってる
        _text.text = $"ユーザ名を\n「{result.DisplayName}」\nで更新ました";
        ES3.Save<string>("PlayerName", _inputField.text);   // プレイヤー名を保存
        Debug.Log($"ユーザ名の更新が成功しました : {result.DisplayName}");
    }

    //ユーザ名の更新失敗
    private void OnUpdateUserNameFailure(PlayFabError error)
    {
        _text.text = "ユーザ名の更新に失敗しました．\n設定画面から再度設定してください．";
        Debug.LogError($"ユーザ名の更新に失敗しました\n{error.GenerateErrorReport()}");
    }

    public void DeleteInputNamePanel()
    {
        _DeletePanelButton.SetActive(false);
        _nameRegisterPanel.SetActive(false);
    }

    public void ShowNameRegisterPanel()
    {
        _nameRegisterPanel.SetActive(true);
    }

}