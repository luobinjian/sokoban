using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class changeName : MonoBehaviour
{
    public TMP_InputField inputField;
    
    public SveManage sveManage;

    void Start()
    {
        PlayerPrefs.SetString("DefaultName","tileMapData.json");
        // 自动获取自身的 TMP_InputField 组件
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
        }
        
        // 添加监听器
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnInputEndEdit);
        }
    }

    void OnInputEndEdit(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Input field is empty");
            return;
        }
        Debug.Log(value);
        sveManage.filename = value + ".json";
        PlayerPrefs.SetString("DefaultName",value);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        // 移除监听（避免内存泄漏）
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnInputEndEdit);
        }
    }
}
