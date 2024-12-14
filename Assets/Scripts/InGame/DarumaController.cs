using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DarumaController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _leftEyeSprite;      // 目の削除/有効
    [SerializeField] private SpriteRenderer _rightEyeSprite;
    [SerializeField] private Transform _leftEyeTransform;        // 目の拡大
    [SerializeField] private Transform _rightEyeTransform;

    private int randomNum = 0;
    GameManager GameManagerScript = null;
    
    // Start is called before the first frame update
    void Start()
    {
        randomNum = Random.Range(0, 2);     // 0なら左がない，1なら右がない
        DeleteEye(randomNum);
    }

    public void DeleteEye(int randomNum)    // ランダムで選ばれた片方の目を削除する
    {
        switch (randomNum)
        {
            case 0:
                _leftEyeSprite.enabled = false;
                break;
            case 1:
                _rightEyeSprite.enabled = false;
                break;
            default:
                Debug.Log("予期せぬ値です：" + randomNum);
                break;
        }
    }

    public void RightButtonClick()
    {
        if (!_rightEyeSprite.enabled)
        {
            GameManagerScript.AddScoreCombo(100 * (GameManagerScript.GetCombo() * 0.01f + 1.0f));            // 加点&コンボ数増加関数を実行
            _rightEyeSprite.enabled = true;
            GameManagerScript.AppearDaruma();
            Destroy(this.gameObject);
        }
        else
        {
            GameManagerScript.ResetCombo();             // コンボリセットを実行
            _rightEyeTransform.localScale *= 1.1f;
        }
    }
    
    public void LeftButtonClick()
    {
        if (!_leftEyeSprite.enabled)
        {
            GameManagerScript.AddScoreCombo(100 * (GameManagerScript.GetCombo() * 0.01f + 1.0f));            // 加点&コンボ数増加関数を実行
            _leftEyeSprite.enabled = true;
            GameManagerScript.AppearDaruma();
            Destroy(this.gameObject);
        }
        else
        {
            GameManagerScript.ResetCombo();             // コンボリセットを実行
            _leftEyeTransform.localScale *= 1.1f;
        }
    }

    public void SetGameManager(GameObject gameManagerObj)       // GameManagerがこれにGameManagerをつけてくれる
    {
        GameManagerScript = gameManagerObj.GetComponent<GameManager>();
    }
}
