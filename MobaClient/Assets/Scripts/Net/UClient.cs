using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net
{
    /// <summary>
    /// 網路客戶端代理
    /// </summary>
    public class UClient
    {
        public IPEndPoint endPoint;
        public int sessionID; //會話ID
        public int sendSN=0; //發送序號
        public int handleSN = 0; //接收序號

        private USocket uSocket;
        private Action<BufferEntity> handleAction; //處理報文的函數，將報文分配給各遊戲Moudle;

        public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int sessionID, Action<BufferEntity> dispatchNetEvent)
        {
            this.uSocket = uSocket;
            this.endPoint = endPoint;
            this.sendSN = sendSN;
            this.handleSN = handleSN;
            this.sessionID = sessionID;
            handleAction = dispatchNetEvent;

            CheckOutTime();
        }

        //處理消息 : 按照報文序號 順序處理報文，如果收到超過當前順序+1的報文 先進行暫存 
        public void Handle(BufferEntity buffer)
        {
            if (this.sessionID == 0 && buffer.session != 0)
            {
                this.sessionID = buffer.session;
                Debug.Log($"成功與Server建立連線 sessionId:{this.sessionID}");

            }
            switch (buffer.messageType)
            {
                case 0://ACK確認報文
                    BufferEntity bufferEntity;
                    if (sendPackage.TryRemove(buffer.sn, out bufferEntity))
                    {
                        Debug.Log($"收到確認報文 序號={buffer.sn}");
                    }
                    break;
                case 1: //業務報文
                    Debug.Log("收到業務報文");
                    BufferEntity ackPackage = new BufferEntity(buffer);
                    //先告訴Server已經收到這個報文
                    uSocket.SendAck(ackPackage);
                    //處理業務報文
                    HandleLogicPackage(buffer);
                    break;

                default:

                    break;
            }
        }


        private ConcurrentDictionary<int, BufferEntity> waitHandle = new ConcurrentDictionary<int, BufferEntity>(); //暫存收到的錯序報文
        //處理 業務報文
        private void HandleLogicPackage(BufferEntity buffer)
        {
            //已經收到過 就返回
            if (buffer.sn <= handleSN) return;
            //收到錯序報文
            if (buffer.sn-handleSN>1)
            {
                if (waitHandle.TryAdd(buffer.sn, buffer))
                {
                    Debug.Log($"收到錯序報文:{buffer.sn}");
                }
                return;
            }
            //更新已經當前處理到的報文序號
            handleSN = buffer.sn;
            if (handleAction != null)
            {
                handleAction(buffer);
            }
            BufferEntity nextBuffer;
            //判斷是否已經收到過下一個報文(錯序)
            if(waitHandle.TryRemove(handleSN+1,out nextBuffer))
            {
                //處理緩衝區的錯序報文
                HandleLogicPackage(nextBuffer);
            }
        }
              
        
        private ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>(); //暫存已經發送,但未收到回傳ACK的報文
        /// <summary>
        /// 客戶端發送報文的方法
        /// </summary>
        /// <param name="package"></param>
        public  void Send(BufferEntity package)
        {
            package.time = TimeHelper.Now();
            sendSN += 1; //每發一次就++
            package.sn = sendSN;

            package.Encoder(false);
            //先判斷是否與Server連接，sessionID=0表示第一次發送，!=0表示已經和Server建立過連接
            if (sessionID != 0)
            {
                //暫存起來
                sendPackage.TryAdd(sendSN,package);
            }
            else
            {
                //尚未與Server連接，不需要暫存直接發送
            }
            uSocket.Send(package.buffer, endPoint);
        }

        private int overTime = 150;
        /// <summary>
        /// 檢測TimeOut的方法
        /// </summary>
        public async void CheckOutTime()
        {
            await Task.Delay(overTime);
            foreach (var package in sendPackage.Values)
            {
                //判斷是否超過最大發送次數 
                if (package.rescurCount>= 10)
                {
                    Debug.Log($"重發10次還是失敗關閉連線{package.session} ");
                    //關閉連線
                    OnDisconnect();
                    return;
                }
                //未達最大重發次數
                if (TimeHelper .Now()- package.time >= (package.rescurCount+1)*overTime)
                {
                    package.rescurCount += 1;
                    Debug.Log($"逾時重發，次數{package.rescurCount} ");
                    uSocket.Send(package.buffer, endPoint);
                }
            }

            CheckOutTime();
        }

        public void OnDisconnect()
        {
            handleAction = null;
            uSocket.Close();

        }
    }
}

