using Game.Model;
using Google.Protobuf.Collections;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrl
{
    public class RoomCtrl : Singleton<RoomCtrl>
    {

        /// <summary>
        /// 獲取房間ID
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public int GetTeamId(int rolesId)
        {
            for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
            {
                if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesId)
                {
                    return 0;
                }
                if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesId)
                {
                    return 1;
                }
            }
            return -1;

        }
        
        /// <summary>
        /// 檢查英雄是否是自己的
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public bool CheckIsSelfRoles(int rolesId)
        {
            return PlayerModel.Instance.roleInfo.RolesID == rolesId;
        }

        /// <summary>
        /// 房間解散時，清除房間訊息
        /// </summary>
        /// <param name="roomInfo"></param>
        public void RemoveRoomInfo()
        {
            PlayerModel.Instance.roomInfo = null;
        }

        /// <summary>
        /// 取得玩家暱稱
        /// </summary>
        /// <returns></returns>
        public string GetNickName(int rolesId)
        {
            //遍歷TeamA
            for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
            {
                if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesId)
                {
                    return PlayerModel.Instance.roomInfo.TeamA[i].NickName;
                }
            }
            //遍歷TeamB
            for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamB.Count; i++)
            {
                if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesId)
                {
                    return PlayerModel.Instance.roomInfo.TeamB[i].NickName;
                }
            }
            //都找不到 返回空
            return null;
        }

        /// <summary>
        /// 儲存角色列表
        /// </summary>
        /// <param name="playerInfos"></param>
        public void SavePlayerList(RepeatedField<PlayerInfo> playerInfos)
        {
            RoomModel.Instance.playerInfos=playerInfos;
        }

        /// <summary>
        /// 取得房間裡全部資訊
        /// </summary>
        public RepeatedField<PlayerInfo> GetPlayernfos()
        {
            return RoomModel.Instance.playerInfos;
        }

        /// <summary>
        /// 保存房間內的玩家模型
        /// </summary>
        /// <param name="rolesId"></param>
        /// <param name="heroModel"></param>
        /// <returns></returns>
        public bool SavePlayerObject(int rolesId,GameObject heroModel)
        {
            if (!RoomModel.Instance.playerObjectDIC.ContainsKey(rolesId))
            {
                RoomModel.Instance.playerObjectDIC[rolesId] = heroModel;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空房間內所有玩家模型
        /// </summary>
        public void RemoveAllPlayerObject()
        {
            RoomModel.Instance.playerObjectDIC.Clear();
        }

        /// <summary>
        /// 保存角色屬性(當前和總屬性)
        /// </summary>
        /// <param name="rolesID"></param>
        /// <param name="currentAttribute"></param>
        /// <param name="totalAttribute"></param>
        public void SaveHeroAttribute(int rolesID, HeroAttributeEntity currentAttribute, HeroAttributeEntity totalAttribute)
        {
            RoomModel.Instance.heroCurrentAtt[rolesID] = currentAttribute;
            RoomModel.Instance.heroTotalAtt[rolesID] = totalAttribute;
        }

        /// <summary>
        /// 清除RoomModel裡戰鬥相關資料(角色模型、角色屬性)
        /// </summary>
        public void ClearBattleData()
        {
            RoomModel.Instance.playerObjectDIC.Clear();
            RoomModel.Instance.heroCurrentAtt.Clear();
            RoomModel.Instance.heroTotalAtt.Clear();
        }
    }
}




