using System;
using System.Collections;
using System.Collections.Generic;
using Game.Ctrl;
using Game.Net;
using ProtoMsg;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class LobbyWindow : BaseWindow
    {
        public LobbyWindow()
        {
            selfType = WindowType.LobbyWindow;
            scenesType = ScenesType.Login;
            resident = false;
            resName = "UIPrefab/Lobby/LobbyWindow";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        private Transform matchTips, matchModeBtn, qualifyingBtn,stopMatchBtn; //匹配中提示訊息、匹配按鈕、排位賽按鈕
        private Text rolesName, rank, goldCount, diamondsCount; //角色暱稱、段位、金幣、鑽石
        protected override void Awake()
        {
            base.Awake();
            //暱稱、段位、金幣、寶石 附值
            rolesName = transform.Find("LobbyBG/RolesName").GetComponent<Text>();
            rank = transform.Find("LobbyBG/Duan").GetComponent<Text>();
            goldCount = transform.Find("LobbyBG/GoldCount").GetComponent<Text>();
            diamondsCount = transform.Find("LobbyBG/DiamondsCount").GetComponent<Text>();

            //匹配、排位、停止匹配按鈕
            matchModeBtn = transform.Find("LobbyBG/MatchModeBtn");
            qualifyingBtn = transform.Find("LobbyBG/QualifyingBtn");
            stopMatchBtn = transform.Find("LobbyBG/StopMatchBtn");
            //提示
            matchTips = transform.Find("LobbyBG/MatchTips");
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1300, HandleLobbyToMatchS2C);
            NetEvent.Instance.AddEventListener(1301, HandleLobbyUpdateMatchStateS2C);
            NetEvent.Instance.AddEventListener(1302, HandleLobbyQuitS2C);
        }
        /// <summary>
        /// 退出匹配的結果
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyQuitS2C(BufferEntity response)
        {
            LobbyQuitMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyQuitMatchS2C>(response.proto);
            if (s2cMSG.Result == 0)
            {
                matchModeBtn.gameObject.SetActive(true);
                qualifyingBtn.gameObject.SetActive(true);
                stopMatchBtn.gameObject.SetActive(false);
                matchTips.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 更新匹配的狀態
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyUpdateMatchStateS2C(BufferEntity response)
        {
            LobbyUpdateMatchStateS2C s2cMSG = ProtobufHelper.FromBytes<LobbyUpdateMatchStateS2C>(response.proto);
            //匹配成功
            if (s2cMSG.Result == 0)
            {
                //先把button狀態改回來
                matchModeBtn.gameObject.SetActive(true);
                qualifyingBtn.gameObject.SetActive(true);
                stopMatchBtn.gameObject.SetActive(false);
                matchTips.gameObject.SetActive(false);

                //房間訊息
                RolesCtrl.Instance.SaveRoomInfo(s2cMSG.RoomInfo);
                Close();

                WindowManager.Instance.OpenWindow(WindowType.RoomWindow);
            }
        }

        /// <summary>
        /// 進入匹配的結果
        /// </summary>
        /// <param name="request"></param>
        private void HandleLobbyToMatchS2C(BufferEntity response)
        {
            LobbyToMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.proto);
            if (s2cMSG.Result != 0)
            {
                //無法匹配
                Debug.Log("無法匹配");
                return;
            }
            if (s2cMSG.Result == 0)
            {
                matchModeBtn.gameObject.SetActive(false);
                qualifyingBtn.gameObject.SetActive(false);
                stopMatchBtn.gameObject.SetActive(true);
                matchTips.gameObject.SetActive(true);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //獲取角色訊息
            RolesInfo rolesInfo=RolesCtrl.Instance.GetRolesInfo();

            //大廳面板 附值
            //rolesName, rank, goldCount, diamondsCount; 
            rolesName.text = rolesInfo.NickName;
            rank.text = rolesInfo.VictoryPoint.ToString();
            goldCount.text = rolesInfo.GoldCoin.ToString();
            diamondsCount.text = rolesInfo.Diamonds.ToString();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            NetEvent.Instance.RemoveEventListener(1300, HandleLobbyToMatchS2C);
            NetEvent.Instance.RemoveEventListener(1301, HandleLobbyUpdateMatchStateS2C);
            NetEvent.Instance.RemoveEventListener(1302, HandleLobbyQuitS2C);
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "MatchModeBtn":
                        buttonList[i].onClick.AddListener(MatchModeBtnOnClick);
                        break;
                    case "StopMatchBtn":
                        buttonList[i].onClick.AddListener(StopMatchBtnOnClick);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 按下停止匹配
        /// </summary>
        private void StopMatchBtnOnClick()
        {
            BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
        }

        /// <summary>
        /// 匹配按鈕按下
        /// </summary>
        private void MatchModeBtnOnClick()
        {
            BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
        }
    }
}

