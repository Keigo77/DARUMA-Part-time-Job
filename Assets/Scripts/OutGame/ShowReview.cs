using UnityEngine;

public class ShowReview : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ES3.Load<int>("ShowReviewCount", defaultValue: 0) <= 2) return;     // 2回，評価ウィンドウを出すまで行う
        
        int rand = Random.Range(0, 5);
        if (rand == 0) {
            UnityEngine.iOS.Device.RequestStoreReview();     // 5回に1回，評価タブを出す
            ES3.Save<int>("ShowReviewCount", (ES3.Load<int>("ShowReviewCount", defaultValue: 0) + 1));
        }
    }
    
}
