using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;

namespace MobaServer.Net
{
    public class BufferFactory
    {
        enum MessageType
        {
            ACK = 0, //確認報文
            Logic = 1//業務報文
        }

        /// <summary>
        /// 創建並發送報文
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BufferEntity CreateAndSendPackage(UClient uClient, int messageID, IMessage message)
        {
            if (uClient.isConnect)
            {
                //打印message報文出來
                Debug.Log(messageID, message);
                BufferEntity bufferEntity = new BufferEntity(uClient.endPoint, uClient.session, 0, 0, MessageType.Logic.GetHashCode(),
                    messageID, ProtobufHelper.ToBytes(message));
                uClient.Send(bufferEntity);
                return bufferEntity;
            }
            return null;

        }

        internal static BufferEntity CreateAndSendPackage(BufferEntity request, IMessage message)
        {
            UClient client = GameManager.uSocket.GetClient(request.session);
            return CreateAndSendPackage(client, request.messageID, message);
        }
    }
}

