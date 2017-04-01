using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class RoomLampInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class RoomLampInfoManager : TableManager<RoomLampInfo, RoomLampInfoManager>
{
    public override string TableName()
    {
        return "Text/RoomLampInfo";
    }

}
