using System;
using System.Collections.Generic;
using System.Threading;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

/// <summary>
/// ランキングのサンプル
/// </summary>
public class ChangeUserName : MonoBehaviour {
    
    [SerializeField] private InputField _inputField;
    [SerializeField] private GameObject _deletePanelButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _nameRegisterPanel;
    private bool isSuccessLogin = false;
    // UniTask
    private CancellationTokenSource _cancellationTokenSource;

    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
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
        _deletePanelButton.SetActive(false); 
        if(!ES3.Load<bool>("IsFinishTutorial", defaultValue: false)) _nameRegisterPanel.SetActive(true);    // 名前を保存済みなら，設定画面を表示(最初は強制的に名前を登録させる)
        else _nameRegisterPanel.SetActive(false);
        _inputField.text = ES3.Load<string>("PlayerName", defaultValue: "");
    }
    
    public async void UpdateUserName()
    {
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
        _text.text = "処理中...";
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: _cancellationTokenSource.Token);
        }
        catch
        {
            Debug.Log("キャンセル");
        }
        
        if (!isSuccessLogin)
        {
            _text.text = "名前の変更に失敗しました．通信環境を整えて，再度お試しください．";
            _deletePanelButton.SetActive(true);
            return;    // ログインできていないならエラー
        }
        SESingleton.seInstance.PlayPushDecideButtonSound();
        if (_inputField.text.Length > 10 || _inputField.text.Length <= 2)       // ユーザー名が10文字より長いなら中止
        {
            // エラー表示
            _text.text = "文字数は3文字以上，10文字以下です．\n入力し直して，再度お試しください．";
            _deletePanelButton.SetActive(true);
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
        _deletePanelButton.SetActive(true);
        ES3.Save<string>("PlayerName", _inputField.text);   // プレイヤー名を保存
        Debug.Log($"ユーザ名の更新が成功しました : {result.DisplayName}");
    }

    //ユーザ名の更新失敗
    private void OnUpdateUserNameFailure(PlayFabError error)
    {
        _text.text = "ユーザ名の更新に失敗しました．\n通信環境を整えて，再度お試しください．";
        Debug.LogError($"ユーザ名の更新に失敗しました\n{error.GenerateErrorReport()}");
    }

    public void DeleteInputNamePanel()
    {
        _text.text = "";
        _deletePanelButton.SetActive(false);
        _nameRegisterPanel.SetActive(false);
    }

    public void ShowNameRegisterPanel()
    {
        _nameRegisterPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }
}