using System;
using System.Collections;
using System.Collections.Generic;
using Game.Net;
using ProtoMsg;
using UnityEngine;
namespace Battle
{
    /// <summary>
    /// 戰鬥監聽器
    /// </summary>
   public class BattleListener : Singleton<BattleListener>
    {
        //初始化 監聽網路消息
        public void Init()
        {
            NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputS2C);
        }

        private Queue<BattleUserInputS2C> awaitHandle;
        /// <summary>
        /// 處理戰鬥輸入
        /// </summary>
        /// <param name="response"></param>
        private void HandleBattleUserInputS2C(BufferEntity response)
        {
            BattleUserInputS2C s2cMSG = ProtobufHelper.FromBytes<BattleUserInputS2C>(response.proto);
            awaitHandle.Enqueue(s2cMSG);
        }

        //移除監聽
        public void RemoveListener()
        {
            NetEvent.Instance.RemoveEventListener(1500, HandleBattleUserInputS2C);
        }

        public void Relese()
        {
            this.RemoveListener();
            awaitHandle.Clear();
        }

        //調度/播放網路事件
        public void PlayerFrame(Action<BattleUserInputS2C> action)
        {
            if (action != null && awaitHandle.Count > 0)
            {
                action(awaitHandle.Dequeue());
            }
        }
    }
}

