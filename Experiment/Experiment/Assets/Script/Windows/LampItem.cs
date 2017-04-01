using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LampItem : MonoBehaviour {

    // Use this for initialization
    public Text funcName;
    [HideInInspector]
    public RoomLampInfo lampInfo;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(params object[] obj)
    {
        lampInfo = (RoomLampInfo)obj[0];
        UpdateShow();
    }

    public void UpdateShow()
    {
        funcName.text = lampInfo.name;
    }


    public void OnOpen()
    {
        ExperimentMain.Instance.SetLampPanel(lampInfo);
    }
}
