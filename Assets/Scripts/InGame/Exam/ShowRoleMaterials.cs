using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowRoleMaterials : MonoBehaviour
{
    [SerializeField] private TextAsset[] _materials;
    [SerializeField] private GameObject _selectPanel;
    [SerializeField] private GameObject _showMaterialPanel;
    [SerializeField] private TextMeshProUGUI _materialText;
    
    // Start is called before the first frame update
    void Start()
    {
        _showMaterialPanel.SetActive(false);
    }
    
    
    public void ShowMaterial(int gettedRole)
    {
        TextAsset textFile = _materials[gettedRole];  // その位の資料を表示する．
        _selectPanel.SetActive(false);
        string[] showText = textFile.text.Split('\n');

        for (int i = 0; i < showText.Length; i++)
        {
            _materialText.text += showText[i] + "\n";
        }
        _showMaterialPanel.SetActive(true);
    }

    public void DeleteMaterial()
    {
        SESingleton.seInstance.PlayPushCancellButtonSound();
        _showMaterialPanel.SetActive(false);
        _selectPanel.SetActive(true);
    }
}
