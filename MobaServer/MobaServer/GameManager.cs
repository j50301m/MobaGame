using MobaServer.GameModule;
using MobaServer.Net;
using System;

namespace MobaServer
{
    class GameManager
    {

        
        static void Main(string[] args)
        {
            Console.WriteLine("啟動Server");
            GameMoudleInit(); //初始化遊戲模組
            NetSystemInit();//初始化網路系統
            Console.ReadLine();
        }

        public static USocket uSocket;
        static void NetSystemInit()
        {
            uSocket = new USocket(DispatchNetEvent);
            Debug.Log("網路系統初始化完成");
        }

        static void DispatchNetEvent(BufferEntity buffer)
        {
            //進行報文分發
            NetEvent.Instance.Dispatch(buffer.messageID, buffer);
        }

        static void GameMoudleInit()
        {
            UserModule.Instance.Init(); //用戶模組
            RolesModule.Instance.Init(); //角色模組
            LobbyModule.Instance.Init();//大廳模組
            RoomModule.Instance.Init(); //房間模組
            BattleModule.Instance.Init();//戰鬥模組
        }
    }
}
