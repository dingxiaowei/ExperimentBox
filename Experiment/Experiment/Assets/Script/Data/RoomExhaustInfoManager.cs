using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class RoomExhaustInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class RoomExhaustInfoManager : TableManager<RoomExhaustInfo, RoomExhaustInfoManager>
{
    public override string TableName()
    {
        return "Text/RoomExhaustInfo";
    }

}
