using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobInitialize : MonoBehaviour
{
    /// <summary>
    /// GoogleAdmobのSDKの初期化．アプリ起動時に行うと良いらしい(https://developers.google.com/admob/unity/banner?hl=ja)
    /// </summary>
    
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }
}