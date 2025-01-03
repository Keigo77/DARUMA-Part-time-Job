using System;
using UnityEngine;

public class ShowAd : MonoBehaviour
{
    private AdmobManager AdmobManagerScript;

    private void Awake()
    {
        AdmobManagerScript = this.GetComponent<AdmobManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdmobManagerScript.LoadAd();
    }

    private void OnDestroy()
    {
        AdmobManagerScript.DestroyAd();
    }
}
