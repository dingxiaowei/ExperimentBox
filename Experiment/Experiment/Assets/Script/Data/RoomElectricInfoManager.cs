using UnityEngine;
using System.Collections;
using System;

[ProtoBuf.ProtoContract(ImplicitFields = ProtoBuf.ImplicitFields.AllFields)]

public class RoomElectricInfo : ITableItem
{
    public int id;
    public string name;
    public int Key()
    {
        return id;
    }
}
public class RoomElectricInfoManager : TableManager<RoomElectricInfo, RoomElectricInfoManager>
{
    public override string TableName()
    {
        return "Text/RoomElectricInfo";
    }

}



