using Game.Ctrl;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class RolesWindow : BaseWindow
    {
        public RolesWindow()
        {
            selfType = WindowType.RolesWindow;
            scenesType = ScenesType.Login;
            resident = false;
            resName = "UIPrefab/Roles/RolesWindow";
        }

        private InputField inputField; //輸入框

        protected override void Awake()
        {
            base.Awake();
            inputField = transform.Find("RolesBG/InputField").GetComponent<InputField>();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1201, HandleRolesCreateS2C);
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            NetEvent.Instance.RemoveEventListener(1201, HandleRolesCreateS2C);
        }


        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }



        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "StartBtn":
                        buttonList[i].onClick.AddListener(startBtnOnClick);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 按下開始鈕(發送創角色訊息)
        /// </summary>
        private void startBtnOnClick()
        {
            RolesCreateC2S c2sMSG = new RolesCreateC2S();
            c2sMSG.NickName = inputField.text;

            BufferFactory.CreateAndSendPackage(1201, c2sMSG);
        }

        /// <summary>
        /// 處理Server回傳訊息
        /// </summary>
        /// <param name="obj"></param>
        private void HandleRolesCreateS2C(BufferEntity response)
        {
            RolesCreateS2C s2cMSG = ProtobufHelper.FromBytes<RolesCreateS2C>(response.proto);
            if (s2cMSG.Result == 0)//創建成功
            {
                //儲存角色
                RolesCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
                //關閉自己
                Close();
                //打開大廳窗口
                WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
            }
            else
            {
                //角色已存在 創建失敗
                Debug.Log("此名稱被創建過了,創建失敗");
                WindowManager.Instance.ShowTips("此名稱被使用了,創建失敗");
            }
        }
    }
}

