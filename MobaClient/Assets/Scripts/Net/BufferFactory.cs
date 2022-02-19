using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net
{
    public class BufferFactory
    {
        enum MessageType
        {
            ACK=0, //確認報文
            Logic=1//業務報文
        }

        /// <summary>
        /// 創建並發送報文
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BufferEntity CreateAndSendPackage(int messageID, IMessage message)
        {
            //Debug.Log($"報文ID:{messageID}\n包體:{JsonHelper.SerializeObject(message)}");
            JsonHelper.Log(messageID, message);
            BufferEntity buffer = new BufferEntity(USocket.local.endPoint, USocket.local.sessionID, 0, 0, MessageType.Logic.GetHashCode(),
                messageID, ProtobufHelper.ToBytes(message));
            USocket.local.Send(buffer);
            return buffer;
        }

    }
}

