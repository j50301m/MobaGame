using MobaServer.Net;
using MobaServer.Room;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.GameModule
{
    public class BattleModule : GameModuleBase<BattleModule>
    {
        public override void AddListener()
        {
            base.AddListener();
            NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputC2S);
        }
        /// <summary>
        /// 處理用戶傳輸的輸入
        /// </summary>
        /// <param name="request"></param>
        private void HandleBattleUserInputC2S(BufferEntity request)
        {
            BattleUserInputC2S  c2sMSG= ProtobufHelper.FromBytes<BattleUserInputC2S>(request.proto);
            RoomEntity roomEntity=RoomManager.Instance.GetRoom(c2sMSG.RoomID);
            if (roomEntity != null)
            {
                roomEntity.HandleBattleUserInputC2S(c2sMSG);
            }
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
            NetEvent.Instance.RemoveEventListener(1500, HandleBattleUserInputC2S);
        }
    }
}
