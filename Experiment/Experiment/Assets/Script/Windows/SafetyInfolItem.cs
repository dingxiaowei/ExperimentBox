using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SafetyInfolItem : MonoBehaviour {

    public Text funcName;
    [HideInInspector]
    public SafetyInfo safeInfo;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(params object[] obj)
    {
        safeInfo = (SafetyInfo)obj[0];
        UpdateShow();
    }

    public void UpdateShow()
    {
        funcName.text = safeInfo.name;
    }
    public void OnOpen()
    {
        ExperimentMain.Instance.SetSafetyMess();
    }
}
