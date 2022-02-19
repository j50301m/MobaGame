using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobaServer.Net
{
    public class USocket
    {
        private UdpClient socket; //socket 通信
        private string ip = "192.168.1.3";
        private int port = 8899;
        private Action<BufferEntity> dispatchEvent;

        //初始化
        public USocket(Action<BufferEntity> dispatchEvent)
        {
            this.dispatchEvent = dispatchEvent;
            socket = new UdpClient(port);

            Receive();

            Task.Run(Handle, ct.Token);
        }

        
        //發送消息
        public async void Send(byte[] data, IPEndPoint endPoint)
        {
            if (socket!=null)
            {
                try
                {
                    int length = await socket.SendAsync(data, data.Length, endPoint);
                    //檢查是否發送完整
                    if (data.Length == length)
                    {
                      //完整的發送
                    }
                }
                catch (Exception e )
                {
                    Debug.LogError("發送異常:" + e.Message);
                    Close();
                }

            }
        
        }

        //發送ACK報文
        public async void SendAck(BufferEntity ackPackage,IPEndPoint endPoint)
        {
            Debug.Log($"發送ACK報文給 Client IP: {endPoint.Address} ,port:{endPoint.Port}");
             Send(ackPackage.buffer, endPoint);
        }

        private ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        //接收消息
        public async void Receive()
        {
            if (socket != null)
            {
                try
                {
                    UdpReceiveResult result = await socket.ReceiveAsync();
                    Debug.Log("接收到了客戶端的消息");
                    awaitHandle.Enqueue(result);
                    Receive();
                }
                catch(Exception e)
                {
                    Debug.LogError("接收異常:"+e.Message);
                    Close();
                }
            }
        }
        private CancellationTokenSource ct = new CancellationTokenSource();
        private int sessionID=1000;
        //處理消息
        private async Task  Handle()
        {
            while (!ct.IsCancellationRequested)
            {
                //判斷是否有東西
                if (awaitHandle.Count > 0)
                {
                    UdpReceiveResult data;
                    if (awaitHandle.TryDequeue(out data))
                    {
                        BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
                        if (bufferEntity.isFull)
                        {
                            //檢查對話ID是否為0，如果=0代表尚未與Server建立連接
                            if (bufferEntity.session == 0)
                            {
                                //分配會話Id 給客戶端
                                sessionID += 1;
                                bufferEntity.session = sessionID;
                                CreateUClient(bufferEntity);
                                Debug.Log($"創建客戶端完成,會話id:{sessionID}");
                            }

                            UClient targetClient;
                            //獲取客戶端
                            if (clients.TryGetValue(bufferEntity.session, out targetClient))
                            {
                                targetClient.Handle(bufferEntity);
                            }
                        }
                    }
                }

            }

        
        }


        //關閉Socket
        private void Close()
        {
            //發送信號，取消尚未執行任務
            ct.Cancel(); 
            //所有客戶端移除
            foreach (var client in clients.Values)
            {
                client.Close();
            }
            clients.Clear();
            
            if (socket!=null)
            {
                socket.Close();
                socket = null;
            }
            if (dispatchEvent != null)
            {
                dispatchEvent = null;
            }

        }

        private ConcurrentDictionary<int, UClient> clients = new ConcurrentDictionary<int, UClient>();
        //與客戶端建立連接
        private void CreateUClient(BufferEntity buffer)
        {
            UClient client;
            if (!clients.TryGetValue(buffer.session, out client))
            {
                client = new UClient(this, buffer.endPoint, 0, 0, buffer.session, dispatchEvent);
                clients.TryAdd(buffer.session, client);
            }
        }
        //移除客戶端
        public void RemoveClient(int sessionId)
        {
            UClient client;
            if (clients.TryRemove(sessionId,out client))
            {
                client.Close();
                client = null;
            }
        }

        //獲取客戶端
        public UClient GetClient(int sessionId)
        {
            UClient client;
            if (clients.TryGetValue(sessionId, out client))
            {
                return client;
            }
            return null;
        }
    }
}
