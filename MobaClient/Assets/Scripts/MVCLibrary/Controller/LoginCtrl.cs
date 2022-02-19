using System;
using System.Collections;
using System.Collections.Generic;
using Game.Model;
using ProtoMsg;
using UnityEngine;

namespace Game.Ctrl
{
    public class LoginCtrl : Singleton<LoginCtrl>
    {

        internal void SaveRolesInfo(RolesInfo rolesInfo)
        {
            PlayerModel.Instance.roleInfo = rolesInfo;
        }
    }
}


