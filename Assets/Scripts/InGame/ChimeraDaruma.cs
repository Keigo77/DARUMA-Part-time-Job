using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraDaruma : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _eyeSpritesList;
    [SerializeField] private Transform[] _eyeTransformList;
    [SerializeField] private List<int> _randomNumList = new List<int> { 0, 1, 2, 3 };
    
    GameManager GameManagerScript = null;
    
    // Start is called before the first frame update
    void Start()
    {
        DeleteEye(3);
    }

    public void DeleteEye(int howDelete)    // ランダムで選ばれた片方の目を削除する
    {
        for (int i = 0; i < howDelete; i++)
        {
            int randomNum = Random.Range(0, _randomNumList.Count);      // 配列から数字を取り出して，それに対応する目を消す
            _randomNumList.Remove(randomNum);
            _eyeSpritesList[randomNum].enabled = false;
        }
    }

    public void ButtonClick(int directionNum)
    {
        if (!_eyeSpritesList[directionNum].enabled)
        {
            // 加点
            _eyeSpritesList[directionNum].enabled = true;
            GameManagerScript.AddScoreCombo(100 * (GameManagerScript.GetCombo() * 0.01f + 1.0f));
        }
        else
        {
            _eyeTransformList[directionNum].localScale *= 1.1f;
        }
    }

    public void SetGameManager(GameObject gameManagerObj)       // GameManagerがこれにGameManagerをつけてくれる
    {
        GameManagerScript = gameManagerObj.GetComponent<GameManager>();
    }
}
