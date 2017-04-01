using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SafetyItem : MonoBehaviour
{

    // Use this for initialization
    public Text funcName;
    [HideInInspector]
    public string mess;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(params object[] obj)
    {
        mess = (string)obj[0];
        UpdateShow();
    }

    public void UpdateShow()
    {
        funcName.text = mess;
    }


    public void OnOpen()
    {

    }
}


