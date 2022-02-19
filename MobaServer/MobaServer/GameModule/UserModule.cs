using Google.Protobuf;
using MobaServer.DBCMD;
using MobaServer.MySql;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
    public class UserModule : GameModuleBase<UserModule>
    {
        public override void Init()
        {
            base.Init();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void AddListener()
        {
            base.AddListener();
            NetEvent.Instance.AddEventListener(1000, HandleRegisterC2S);
            NetEvent.Instance.AddEventListener(1001, HandleLoginC2S);
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            NetEvent.Instance.RemoveEventListener(1000, HandleRegisterC2S);
            NetEvent.Instance.RemoveEventListener(1001, HandleLoginC2S);
        }
        /// <summary>
        /// 註冊功能
        /// </summary>
        /// <param name="request"></param>
        private void HandleRegisterC2S(BufferEntity request)
        {
            //反序列化
            UserRegisterC2S c2sMSG = ProtobufHelper.FromBytes<UserRegisterC2S>(request.proto);

            //註冊帳號到DB
            UserRegisterS2C s2cMSG = new UserRegisterS2C();
            if (DBUserInfo.Instance.Select(MySqlCMD.Where("Account", c2sMSG.UserInfo.Account)) != null)
            {
                Debug.Log("帳號已被註冊");
                s2cMSG.Result = 3;
            }
            else  //帳號沒有被註冊 (可註冊)
            {
                bool result = DBUserInfo.Instance.Insert(c2sMSG.UserInfo);
                if (result == true) s2cMSG.Result = 0;//註冊成功
                else s2cMSG.Result = 4;//未知原因導致錯誤
            }

            //回傳結果
            BufferFactory.CreateAndSendPackage(request, s2cMSG);
        }
        /// <summary>
        /// 登入功能
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLoginC2S(BufferEntity request)
        {
            //反序列化
            UserLoginC2S c2sMSG = ProtobufHelper.FromBytes<UserLoginC2S>(request.proto);

            //查找是否有帳號
            string sqlCMD = MySqlCMD.Where("Account", c2sMSG.UserInfo.Account) +
                MySqlCMD.And("Password", c2sMSG.UserInfo.Password);

            UserLoginS2C s2cMSG = new UserLoginS2C();
            UserInfo userInfo = DBUserInfo.Instance.Select(sqlCMD);

            if (userInfo == null) //找不到的情況
            {
                s2cMSG.Result = 2;//帳號、密碼有誤
                //回傳結果
                BufferFactory.CreateAndSendPackage(request, s2cMSG);
                return;
            }

            s2cMSG.UserInfo = userInfo;
            s2cMSG.Result = 0;//登入成功

            //查找是否有角色
            RolesInfo rolesInfo = DBRolesInfo.Instance.Select(MySqlCMD.Where("ID", s2cMSG.UserInfo.ID));
            if (rolesInfo != null)  s2cMSG.RolesInfo = rolesInfo;

            //緩存一個player 到本地
            PlayerManager.Add(request.session, s2cMSG.UserInfo.ID, new PlayerEntity()
            {
                userInfo = s2cMSG.UserInfo,
                session = request.session,
                rolesInfo = s2cMSG.RolesInfo
            }) ;

            //回傳結果
            BufferFactory.CreateAndSendPackage(request, s2cMSG);
        }




    }
}
