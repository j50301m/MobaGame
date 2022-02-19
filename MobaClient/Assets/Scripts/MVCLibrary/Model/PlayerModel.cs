using System.Collections;
using System.Collections.Generic;
using ProtoMsg;
using UnityEngine;

namespace Game.Model
{
    public  class PlayerModel : Singleton<PlayerModel>
    {
        public RolesInfo roleInfo { get; internal set; }
        public RoomInfo roomInfo { get; internal set; }

    }
}

