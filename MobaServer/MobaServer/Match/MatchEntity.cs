using MobaServer.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Match
{
    /// <summary>
    /// 每一個隊伍的實體
    /// </summary>
    public class MatchEntity
    {
        public PlayerEntity player; //玩家實體
        public int teamId; //隊伍ID
    }
}
