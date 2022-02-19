using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Game.Net
{
    public class BufferEntity
    {
        public int rescurCount=0; //重發次數 (工程內部使用，並非業務數據)
        public IPEndPoint endPoint; //發送目標終端
        public byte[] buffer;//最終要發送 或收到的數據

        #region 封包成員
        public int protoSize;
        public int session; //對話ID
        public int sn; //序號
        public int moduleID;// 模組ID
        public long time;
        public int messageType;// 協議類型
        public int messageID;//協議類型
        public byte[] proto; //業務報文
        #endregion

        /// <summary>
        ///建構發送報文
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="session"></param>
        /// <param name="sn"></param>
        /// <param name="moduleID"></param>
        /// <param name="messageType"></param>
        /// <param name="messageID"></param>
        /// <param name="proto"></param>
        public BufferEntity(IPEndPoint endPoint, int session, int sn, int moduleID, int messageType, int messageID, byte[] proto)
        {
            protoSize = proto.Length;
            this.endPoint = endPoint;
            this.session = session;
            this.sn = sn;
            this.moduleID = moduleID;
            this.messageType = messageType;
            this.messageID = messageID;
            this.proto = proto;
        }

        /// <summary>
        /// 報文編碼方法
        /// </summary>
        /// <param name="isAck"></param>
        /// <returns></returns>
        public byte[] Encoder(bool isAck)
        {
            if (isAck)
            {
                protoSize = 0; //ACK包體大小=0
            }
            byte[] data = new byte[32 + protoSize]; //int =4字節 ,long=8字節

            byte[] _length=BitConverter.GetBytes(protoSize);
            byte[] _session = BitConverter.GetBytes(session);
            byte[] _sn = BitConverter.GetBytes(sn);
            byte[] _moduleID = BitConverter.GetBytes(moduleID);
            byte[] _time = BitConverter.GetBytes(time);
            byte[] _messageType = BitConverter.GetBytes(messageType);
            byte[] _messageID = BitConverter.GetBytes(messageID);

            //將字節Array寫到data
            Array.Copy(_length, 0, data, 0, 4);
            Array.Copy(_session, 0, data, 4, 4);
            Array.Copy(_sn, 0, data, 8, 4);
            Array.Copy(_moduleID, 0, data, 12, 4);
            Array.Copy(_time, 0, data, 16, 8);
            Array.Copy(_messageType, 0, data, 24, 4);
            Array.Copy(_messageID, 0, data, 28, 4);

            if (!isAck)
            {
                //加入業務數據
                Array.Copy(proto, 0, data, 32, proto.Length);
            }
            buffer = data;
            return data;

        }

        /// <summary>
        /// 建構 收到報文的實體
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="buffer"></param>
        public BufferEntity(IPEndPoint endPoint,byte[] buffer)
        {
            this.endPoint = endPoint;
            this.buffer = buffer;
            Decode();
        }

        public  bool isFull; //判斷包體完整性
        //將報文反序列
        private void Decode()
        {
            //判斷傳進來的報文是否大於4個字節 如果小於就沒辦法解析
            if (buffer.Length >= 4)
            {
                //byte轉成 int 或 long
                protoSize = BitConverter.ToInt32(buffer, 0);

                if (buffer.Length == protoSize + 32)
                {
                    isFull = true;
                }
            }
            else
            {
                isFull = false;
                return;
            }

            session = BitConverter.ToInt32(buffer, 4);
            sn = BitConverter.ToInt32(buffer, 8);
            moduleID = BitConverter.ToInt32(buffer, 12);
            time = BitConverter.ToInt64(buffer, 16);
            messageType = BitConverter.ToInt32(buffer, 24);
            messageID = BitConverter.ToInt32(buffer, 28);

            //判斷是否為ACK報文
            if (messageType == 0)
            {

            }
            else
            {
                proto = new byte[protoSize];
                Array.Copy(buffer, 32, proto, 0, protoSize);
            }
       
        }

        /// <summary>
        /// 建立ACK報文的實體
        /// </summary>
        /// <param name="package">收到的報文實體(已經Decoded)</param>
        public BufferEntity(BufferEntity package)
        {
            protoSize = 0;
            this.endPoint = package.endPoint;
            this.session = package.session;
            this.sn = package.sn;
            this.moduleID = package.moduleID;
            this.time = 0;
            this.messageType =0;
            this.messageID = package.messageID;
            buffer = Encoder(true);
        }
    }
}

