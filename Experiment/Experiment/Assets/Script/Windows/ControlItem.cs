using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class ControlItem : MonoBehaviour {

    // Use this for initialization
    public Text funcName;
    [HideInInspector]
    public ControlInfo ctrlInfo;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(params object[] obj)
    {
        ctrlInfo = (ControlInfo)obj[0];
        UpdateShow();
    }

    public void UpdateShow()
    {
        funcName.text = ctrlInfo.name;
    }


    public void OnOpen()
    {
        //ExperimentMain.Instance.MainScrollView.SetActive(false);
        //ExperimentMain.Instance.LampScrollView.SetActive(true);
        ExperimentMain.Instance.SetRoomLamp();



    }
}
