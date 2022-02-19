using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;

namespace MobaServer.GameModule
{
    public class RoomModule : GameModuleBase<RoomModule>
    {
        public override void AddListener()
        {
            base.AddListener();
            NetEvent.Instance.AddEventListener(1400, HandleRoomSelectHeroC2S);
            NetEvent.Instance.AddEventListener(1401, HandleRoomSelectHeroSkillC2S);
            //NetEvent.Instance.AddEventListener(1402, HandleRoomCreateC2S);
            NetEvent.Instance.AddEventListener(1404, HandleRoomSendMsgC2S);
            NetEvent.Instance.AddEventListener(1405, HandleRoomLockHeroC2S);
            NetEvent.Instance.AddEventListener(1406, HandleRoomLoadProgressC2S);

        }

        private void HandleRoomLoadProgressC2S(BufferEntity request)
        {
            RoomLoadProgressC2S c2sMSG = ProtobufHelper.FromBytes<RoomLoadProgressC2S>(request.proto);
            RoomLoadProgressS2C s2cMSG = new RoomLoadProgressS2C();
            s2cMSG.IsBattleStart = false;

            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            bool result=player.roomEntity.UpdateLoadProgress(player.rolesInfo.RolesID, c2sMSG.LoadProgress);

            //如果沒有還有玩家沒加載完
            if (!result)
            {
                //單獨回復 發送request的Client
                player.roomEntity.GetLoadProgress(ref s2cMSG);
                BufferFactory.CreateAndSendPackage(request, s2cMSG);
            } 
        }

        /// <summary>
        /// 鎖定英雄
        /// </summary>
        /// <param name="request"></param>
        private void HandleRoomLockHeroC2S(BufferEntity request)
        {
            RoomLockHeroC2S c2sMSG = ProtobufHelper.FromBytes<RoomLockHeroC2S>(request.proto);
            RoomLockHeroS2C s2cMSG = new RoomLockHeroS2C();
            s2cMSG.HeroID = c2sMSG.HeroID;

            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            s2cMSG.RolesID = player.rolesInfo.RolesID;

            //緩存玩家所選角色
            player.roomEntity.LockHero(s2cMSG.RolesID, s2cMSG.HeroID);

            //只廣播給同隊伍玩家
            //player.roomEntity.Broadcast(player.TeamId, request.messageID, s2cMSG);
            //廣撥給全部人 測試用
            player.roomEntity.Broadcast(request.messageID, s2cMSG);

        }

        /// <summary>
        /// 隊伍聊天訊息
        /// </summary>
        /// <param name="request"></param>
        private void HandleRoomSendMsgC2S(BufferEntity request)
        {
            RoomSendMsgC2S c2sMSG = ProtobufHelper.FromBytes<RoomSendMsgC2S>(request.proto);
            RoomSendMsgS2C s2cMSG = new RoomSendMsgS2C();
            s2cMSG.Text = c2sMSG.Text;

            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            s2cMSG.RolesID = player.rolesInfo.RolesID;
            //只廣播給同隊伍玩家
            //player.roomEntity.Broadcast(player.TeamId, request.messageID, s2cMSG);

            //廣撥給全部人 測試用
            player.roomEntity.Broadcast(request.messageID, s2cMSG);
        }

        /// <summary>
        /// 選擇召喚師技能
        /// </summary>
        /// <param name="request"></param>
        private void HandleRoomSelectHeroSkillC2S(BufferEntity request)
        {
            RoomSelectHeroSkillC2S c2sMSG = ProtobufHelper.FromBytes<RoomSelectHeroSkillC2S>(request.proto);
            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            PlayerInfo playerinfo=player.roomEntity.UpdateSkill(player.rolesInfo.RolesID, c2sMSG.SkillID, c2sMSG.GridID);

            //第一個技能
            RoomSelectHeroSkillS2C s2cMSG_skill0 = new RoomSelectHeroSkillS2C()
            {
                RolesID = player.rolesInfo.RolesID,
                GridID=0,
                SkillID=playerinfo.SkillA
            };
            //第2個技能
            RoomSelectHeroSkillS2C s2cMSG_skill1 = new RoomSelectHeroSkillS2C()
            {
                RolesID = player.rolesInfo.RolesID,
                GridID = 1,
                SkillID = playerinfo.SkillB
            };
            player.roomEntity.Broadcast(request.messageID, s2cMSG_skill0);
            player.roomEntity.Broadcast(request.messageID, s2cMSG_skill1);
        }

        /// <summary>
        /// 用戶選擇英雄
        /// </summary>
        /// <param name="request"></param>
        private void HandleRoomSelectHeroC2S(BufferEntity request)
        {
            //還沒鎖定先不需要把角色存到playerEntity裡
            RoomSelectHeroC2S c2sMSG = ProtobufHelper.FromBytes<RoomSelectHeroC2S>(request.proto);
            RoomSelectHeroS2C s2cMSG = new RoomSelectHeroS2C();
            s2cMSG.HeroID = c2sMSG.HeroID;
            PlayerEntity player=PlayerManager.GetPlayerEntityFromSession(request.session);
            s2cMSG.RolesID = player.rolesInfo.RolesID;

            player.roomEntity.Broadcast(request.messageID, s2cMSG);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            NetEvent.Instance.RemoveEventListener(1400, HandleRoomSelectHeroC2S);
            NetEvent.Instance.RemoveEventListener(1401, HandleRoomSelectHeroSkillC2S);
            NetEvent.Instance.RemoveEventListener(1404, HandleRoomSendMsgC2S);
            NetEvent.Instance.RemoveEventListener(1405, HandleRoomLockHeroC2S);
            NetEvent.Instance.RemoveEventListener(1406, HandleRoomLoadProgressC2S);
        }
    }
}
