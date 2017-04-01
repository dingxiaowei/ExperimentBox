using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class SafetyInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class SafetyInfoManager : TableManager<SafetyInfo, SafetyInfoManager>
{
    public override string TableName()
    {
        return "Text/SafetyInfo";
    }

}
