using Battle;
using Game.Ctrl;
using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.View
{
    public class RoomWindow : BaseWindow
    {
        public RoomWindow()
        {
            selfType = WindowType.RoomWindow;
            scenesType = ScenesType.Login;
            resident = false;
            resName = "UIPrefab/Room/RoomWindow";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //按下Enter發送聊天訊息
            if (Input.GetKeyDown(KeyCode.Return))
            {
                BufferFactory.CreateAndSendPackage(1404, new RoomSendMsgC2S()
               {
                    Text=chatInput.text
                });
            }
        }

        private Transform skillInfo;
        private Text time, chatText; //倒數和 聊天內容
        private Scrollbar chatVertical; //聊天窗的拉霸
        public int countDown = 10;
        private CancellationTokenSource cancelToken;
        private Transform teamA_HeroItem, teamB_HeroItem;
        private Dictionary<int, GameObject> rolesDIC;
        private Image skillA, skillB; //召喚師技能
        private InputField chatInput;

        protected override void Awake()
        {
            base.Awake();
            //初始化 
            countDown = 20; //倒數時間
            rolesDIC = new Dictionary<int, GameObject>();  //角色字典
            playerLoadDIC = new Dictionary<int, GameObject>(); //把加載中的heroItrm都存起來 key=rolesId value=heroItem
            BattleListener.Instance.Init(); //初始化 戰鬥監聽

            //查找組件
            chatInput = transform.Find("ChatInput").GetComponent<InputField>();
            skillInfo = transform.Find("SkillInfo");
            time = transform.Find("Time").GetComponent<Text>();
            chatText = transform.Find("ChatBG/Scroll View/Viewport/Content/ChatText").GetComponent<Text>();
            chatVertical = transform.Find("ChatBG/Scroll View/ChatVertical").GetComponent<Scrollbar>();
            teamA_HeroItem = transform.Find("TeamA/Team_HeroA_item");
            teamB_HeroItem = transform.Find("TeamB/Team_HeroB_item");
            skillA = transform.Find("SkillA").GetComponent<Image>();
            skillB = transform.Find("SkillB").GetComponent<Image>();

            //房間訊息
            RoomInfo roomInfo = RolesCtrl.Instance.GetRoomInfo();
            for (int i = 0; i < roomInfo.TeamA.Count; i++)
            {
                GameObject go = GameObject.Instantiate(teamA_HeroItem.gameObject, teamA_HeroItem.parent, false);
                go.transform.Find("Hero_NickName").GetComponent<Text>().text = roomInfo.TeamA[i].NickName;
                go.SetActive(true);
                rolesDIC[roomInfo.TeamA[i].RolesID] = go;
            }
            for (int i = 0; i < roomInfo.TeamB.Count; i++)
            {
                GameObject go = GameObject.Instantiate(teamB_HeroItem.gameObject, teamB_HeroItem.parent, false);
                go.transform.Find("Hero_NickName").GetComponent<Text>().text = roomInfo.TeamB[i].NickName;
                go.SetActive(true);
                rolesDIC[roomInfo.TeamB[i].RolesID] = go;
            }

            //開始倒數計時
            cancelToken = new CancellationTokenSource();
            time.text = $"倒數計時:{countDown}";
            TimeDown();
        }


        private async void TimeDown()
        {
            while (countDown > 0)
            {

                await Task.Delay(1000); //每隔一秒
                if (!cancelToken.IsCancellationRequested)
                {
                    countDown -= 1;
                    time.text = $"倒數計時:{countDown}";
                }
                else return;

            }
        }
        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1400, HeandleRoomSelectHeroS2C);
            NetEvent.Instance.AddEventListener(1401, HeandleRoomSelectHeroSkillS2C);
            //NetEvent.Instance.AddEventListener(1402, HeandleRoomCreateC2S);
            NetEvent.Instance.AddEventListener(1403, HeandleRoomCloseS2C);
            NetEvent.Instance.AddEventListener(1404, HeandleRoomSendMsgS2C);
            NetEvent.Instance.AddEventListener(1405, HeandleRoomLockHeroS2C);
            NetEvent.Instance.AddEventListener(1406, HeandleRoomLoadProgressS2C);
            NetEvent.Instance.AddEventListener(1407, HeandleRoomToBattleS2C);
        }

        private Dictionary<int, GameObject> playerLoadDIC;
        private Transform heroA_item, heroB_item;
        private AsyncOperation async;
        /// <summary>
        /// 加載進入戰鬥
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomToBattleS2C(BufferEntity response)
        {
            RoomToBattleS2C s2cMSG = ProtobufHelper.FromBytes<RoomToBattleS2C>(response.proto);
            RoomCtrl.Instance.SavePlayerList(s2cMSG.PlayerList);

            //設置讀取畫面
            transform.Find("LoadBG").gameObject.SetActive(true);
            //獲取角色框
            heroA_item = transform.Find("LoadBG/L_TeamA/HeroA_item");
            heroB_item = transform.Find("LoadBG/L_TeamB/HeroB_item");

            for (int i = 0; i < s2cMSG.PlayerList.Count; i++)
            {
                GameObject go;
                //隊伍A
                if (s2cMSG.PlayerList[i].TeamID == 0)
                {
                    go = GameObject.Instantiate(heroA_item.gameObject, heroA_item.parent, false);
                }
                //隊伍B
                else if (s2cMSG.PlayerList[i].TeamID == 1)
                {
                    go = GameObject.Instantiate(heroB_item.gameObject, heroB_item.parent, false);
                }
                else
                {
                    Debug.LogError("加載畫面缺少隊伍ID");
                    return;
                }
                //設置屬性
                go.transform.GetComponent<Image>().sprite = ResManager.Instance.LoadHeroTexture(s2cMSG.PlayerList[i].HeroID);
                go.transform.Find("NickName").GetComponent<Text>().text = s2cMSG.PlayerList[i].RolesInfo.NickName;
                go.transform.Find("SkillA").GetComponent<Image>().sprite = ResManager.Instance.LoadGeneraSkill(s2cMSG.PlayerList[i].SkillA);
                go.transform.Find("SkillB").GetComponent<Image>().sprite = ResManager.Instance.LoadGeneraSkill(s2cMSG.PlayerList[i].SkillB);
                go.transform.Find("Progress").GetComponent<Text>().text = "0%";
                go.gameObject.SetActive(true);
                //保存到字典中
                playerLoadDIC[s2cMSG.PlayerList[i].RolesInfo.RolesID] = go;
            }

            async = SceneManager.LoadSceneAsync("Level01");
            async.allowSceneActivation = false; //不要自動加載

            //定時發送加載進度
            SendProgeress();
        }

        /// <summary>
        /// 發送加載進度
        /// </summary>
        private async void SendProgeress()
        {
            BufferFactory.CreateAndSendPackage(1406, new RoomLoadProgressC2S()
            {
                LoadProgress = (int)(async.progress >= 0.89f ? 100 : async.progress * 100)
            });
            Debug.Log(string.Format("<color=#ff0000>{0}</color>","加載進度" + async.progress));
            await Task.Delay(500);
            if (!cancelToken.IsCancellationRequested) return;
            SendProgeress();
        }

        /// <summary>
        /// 處理加載進度
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomLoadProgressS2C(BufferEntity response)
        {
            RoomLoadProgressS2C s2cMSG = ProtobufHelper.FromBytes<RoomLoadProgressS2C>(response.proto);

            if (cancelToken != null) cancelToken.Cancel(); //取消所有非同步方法
            //更新介面
            if (s2cMSG.IsBattleStart)
            {

                for (int i = 0; i < s2cMSG.RolesID.Count; i++)
                {
                    playerLoadDIC[s2cMSG.RolesID[i]].transform.Find("Progress").GetComponent<Text>().text = "100%";
                }
                async.allowSceneActivation = true;  //啟動對戰Sence
                Close();//關閉自己
            }
            else
            {
                //如果不能進入戰鬥場景
                for (int i = 0; i < s2cMSG.RolesID.Count; i++)
                {
                    //只顯示進度
                    playerLoadDIC[s2cMSG.RolesID[i]].transform.Find("Progress").GetComponent<Text>().text = $"{s2cMSG.LoadProgress[i]}%";
                }
            }
        }

        /// <summary>
        /// 鎖定英雄
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomLockHeroS2C(BufferEntity response)
        {
            RoomLockHeroS2C s2cMSG = ProtobufHelper.FromBytes<RoomLockHeroS2C>(response.proto);
            rolesDIC[s2cMSG.RolesID].transform.Find("Hero_State").GetComponent<Text>().text = "已鎖定";
            //判斷有沒有鎖定過
            if (isLock) return;
            if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
            {
                isLock = true;
            }
        }

        /// <summary>
        /// 處理選擇英雄，更新頭像
        /// </summary>
        /// <param name="obj"></param>
        private void HeandleRoomSelectHeroS2C(BufferEntity response)
        {
            RoomSelectHeroS2C s2cMSG = ProtobufHelper.FromBytes<RoomSelectHeroS2C>(response.proto);

            //判斷隊伍選擇是否正確
            if (RoomCtrl.Instance.GetTeamId(s2cMSG.RolesID) == -1)
            {
                Debug.LogError("隊伍獲取失敗!!");
                return;
            }
            rolesDIC[s2cMSG.RolesID].transform.Find("Hero_Head").GetComponent<Image>().sprite
               = ResManager.Instance.LoadRoundHead(s2cMSG.HeroID);

            //判斷回傳的是不是自己選的英雄
            if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
            {
                //更新鎖定英雄Id
                lockHeroId = s2cMSG.HeroID;
            }


        }

        /// <summary>
        /// 處理發送聊天訊息
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomSendMsgS2C(BufferEntity response)
        {
            RoomSendMsgS2C s2cMSG = ProtobufHelper.FromBytes<RoomSendMsgS2C>(response.proto);
            //打輸輸入
            chatText.text += $"{RoomCtrl.Instance.GetNickName(s2cMSG.RolesID)} : {s2cMSG.Text}\n";
            //保持顯示最新的聊天訊息
            chatVertical.value = 0;

        }

        /// <summary>
        /// 處理選擇召喚師技能
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomSelectHeroSkillS2C(BufferEntity response)
        {
            RoomSelectHeroSkillS2C s2cMSG = ProtobufHelper.FromBytes<RoomSelectHeroSkillS2C>(response.proto);
            Debug.Log($"收到召喚師技能{s2cMSG.GridID},技能ID{s2cMSG.SkillID}");
            //判斷技能是否正確
            if (s2cMSG.GridID != 0 && s2cMSG.GridID != 1)
            {
                Debug.Log("召喚師技能錯誤");
                return;
            }
            //技能A
            else if (s2cMSG.GridID == 0)
            {
                //設置角色底下的技能ICON
                rolesDIC[s2cMSG.RolesID].transform.Find("Hero_SkillA").GetComponent<Image>().sprite
                    = ResManager.Instance.LoadGeneraSkill(s2cMSG.SkillID);
                if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
                {
                    skillA.sprite = ResManager.Instance.LoadGeneraSkill(s2cMSG.SkillID);
                    skillInfo.gameObject.SetActive(false); //關閉技能選擇面板
                }
            }
            //技能B
            else if (s2cMSG.GridID == 1)
            {
                //設置角色底下的技能ICON
                rolesDIC[s2cMSG.RolesID].transform.Find("Hero_SkillB").GetComponent<Image>().sprite
                    = ResManager.Instance.LoadGeneraSkill(s2cMSG.SkillID);
                if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
                {
                    skillB.sprite = ResManager.Instance.LoadGeneraSkill(s2cMSG.SkillID);
                    skillInfo.gameObject.SetActive(false);//關閉技能選擇面板
                }
            }


        }

        /// <summary>
        /// 處理解散房間
        /// </summary>
        /// <param name="response"></param>
        private void HeandleRoomCloseS2C(BufferEntity response)
        {
            RoomCloseS2C s2cMSG = ProtobufHelper.FromBytes<RoomCloseS2C>(response.proto);
            //取消非同步事件
            cancelToken.Cancel();
            //cancelToken.Dispose();
            //移除緩存的房間的資訊
            RoomCtrl.Instance.RemoveRoomInfo();
            //關閉房間
            Close();
            //打開大廳
            WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            cancelToken.Cancel();
            cancelToken.Dispose();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //設置召喚師技能 初始圖
            skillA.sprite = ResManager.Instance.LoadGeneraSkill(103);
            skillB.sprite = ResManager.Instance.LoadGeneraSkill(106);

        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            NetEvent.Instance.RemoveEventListener(1400, HeandleRoomSelectHeroS2C);
            NetEvent.Instance.RemoveEventListener(1401, HeandleRoomSelectHeroSkillS2C);
            // NetEvent.Instance.RemoveEventListener(1402, HeandleRoomCreateC2S);
            NetEvent.Instance.RemoveEventListener(1403, HeandleRoomCloseS2C);
            NetEvent.Instance.RemoveEventListener(1404, HeandleRoomSendMsgS2C);
            NetEvent.Instance.RemoveEventListener(1405, HeandleRoomLockHeroS2C);
            NetEvent.Instance.RemoveEventListener(1406, HeandleRoomLoadProgressS2C);
            NetEvent.Instance.RemoveEventListener(1407, HeandleRoomToBattleS2C);
        }

        private int lockHeroId;   //鎖定的英雄Id
        private bool isLock = false; //表示是否鎖定英雄
        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "Hero1001":
                        SendSelectHero(buttonList[i], 1001);
                        break;
                    case "Hero1002":
                        SendSelectHero(buttonList[i], 1002);
                        break;
                    case "Hero1003":
                        SendSelectHero(buttonList[i], 1003);
                        break;
                    case "Hero1004":
                        SendSelectHero(buttonList[i], 1004);
                        break;
                    case "Hero1005":
                        SendSelectHero(buttonList[i], 1005);
                        break;
                    case "Lock":
                        buttonList[i].onClick.AddListener(() =>
                        {
                            if (!isLock)
                            {
                                if (lockHeroId == 0) //如果沒有選擇英雄
                                {
                                    Debug.Log("尚未選擇英雄!!");
                                    return;
                                }
                                //isLock = true;
                                BufferFactory.CreateAndSendPackage(1405, new RoomLockHeroC2S()
                                {
                                    HeroID = lockHeroId
                                });
                            }
                        });
                        break;
                    case "SkillA":
                        buttonList[i].onClick.AddListener(SkillAOnClick);
                        break;
                    case "SkillB":
                        buttonList[i].onClick.AddListener(SkillBOnClick);
                        break;
                    case "chengjie":
                        SendSelectSkill(buttonList[i], 101);
                        break;
                    case "chuansong":
                        SendSelectSkill(buttonList[i], 102);
                        break;
                    case "dianran":
                        SendSelectSkill(buttonList[i], 103);
                        break;
                    case "jinghua":
                        SendSelectSkill(buttonList[i], 104);
                        break;
                    case "pnigzhang":
                        SendSelectSkill(buttonList[i], 105);
                        break;
                    case "shanxian":
                        SendSelectSkill(buttonList[i], 106);
                        break;
                    case "xuruo":
                        SendSelectSkill(buttonList[i], 107);
                        break;
                    case "zhiliao":
                        SendSelectSkill(buttonList[i], 108);
                        break;
                }
            }
        }

        private void SendSelectSkill(Button btn, int skillId)
        {
            btn.onClick.AddListener(() =>
            {
                //先判斷英雄是否鎖定了
                if (isLock) return;
                //在判斷SkillA SkillB有沒有重複，尚未實作
                BufferFactory.CreateAndSendPackage(1401, new RoomSelectHeroSkillC2S() { SkillID = skillId, GridID = grid });

            });
        }

        private int grid = 0;
        private void SkillAOnClick()
        {
            grid = 0;
            skillInfo.gameObject.SetActive(true);
        }

        private void SkillBOnClick()
        {
            grid = 1;
            skillInfo.gameObject.SetActive(true);
        }

        private void SendSelectHero(Button btn, int heroId)
        {
            btn.onClick.AddListener(() =>
            {
                if (!isLock)
                {
                    BufferFactory.CreateAndSendPackage(1400, new RoomSelectHeroC2S() { HeroID = heroId });
                    Debug.Log($"發送HeroId:{heroId}");
                    //  lockHeroId = heroId;
                }
            });
        }
    }
}

