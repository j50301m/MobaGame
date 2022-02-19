using MobaServer.Player;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
   public class UClient
    {
        private USocket uSocket;
        public  IPEndPoint endPoint;
        private int sendSN;
        private int handleSN;
        public int session;
        private Action<BufferEntity> dispatchNetEvent;

        public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int session, Action<BufferEntity> dispatchEvent)
        {
            this.uSocket = uSocket;
            this.endPoint = endPoint;
            this.sendSN = sendSN;
            this.handleSN = handleSN;
            this.session = session;
            this.dispatchNetEvent = dispatchEvent;

            CheckTimeOut();
        }

        public  bool isConnect=true;

        private int overTime = 150;
        private async void CheckTimeOut()
        {
            await Task.Delay(overTime);
            foreach (var package in sendPackage.Values)
            {
                if (package.rescurCount>= 10)
                {
                    Debug.LogError($"重發10次還是失敗，協議ID:{package.messageID}");
                    uSocket.RemoveClient(session);
                    return;
                }
                if (TimeHelper.Now() - package.time >= (package.rescurCount + 1) * overTime)
                {
                    package.rescurCount += 1;
                    Debug.Log($"TimeOut重發 序號:{package.sn}");
                    uSocket.Send(package.buffer, endPoint);
                }
            }
            CheckTimeOut();
        }

        internal void Handle(BufferEntity buffer)
        {
            //移除掉已經發送的BufferEntity
            switch (buffer.messageType)
            {
                case 0://ACK報文
                    BufferEntity buff;
                    if (sendPackage.TryRemove(buffer.sn, out buff))
                    {
                        Debug.Log($"報文以確認,序號:{buffer.sn}");
                    }
                    else
                    {
                        Debug.Log($"要確認的報文不存在，序號{buffer.sn}");
                    }
                    break;

                case 1:   //業務報文
                    //if (buffer.sn != 1)
                    //{
                    //    return;   //測試Client TimeOut重發
                    //}
                    Debug.Log("收到的是業務報文");
                    //回送ACK報文
                    BufferEntity ackPackage = new BufferEntity(buffer);
                    uSocket.SendAck(ackPackage, endPoint);

                    HandleLogicPackage(buffer);
                    break;
                default:
                    break;
            }
        }

        //暫存 錯序的報文
        private ConcurrentDictionary<int, BufferEntity> waitHandle = new ConcurrentDictionary<int, BufferEntity>();
        //業務報文 邏輯處理
        private void HandleLogicPackage(BufferEntity buffer)
        {
            //收到過去的報文
            if (buffer.sn <= handleSN)
            {
                Debug.Log($"此報文以經處理過，直接丟棄 序號{buffer.sn}");
                return;
            }
            //收到未來的報文
            if (buffer.sn - handleSN > 1)
            {
                if (waitHandle.TryAdd(buffer.sn,buffer))
                {
                    Debug.Log($"將錯序的報文先暫存 序號:{buffer.sn}");
                }
                return;
            }
            
            //收到當前的報文
            handleSN = buffer.sn;
            if (dispatchNetEvent != null)
            {
                Debug.Log("分發消息到各module");
                dispatchNetEvent(buffer);
            }
            BufferEntity nextBuffer;
            if (waitHandle.TryRemove(handleSN + 1,out nextBuffer))
            {
                HandleLogicPackage(nextBuffer);
            }
        }

        private ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>();
        //發送的方法
        public void  Send(BufferEntity package)
        {
            if (isConnect==false)   return;

            package.time = TimeHelper.Now();
            sendSN += 1;
            package.sn = sendSN;

            //序列化
            package.Encoder(false);
            uSocket.Send(package.buffer, endPoint);
            //判斷這包是否已經發送
            if (session != 0)
            {
                sendPackage.TryAdd(package.sn, package);
            }
        }
        internal void Close()
        {
            isConnect = false;
            //移除玩家實體
            if(PlayerManager.GetPlayerEntityFromSession(session)!=null) PlayerManager.RemovePlayerEntityFromSession(session);

            //throw new NotImplementedException();
        }
    }
}
