using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class RoomAdapterInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class RoomAdapterInfoManager : TableManager<RoomAdapterInfo, RoomAdapterInfoManager>
{
    public override string TableName()
    {
        return "Text/RoomAdapterInfo";
    }

}

