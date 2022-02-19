using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobaServer.Match;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;

namespace MobaServer.GameModule
{
    public class LobbyModule : GameModuleBase<LobbyModule>
    {
        public override void AddListener()
        {
            base.AddListener();
            NetEvent.Instance.AddEventListener(1300, HandleLobbyToMatchC2S);
            //NetEvent.Instance.AddEventListener(1301, HandleLobbyUpdateMatchStateC2S);
            NetEvent.Instance.AddEventListener(1302, HandleLobbyQuitMatchC2S);
            
        }

        /// <summary>
        /// 退出匹配
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyQuitMatchC2S(BufferEntity request)
        {
            LobbyQuitMatchC2S c2sMSG = ProtobufHelper.FromBytes<LobbyQuitMatchC2S>(request.proto);
            LobbyQuitMatchS2C s2cMSG = new LobbyQuitMatchS2C();

            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            if (player != null)
            {
                if (MatchManager.Instance.Remove(player.matchEntity))
                {
                    player.matchEntity = null;
                    s2cMSG.Result = 0; //取消列隊成功
                }
                else s2cMSG.Result = 1; //此玩家不在匹配狀態

                BufferFactory.CreateAndSendPackage(request,s2cMSG);
            }
        }

        /// <summary>
        /// 進入匹配
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyToMatchC2S(BufferEntity request)
        {
            LobbyToMatchC2S c2sMSG = ProtobufHelper.FromBytes<LobbyToMatchC2S>(request.proto);
            LobbyToMatchS2C s2cMSG = new LobbyToMatchS2C();
            s2cMSG.Result = 0;

            MatchEntity matchEntity = new MatchEntity();
            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);

            matchEntity.teamId = player.rolesInfo.RolesID;
            matchEntity.player = player;
            player.matchEntity = matchEntity;

            BufferFactory.CreateAndSendPackage(request, s2cMSG);
            MatchManager.Instance.Add(matchEntity);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Release()
        {
            base.Release();
            NetEvent.Instance.RemoveEventListener(1300, HandleLobbyToMatchC2S);
            //NetEvent.Instance.RemoveEventListener(1301, HandleLobbyUpdateMatchStateC2S);
            NetEvent.Instance.RemoveEventListener(1302, HandleLobbyQuitMatchC2S);
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
        }
    }
}
