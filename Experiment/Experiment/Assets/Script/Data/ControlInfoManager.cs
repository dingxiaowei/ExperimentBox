using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class ControlInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class ControlInfoManager : TableManager<ControlInfo, ControlInfoManager>
{

    public override string TableName()
    {
        return "Text/ControlInfo";
    }

}
