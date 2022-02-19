using MobaServer.Match;
using MobaServer.Room;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Player
{
    public class PlayerEntity
    {
        public int session; //會話ID
        public UserInfo userInfo;
        public RolesInfo rolesInfo;

        //匹配的信息
        public MatchEntity matchEntity { get; internal set; }
        //房間的信息
        public RoomEntity roomEntity { get; internal set; }
        //陣營ID
        public int TeamId;

        /// <summary>
        /// 用戶銷毀
        /// </summary>
        public void Destroy()
        {
            Debug.Log("用戶斷開連接");
        }
    }
}
