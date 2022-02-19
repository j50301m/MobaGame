using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Google.Protobuf.Collections;

namespace Game.Model
{ 
    //保存房間數據
    public class RoomModel : Singleton<RoomModel>
    {
        public RepeatedField<PlayerInfo> playerInfos =new RepeatedField<PlayerInfo>();
        public Dictionary<int, GameObject> playerObjectDIC=new Dictionary<int, GameObject>();//玩家英雄的字典 key=rolId
        public Dictionary<int, HeroAttributeEntity> heroCurrentAtt = new Dictionary<int, HeroAttributeEntity>(); //玩家角色當前屬性
        public Dictionary<int, HeroAttributeEntity> heroTotalAtt = new Dictionary<int, HeroAttributeEntity>(); //玩家角色總屬性
    }
}

