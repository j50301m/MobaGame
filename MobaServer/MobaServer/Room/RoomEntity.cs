using Google.Protobuf;
using MobaServer.Net;
using MobaServer.Player;
using ProtoMsg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MobaServer.Room
{
    public class RoomEntity
    {
        #region 參數區
        public int roomID; //房間ID
        public int selectHeroTime=20000; //選擇英雄 時間
        public RoomInfo roomInfo; //房間訊息
        private ConcurrentDictionary<int, PlayerInfo> playerList = new ConcurrentDictionary<int, PlayerInfo>();//房間中玩家列表
        private ConcurrentDictionary<int, UClient> clientList = new ConcurrentDictionary<int, UClient>();  //UClient 客戶端列表
        private int lockCount; //鎖定次數
        ConcurrentDictionary<int, int> playerProgress = new ConcurrentDictionary<int, int>(); //每個玩家的加載進度
        private bool isLoadComplete = false; //加載是否完成
        private int skillA_ID= 103; //預設召喚師技能
        private int skillB_ID = 106;//預設召喚師技能
        private Timer timer;
        private int timerDelay = 100; //100ms 廣播一次對戰資訊

        private ConcurrentQueue<BattleUserInputS2C> userInputQueue = new ConcurrentQueue<BattleUserInputS2C>();
        /// <summary>
        /// 處理用戶輸入
        /// </summary>
        /// <param name="c2sMSG"></param>
        internal void HandleBattleUserInputC2S(BattleUserInputC2S c2sMSG)
        {
            BattleUserInputS2C s2cMSG = new BattleUserInputS2C();
            s2cMSG.CMD = c2sMSG;
            userInputQueue.Enqueue(s2cMSG); //將要廣播的資訊放到對列中 RoomEntity會自動撥放

            
        }

        #endregion
        //建構子
        public RoomEntity(RoomInfo roomInfo)
        {
            this.roomID = roomInfo.ID;
            this.roomInfo = roomInfo;
            Init();
            CreateBattle();
            SetTimer(timerDelay);
            timer.Start(); //開始廣播戰鬥資訊
        }
        //初始化
        private void Init()
        {
            //把玩家和client位置加到List裡
            for (int i = 0; i < roomInfo.TeamA.count; i++)
            {
                PlayerInfo playerInfo = new PlayerInfo();

                playerInfo.RolesInfo = roomInfo.TeamA[i];
                playerInfo.SkillA = skillA_ID; //召喚師技能默認點燃
                playerInfo.SkillB = skillB_ID;//召喚師技能默認閃現
                playerInfo.HeroID = 0;//角色默認未選擇
                playerInfo.TeamID = 0; //默認藍方
                playerInfo.PosID = i;//初始/重生位置

                //加到List
                playerList.TryAdd(playerInfo.RolesInfo.RolesID, playerInfo);

                //獲取玩家當前client
                UClient client = GameManager.uSocket.GetClient(PlayerManager.GetPlayerEntityFromRoles(playerInfo.RolesInfo.RolesID).session);
                //緩存每一個客戶端
                clientList.TryAdd(playerInfo.RolesInfo.RolesID, client);
                //緩存每一個玩家進度
                playerProgress.TryAdd(playerInfo.RolesInfo.RolesID, 0);
            }
            for (int i = 0; i < roomInfo.TeamB.count; i++)
            {
                PlayerInfo playerInfo = new PlayerInfo();

                playerInfo.RolesInfo = roomInfo.TeamB[i];
                playerInfo.SkillA = skillA_ID; //召喚師技能默認點燃
                playerInfo.SkillB = skillB_ID;//召喚師技能默認閃現
                playerInfo.HeroID = 0;//角色默認未選擇
                playerInfo.TeamID = 1; //默認紫方
                playerInfo.PosID = i + 5;//初始/重生位置

                //加到List
                playerList.TryAdd(playerInfo.RolesInfo.RolesID, playerInfo);

                //獲取玩家當前client
                UClient client = GameManager.uSocket.GetClient(PlayerManager.GetPlayerEntityFromRoles(playerInfo.RolesInfo.RolesID).session);
                //緩存每一個客戶端
                clientList.TryAdd(playerInfo.RolesInfo.RolesID, client);
                //緩存每一個玩家進度
                playerProgress.TryAdd(playerInfo.RolesInfo.RolesID, 0);
            }
        }
        //設定戰鬥資訊的定時廣播
        private void SetTimer(int delayTime)
        {
            //定時廣播戰鬥資訊
            timer = new Timer(delayTime);
            timer.Elapsed += BroadcastBattleInfo;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        //進入Battle
        private async void CreateBattle()
        {
            await Task.Delay(selectHeroTime);

            //判斷所有人都按下確定
            if(lockCount != roomInfo.TeamA.count + roomInfo.TeamB.count)
            {
                RoomCloseS2C s2cMSG = new RoomCloseS2C();
                Broadcast(1403, s2cMSG);
                //釋放掉房間
                RoomManager.Instance.Remove(roomID);
                return;
            }
            else //所有人都選好了英雄
            { 
                //加載戰鬥
                RoomToBattleS2C s2cMSG = new RoomToBattleS2C();
                foreach (var rolesID in playerList.Keys)
                {
                    //UClient client = GameManager.uSocket.GetClient(PlayerManager.GetPlayerEntityFromRoles(rolesID).session);
                    // clientList.TryAdd(rolesID, client);
                    s2cMSG.PlayerList.Add(playerList[rolesID]);
                }
                //給全部人通知
                Broadcast(1407, s2cMSG);
            }

        }

        #region 群發消息
        /// <summary>
        ///給全部player發消息
        /// </summary>
        public void Broadcast(int messageId, IMessage s2cMSG)
        {
            foreach (var client in clientList.Values)
            {
                BufferFactory.CreateAndSendPackage(client, messageId, s2cMSG);
            }
        }
        /// <summary>
        /// 給隊伍發消息
        /// </summary>
        public void Broadcast(int teamId,int messageId, IMessage s2cMSG)
        {
            if (teamId == 0) //隊伍A
            {
                for (int i = 0; i < roomInfo.TeamA.count; i++)
                {
                    UClient client;
                    if (clientList.TryGetValue(roomInfo.TeamA[i].RolesID, out client))
                    {
                        BufferFactory.CreateAndSendPackage(client, messageId, s2cMSG);
                    }
                }
            }
            else //隊伍B
            {
                for (int i = 0; i < roomInfo.TeamB.count; i++)
                {
                    UClient client;
                    if (clientList.TryGetValue(roomInfo.TeamB[i].RolesID, out client))
                    {
                        BufferFactory.CreateAndSendPackage(client, messageId, s2cMSG);
                    }
                }
            }
        }
        /// <summary>
        /// 廣播戰鬥資訊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BroadcastBattleInfo(object sender, ElapsedEventArgs e)
        {
            BattleUserInputS2C s2cMSG;
            //判斷對列內是否有東西
            if (userInputQueue.Count <= 0) return;
            //嘗試拿取元素
            if (!userInputQueue.TryPeek(out s2cMSG)) Debug.LogError("RoomEntity 廣播戰鬥資訊失敗");
            else if (s2cMSG != null)
            {
                userInputQueue.TryDequeue(out s2cMSG);
                Broadcast(1500, s2cMSG);
            }
        }
        #endregion

        /// <summary>
        /// 鎖定英雄
        /// </summary>
        /// <param name="rolesId"></param>
        /// <param name="heroId"></param>
        public void LockHero(int rolesId, int heroId)
        {
            lockCount += 1;
            playerList[rolesId].HeroID = heroId;
        }

        /// <summary>
        /// 更新召喚師技能
        /// </summary>
        /// <param name="rolesId">角色Id</param>
        /// <param name="skillId">技能Id</param>
        /// <param name="gridID">召喚師技能編號</param>
        public PlayerInfo UpdateSkill(int rolesId, int skillId,int gridID)
        {
            
            if (gridID == 0) //第一個技能
            {
                //判斷這招是不是選過
                if (playerList[rolesId].SkillB != skillId) playerList[rolesId].SkillA = skillId;
                else
                {
                    playerList[rolesId].SkillB = playerList[rolesId].SkillA;
                    playerList[rolesId].SkillA = skillId;
                } 
            }
            if(gridID==1) //第二個技能
            {
                //判斷這招是不是選過
                if (playerList[rolesId].SkillA != skillId) playerList[rolesId].SkillB = skillId;
                else
                {
                    playerList[rolesId].SkillA = playerList[rolesId].SkillB;
                    playerList[rolesId].SkillB = skillId;
                }
            }
            return playerList[rolesId];
        }
        
        /// <summary>
        /// 更新載入狀況
        /// </summary>
        /// <param name="rolesId"></param>
        /// <param name="progress"></param>
        public bool UpdateLoadProgress(int rolesId, int progress)
        {
            //判斷是否大家都加載完成了
            if (isLoadComplete)
            {
                //告訴客戶端 完成加載
                RoomLoadProgressS2C s2cMSG = new RoomLoadProgressS2C();
                s2cMSG.IsBattleStart = true;
                foreach (var item in playerProgress.Keys)
                {
                    s2cMSG.RolesID.Add(item);
                    s2cMSG.LoadProgress.Add(playerProgress[item]);
                }

                Broadcast(1406, s2cMSG);
                return true;
            }
            else
            {
                playerProgress[rolesId] = progress;
                foreach (var value in playerProgress.Values)
                {
                    if (value <100)
                    {
                        isLoadComplete = false;
                        return false;
                    }
                }
                isLoadComplete = true;
                UpdateLoadProgress(rolesId, progress);
            }
            return true;
        }

        /// <summary>
        ///獲取玩家的進度
        /// </summary>
        public void GetLoadProgress(ref RoomLoadProgressS2C s2cMSG)
        {
            foreach (var item in playerProgress.Keys)
            {
                s2cMSG.RolesID.Add(item);
                s2cMSG.LoadProgress.Add(playerProgress[item]);
            }
        }

        /// <summary>
        /// 銷毀房間時要做的事
        /// </summary>
        public void Close()
        {
            timer.Close(); //釋放Timer
            lockCount = 0;
            isLoadComplete = false;
            userInputQueue = null;
            playerList.Clear();
            clientList.Clear();
        }
    }
}
