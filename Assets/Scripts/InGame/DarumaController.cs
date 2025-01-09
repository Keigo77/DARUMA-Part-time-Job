using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarumaController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _eyeSpritesList;
    [SerializeField] private Transform[] _eyeTransformList;
    [SerializeField] private List<int> _randomNumList = new List<int> { 0, 1, 2, 3 };
    [SerializeField] private int _howDelete = 3;
    [SerializeField] private int _scoreBasis = 50;
    
    GameManager GameManagerScript = null;
    private SESingleton seInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        DeleteEye(_howDelete);
        this.seInstance = SESingleton.seInstance;
    }

    public void DeleteEye(int howDelete)    // ランダムで選ばれた片方の目を削除する
    {
        for (int i = 0; i < howDelete; i++)
        {
            int randomNum = Random.Range(0, _randomNumList.Count);      // 配列から数字を取り出して，それに対応する目を消す
            _eyeSpritesList[_randomNumList[randomNum]].enabled = false;
            _randomNumList.RemoveAt(randomNum);
        }
    }

    public void ButtonClicked(int directionNum)
    {
        if (_eyeSpritesList.Length <= directionNum) return;     // 2つ目だるまの時に上下ボタンを押したら早期return
        seInstance.PlayWriteSE();  // 書く効果音
        if (!_eyeSpritesList[directionNum].enabled)
        {
            // 加点
            _eyeSpritesList[directionNum].enabled = true;
            isCompeteEye();
        }
        else
        {
            _eyeTransformList[directionNum].localScale *= 1.3f;    // ミスしたら目を大きくする，コンボリセット，ミス効果音
            GameManagerScript.ResetCombo();
            this.seInstance.PlayMissSE();
        }
    }

    private void isCompeteEye()
    {
        bool isCompeteEye = true;
        for (int i = 0; i < _eyeSpritesList.Length; i++)    // 全部の目が書けていたら完成
        {
            if (!_eyeSpritesList[i].enabled) isCompeteEye = false;
        }
        if (isCompeteEye)       // もしだるまが完成したら加点，破壊，新しいのを出現．さらに完成した効果音
        {
            this.seInstance.PlayCompleteSE();
            GameManagerScript.AddScoreCombo(_scoreBasis * (GameManagerScript.combo * 0.01f + 1.0f));
            GameManagerScript.AppearDaruma();
            Destroy(this.gameObject);
        }
    }

    public void SetGameManager(GameObject gameManagerObj)       // GameManagerがこれにGameManagerをつけてくれる
    {
        GameManagerScript = gameManagerObj.GetComponent<GameManager>();
    }
}
