using Game.Ctrl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BatteManager : MonoBehaviour
    {
        #region 參數區
        //BattleSceneConfig
        //10個英雄的初始位置
        public Vector3[] spawnPosition = new Vector3[10] { 
        //隊伍A的位置 0-4
        new Vector3(-6.52f,0,-8.96f),
        new Vector3(-2.26f,0,-3.71f),
        new Vector3(-6.71f,0,-4.01f ),
        new Vector3(-4.28f,0,-5.89f ),
        new Vector3(-2.02f,0,-8.23f), 
        //隊伍B的位置 5-9
        new Vector3(-95.198f,0,-96.542f),
        new Vector3(-99.892f,0,-101.4f ),
        new Vector3(-95.432f,0,-101.49f),
        new Vector3(-97.692f,0,-99.409f),
        new Vector3(-99.7443f,0,-96.884f), };
        //10個英雄初始的角度
        public Vector3[] spawnRotation = new Vector3[10] {
        //隊伍A的角度 0-4
        new Vector3(0,242.49f,0),
        new Vector3(0,-122.251f,0),
        new Vector3( 0,-152.659f,0 ),
        new Vector3(0,230.56f,0),
        new Vector3( 0,-149.089f,0 ),
        //隊伍B的角度 5-9
        new Vector3(0,67.403f,0 ),
        new Vector3(0,-297.338f,0 ),
        new Vector3( 0,-327.746f,0),
        new Vector3(0,55.473f,0),
        new Vector3(0,-324.176f,0), };
        //野怪的位置

        Dictionary<int, PlayerCtrl> PlayerCtrlDIC; //玩家控制器字典 key=rolesId 

        #endregion
        private void Awake()
        {
            PlayerCtrlDIC = new Dictionary<int, PlayerCtrl>();
            //取得房間裡所有PlayerInfo
            foreach (var playerInfo in RoomCtrl.Instance.GetPlayernfos())
            {
                //創建Player模型
                GameObject hero = ResManager.Instance.LoadHeroModel(playerInfo.HeroID);
                //設置位置
                hero.transform.position = spawnPosition[playerInfo.PosID];
                hero.transform.eulerAngles = spawnRotation[playerInfo.PosID];
                //加載玩家控制器組件 到模型身上
                PlayerCtrl playerCtrl = hero.AddComponent<PlayerCtrl>();
                //保存
                if (!PlayerCtrlDIC.ContainsKey(playerInfo.RolesInfo.RolesID))PlayerCtrlDIC[playerInfo.RolesInfo.RolesID] = playerCtrl; //保存玩家控制器
                RoomCtrl.Instance.SavePlayerObject(playerInfo.RolesInfo.RolesID, hero); //保存模型
                //初始化 每個控制器
                playerCtrl.Init(playerInfo);

            }


            //添加控制器
            //保存 角色物體
            //每個角色 都要初始化
            //家載戰鬥UI
            //輸入管理器初始化
        }

        private void OnDestroy()
        {
            //移除所有玩家模型、屬性相關資料
            RoomCtrl.Instance.ClearBattleData();
        }
    }
}

