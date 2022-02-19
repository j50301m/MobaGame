using Game.Model;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrl
{
    public class RolesCtrl : Singleton<RolesCtrl>
    {
        /// <summary>
        /// 保存角色資訊
        /// </summary>
        /// <param name="rolesInfo"></param>
        public void SaveRolesInfo(RolesInfo rolesInfo)
        {
            PlayerModel.Instance.roleInfo = rolesInfo;
        }

        /// <summary>
        /// 獲取角色資訊
        /// </summary>
        /// <returns></returns>
        public RolesInfo GetRolesInfo()
        {
            return PlayerModel.Instance.roleInfo;
        }

        /// <summary>
        /// 保存房間訊息
        /// </summary>
        /// <param name="roomInfo"></param>
        public void SaveRoomInfo(RoomInfo roomInfo)
        {
            PlayerModel.Instance.roomInfo = roomInfo;
        }

        /// <summary>
        /// 獲取房間訊息
        /// </summary>
        /// <param name="roomInfo"></param>
        /// <returns></returns>
        public RoomInfo GetRoomInfo()
        {
            return PlayerModel.Instance.roomInfo;
        }

        /// <summary>
        /// 檢查是否是自己的角色
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public bool CheckIsSelf(int rolesId)
        {
            return PlayerModel.Instance.roleInfo.RolesID == rolesId;
        }
    }
}

