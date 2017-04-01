using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public enum OptionType
{
    Safety,
    Control,
    Sensor
}


public enum ReturnLayer
{
    None,
    Control_Lamp_Layer1,
    Control_Lamp_Layer2,
    Control_Electric_Layer1,
    Control_Electric_Layer2,
    Control_Curtains_Layer1,
    Control_Curtains_Layer2,
    Control_Exhaust_Layer1,
    Control_Exhaust_Layer2,
    Control_Adapter_Layer1,
    Control_Adapter_Layer2,
    Safety_Layer
    
}


public class ExperimentMain : MonoBehaviour {
    public GameObject MainScrollView;
    public GameObject LampScrollView;
    public GameObject LampPanel;
    public GameObject SafetyScrollView;
    public GameObject SensorPanel;
    public GameObject Bottom;
    public GameObject TitleBottom;


    public Button ReturnBtn;
    public Button RefreshBtn;

    public GridLayoutGroup ShellThings;
    public GridLayoutGroup LampShellThings;
    public GridLayoutGroup SafetyShellThings;

    public Text TitleBottomText;
    public Text topTitle;
    [HideInInspector]
    public OptionType mSelectMode = OptionType.Control;
    public ReturnLayer mReturnLayer = ReturnLayer.None;

    //放置安防信息的链表
    private List<string> unSafeInfo;
    //网络类
    private ConnectSocket mySocket;

    public static ExperimentMain Instance
    {
        get
        {
            return GameObject.FindObjectOfType<ExperimentMain>();
        }
    }
    void Start () {
        mySocket = ConnectSocket.getSocketInstance();

        InitSetting();
        ClearStateShow();
        UpdateControlStateShow();

    }

    void InitSetting()
    {
        MainScrollView.SetActive(true);
        Bottom.SetActive(true);

        ReturnBtn.gameObject.SetActive(false);
        RefreshBtn.gameObject.SetActive(false);
        LampScrollView.SetActive(false);
        LampPanel.SetActive(false);
        SafetyScrollView.SetActive(false);
        SensorPanel.SetActive(false);
        TitleBottom.SetActive(false);


        //报警池信息初始化
        unSafeInfo = new List<string>();

        //手动添加报警信息
        AddUnsafeList("2017年3月8日10点20分有人靠近");
        AddUnsafeList("2017年3月8日10点20分温度传感器报警");
        AddUnsafeList("2017年3月8日10点20分可燃性气体传感器报警");
        AddUnsafeList("2017年3月8日10点20分火焰传感器报警");



    }
    #region
    public void AddUnsafeList(string unsafemess)
    {
        unSafeInfo.Add(unsafemess);
    }


    #endregion

    void Update () {
	
	}

    public void OnChoose(int choose)
    {
        if (choose == 1) mSelectMode = OptionType.Safety;
        else if ( choose == 2) mSelectMode = OptionType.Control;
        else mSelectMode = OptionType.Sensor;

        UpdateMiddleShow();
    }
    //更新滚动窗体显示内容
    public void UpdateMiddleShow()
    {
        ClearStateShow();
        switch (mSelectMode)
        {
            case OptionType.Sensor:
                UpdateSensorStateShow();
                break;
            case OptionType.Safety:
                UpdateSafetyStateShow();
                break;
            case OptionType.Control:
                UpdateControlStateShow();
                break;
            default:
                break;

        }  
    }

    //清除滚动框的节点
    public void ClearStateShow()
    {
        for (int i = 0; i < ShellThings.transform.childCount; i++)
        {
            Destroy(ShellThings.transform.GetChild(i).gameObject);
        }
    }
    //显示传感器信息状态
    public void UpdateSensorStateShow()
    {
        MainScrollView.SetActive(false);
        SensorPanel.SetActive(true);
    }

    //显示控制信息状态
    public void UpdateControlStateShow()
    {
        MainScrollView.SetActive(true);
        SensorPanel.SetActive(false);
        ControlInfo[] ctrlItems = ControlInfoManager.Instance.GetAllItem();

        foreach (ControlInfo item in ctrlItems)
        {
            AddControlItem(item);
        }
    }


    #region  安防
    //显示安防信息状态
    public void UpdateSafetyStateShow()
    {
        MainScrollView.SetActive(true);
        SensorPanel.SetActive(false);
        SafetyInfo[] safeItems = SafetyInfoManager.Instance.GetAllItem();

        foreach (SafetyInfo item in safeItems)
        {
            AddSafetyItem(item);
        }
    }

    //添加安防信息状态下的子节点
    public void AddSafetyItem(SafetyInfo safeItem)
    {
        var itemRes = Resources.Load("Prefabs/SafetyInfolItem");
        if (itemRes != null)
        {
            var itemObj = Instantiate(itemRes) as GameObject;
            itemObj.transform.parent = ShellThings.transform;
            itemObj.layer = gameObject.layer;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
            itemObj.transform.localScale = Vector3.one;
            itemObj.name = safeItem.id.ToString();

            itemObj.GetComponent<SafetyInfolItem>().Init(safeItem);

        }
        else
        {
            Debug.Log("====AddSafetyItem==safeItem===不存在==");
        }
    }
    public void SetSafetyMess()
    {
        mReturnLayer = ReturnLayer.Safety_Layer;

        MainScrollView.SetActive(false);
        Bottom.SetActive(false);
        SafetyScrollView.SetActive(true);
        SetTitleBottom("安防信息");
        ReturnBtn.gameObject.SetActive(true);


        //更新列表
        ClearSafetyMessShow();
        UpdateSafetyMessShow();
    }

    public void Return__Safety_Layer()
    {
        mReturnLayer = ReturnLayer.None;
        MainScrollView.SetActive(true);
        Bottom.SetActive(true);
        TitleBottom.SetActive(false);
        SafetyScrollView.SetActive(false);
        ReturnBtn.gameObject.SetActive(false);
    }
    //添加没有报警新节点 
    public void NoUnsafeMess()
    {
        AddSafetyMessItem(0, "没有报警信息");
    }
    //清空报警信息节点
    public void ClearSafetyMessShow()
    {
        for (int i = 0; i < SafetyShellThings.transform.childCount; i++)
        {
            Destroy(SafetyShellThings.transform.GetChild(i).gameObject);
        }
    }

    //更新报警信息显示
    public void UpdateSafetyMessShow()
    {
        int index = 0;
        foreach (var item in unSafeInfo)
        {
            index++;
            AddSafetyMessItem(index, item);
        }
        if (index == 0) NoUnsafeMess();
    }

    //添加报警信息子节点
    public void AddSafetyMessItem(int index, string mess)
    {
        var itemRes = Resources.Load("Prefabs/SafetyItem");
        if (itemRes != null)
        {
            var itemObj = Instantiate(itemRes) as GameObject;
            itemObj.transform.parent = SafetyShellThings.transform;
            itemObj.layer = gameObject.layer;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
            itemObj.transform.localScale = Vector3.one;
            itemObj.name = index.ToString();

            itemObj.GetComponent<SafetyItem>().Init(mess);

        }
    }
    #endregion

        //添加控制信息下的子节点
    public void AddControlItem(ControlInfo ctrlItem)
    {
        var itemRes = Resources.Load("Prefabs/ControlItem");
        if (itemRes != null)
        {
            var itemObj = Instantiate(itemRes) as GameObject;
            itemObj.transform.parent = ShellThings.transform;
            itemObj.layer = gameObject.layer;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
            itemObj.transform.localScale = Vector3.one;
            itemObj.name = ctrlItem.id.ToString();

            itemObj.GetComponent<ControlItem>().Init(ctrlItem);

        }
        else
        {
            Debug.Log("====AddControlItem==ControlItem===不存在==");
        }
    }


    //设置底边框显示
    public void SetTitleBottom(string mtitle)
    {
        TitleBottom.SetActive(true);
        TitleBottomText.text = mtitle;
    }

    #region 房间灯
    public void SetRoomLamp()
    {
        mReturnLayer = ReturnLayer.Control_Lamp_Layer1;

        MainScrollView.SetActive(false);
        Bottom.SetActive(false);
        LampScrollView.SetActive(true);
        SetTitleBottom("灯光控制");
        ReturnBtn.gameObject.SetActive(true);


        //更新列表
        ClearLampRoomShow();
        UpdateLampRoomShow();
    }
    public void Return__Lamp_Layer1()
    {
        ClearLampRoomShow();

        MainScrollView.SetActive(true);
        Bottom.SetActive(true);
        LampScrollView.SetActive(false);
        TitleBottom.SetActive(false);
        ReturnBtn.gameObject.SetActive(false);


        mReturnLayer = ReturnLayer.None;


    }
    //清除房灯节点
    public void ClearLampRoomShow()
    {
        for (int i=0; i<LampShellThings.transform.childCount;i++)
        {
            Destroy(LampShellThings.transform.GetChild(i).gameObject);
        }
    }
    //更新房灯显示
    public void UpdateLampRoomShow()
    {
        var lampItems = RoomLampInfoManager.Instance.GetAllItem();

        foreach (RoomLampInfo item in lampItems)
        {
            AddRoomLampItem(item);
        }
    }
    //添加房灯的子节点
    public void AddRoomLampItem(RoomLampInfo lampItem)
    {
        var itemRes = Resources.Load("Prefabs/LampItem");
        if (itemRes != null)
        {
            var itemObj = Instantiate(itemRes) as GameObject;
            itemObj.transform.parent = LampShellThings.transform;
            itemObj.layer = gameObject.layer;
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
            itemObj.transform.localScale = Vector3.one;
            itemObj.name = lampItem.id.ToString();

            itemObj.GetComponent<LampItem>().Init(lampItem);

        }
        else
        {
            Debug.Log("====AddSafetyItem==safeItem===不存在==");
        }
    }
    #endregion
    
    #region 设置灯面板显示隐藏

    public void SetLampPanel(RoomLampInfo itemLamp)
    {
        mReturnLayer = ReturnLayer.Control_Lamp_Layer2;

        LampScrollView.SetActive(false);
        SetTitleBottom(itemLamp.name);
        LampPanel.SetActive(true);

    }

    public void Return__Lamp_Layer2()
    {


        LampScrollView.SetActive(true);
        SetTitleBottom("灯光控制");
        LampPanel.SetActive(false);

        mReturnLayer = ReturnLayer.Control_Lamp_Layer1;


    }

    #endregion


    public void OnReturnClick()
    {
        switch (mReturnLayer)
        {
            case ReturnLayer.Control_Lamp_Layer1:
                Return__Lamp_Layer1();
                break;
            case ReturnLayer.Control_Lamp_Layer2:
                Return__Lamp_Layer2();
                break;
            case ReturnLayer.Control_Electric_Layer1:
                break;
            case ReturnLayer.Control_Electric_Layer2:
                break;
            case ReturnLayer.Control_Curtains_Layer1:
                break;
            case ReturnLayer.Control_Curtains_Layer2:
                break;
            case ReturnLayer.Control_Exhaust_Layer1:
                break;
            case ReturnLayer.Control_Exhaust_Layer2:
                break;
            case ReturnLayer.Control_Adapter_Layer1:
                break;
            case ReturnLayer.Control_Adapter_Layer2:
                break;
            case ReturnLayer.Safety_Layer:
                Return__Safety_Layer();

                break;
            default:
                break;
        }
    }

    public void OnCancel()
    {

    }

    public void OnClose()
    {
        //Application.Quit();
    }

    #region 发送消息
    public void SendCtrlMess()
    {
        if (mySocket == null) mySocket = ConnectSocket.getSocketInstance();
        byte[] messByte = new byte[13];
        //帧头
        messByte[0] = 0x54;
        messByte[1] = 0x43;
        //流水号
        messByte[2] = 0x00;
        messByte[3] = 0x00;
        messByte[4] = 0x00;
        messByte[5] = 0x00;
        // 设备类型 的灯
        messByte[6] = 0x01;
        //设备编号
        messByte[7] = 0x01;
        //功能码
        messByte[8] = 0x01;
        //数据长度
        messByte[9] = 0x00;

        //CRC16校验
        messByte[10] = 0x54;
        messByte[11] = 0x54;
        //帧尾
        messByte[12] = 0xED;
        mySocket.SendMess(messByte);

    }
    #endregion
}
