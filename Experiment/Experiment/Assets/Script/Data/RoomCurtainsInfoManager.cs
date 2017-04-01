using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class RoomCurtainsInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class RoomCurtainsInfoManager : TableManager<RoomCurtainsInfo, RoomCurtainsInfoManager>
{
    public override string TableName()
    {
        return "Text/RoomCurtainsInfo";
    }

}
