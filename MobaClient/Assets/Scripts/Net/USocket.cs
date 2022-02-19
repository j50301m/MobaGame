using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


namespace Game.Net
{
    /// <summary>
    /// 提供 Socket發送、接收的方法
    /// </summary>
    public class USocket
    {
        private UdpClient udpClient;
        private string ip="192.168.1.4"; //Server IP
        private int port = 8899; //Server port

        public static IPEndPoint server;
        public static UClient local; //Client代理 :完成發送、處理報文的邏輯、保證發送的順序

        public USocket(Action<BufferEntity> dispatchNetEvent)
        {
            udpClient = new UdpClient(0);
            server = new IPEndPoint(IPAddress.Parse(ip), port);
            local = new UClient(this, server, 0, 0, 0, dispatchNetEvent);

            ReceiveTask(); //接收消息

        }

        private ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        /// <summary>
        /// 接收報文的方法
        /// </summary>
        public async void ReceiveTask()
        {
            //持續接收，直到斷線
            while (udpClient != null)
            {
                try
                {
                    UdpReceiveResult result= await udpClient.ReceiveAsync();
                    Debug.Log("接收到來自Server的消息");
                    awaitHandle.Enqueue(result);
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                }
                
            }


        }

        /// <summary>
        /// 發送報文的方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endPoint"></param>
        public async void Send(byte[] data, IPEndPoint endPoint)
        {
            if (udpClient != null)
            {
                try
                {
                    //int length=await udpClient.SendAsync(data, data.Length,ip,port);
                   server = endPoint;
                    ip = endPoint.Address.ToString();
                    port = endPoint.Port;
                   int length = await udpClient.SendAsync(data, data.Length,endPoint);
                }
                catch (Exception e)
                {
                    Debug.LogError($"發送異常:{e.Message}");
                }

            }
        }

        /// <summary>
        /// 發送ACK報文,解包後馬上調用
        /// </summary>
        /// <param name="bufferEntity"></param>
        public  void SendAck(BufferEntity bufferEntity)
        {
            Send(bufferEntity.buffer, server);
        }

        /// <summary>
        /// Update裡面調用處理邏輯
        /// </summary>
        public void Handle()
        {
            if (awaitHandle.Count > 0)
            {
                if (awaitHandle.TryDequeue(out UdpReceiveResult data))
                {
                    //反序列化
                    BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
                    if (bufferEntity.isFull)
                    {
                        Debug.Log($"處理消息,id:{bufferEntity.messageID},序號:{bufferEntity.sn}");
                        //處理業務邏輯
                        local.Handle(bufferEntity);
                    }

                }

            }

        }

        /// <summary>
        /// 關閉udpClient
        /// </summary>
        public void Close()
        {
            if (local != null) local = null;
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
            }

        }
    }
}

