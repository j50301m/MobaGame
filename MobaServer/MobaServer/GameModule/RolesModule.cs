using MobaServer.DBCMD;
using MobaServer.MySql;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.GameModule
{
    public class RolesModule : GameModuleBase<RolesModule>
    {
        public override void AddListener()
        {
            base.AddListener();
            NetEvent.Instance.AddEventListener(1201, HandleRolesCreateC2S);
        }

        /// <summary>
        /// 去DB查詢角色表
        /// </summary>
        /// <param name="obj"></param>
        private void HandleRolesCreateC2S(BufferEntity request)
        {
            //反序列化
            RolesCreateC2S c2sMSG = ProtobufHelper.FromBytes<RolesCreateC2S>(request.proto);

            RolesCreateS2C s2cMSG = new RolesCreateS2C();
            //查詢此暱稱是否被使用過
            if (DBRolesInfo.Instance.Select(MySqlCMD.Where("NickName", c2sMSG.NickName)) == null)
            {
                //獲取當前用戶ID
                PlayerEntity playerEntity = PlayerManager.GetPlayerEntityFromSession(request.session);

                RolesInfo rolesInfo = new RolesInfo();
                rolesInfo.NickName = c2sMSG.NickName;
                rolesInfo.ID = playerEntity.userInfo.ID;
                rolesInfo.RolesID = playerEntity.userInfo.ID;

                bool result = DBRolesInfo.Instance.Insert(rolesInfo);
                if (result) //創角成功
                {
                    s2cMSG.Result = 0;
                    s2cMSG.RolesInfo = rolesInfo;
                    
                    //緩存角色訊息到Server本地
                    playerEntity.rolesInfo = rolesInfo;
                    PlayerManager.Update(request.session, rolesInfo.RolesID, playerEntity);
                }
                else
                {
                    s2cMSG.Result = 2; //無法寫入 
                    Debug.Log($"插入角色數據異常,暱稱{c2sMSG.NickName}!");
                }

            }
            else
            {
                s2cMSG.Result = 1; //已被創建過
            }
            BufferFactory.CreateAndSendPackage(request,s2cMSG);
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
            NetEvent.Instance.RemoveEventListener(1201, HandleRolesCreateC2S);
        }
    }
}
