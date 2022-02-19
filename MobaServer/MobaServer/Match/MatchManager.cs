using MobaServer.Room;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Match
{
    class MatchManager:Singleton<MatchManager>
    {
        //每個隊伍,多少人可以開打 1V1 2V2 5V5
        public int number = 1;

        //key =teamId ,value=matchEntity
        ConcurrentDictionary<int, MatchEntity> pool = new ConcurrentDictionary<int, MatchEntity>();

        //添加匹配池
        public bool Add(MatchEntity matchEntity)
        {
            if (!pool.TryAdd(matchEntity.teamId, matchEntity))
            {
                Debug.Log($"加入匹配池失敗! ID:{matchEntity.teamId}");
                return false;
            }
            //判斷是否
            if (pool.Count >= number * 2)
            {
                //匹配完成事件
                MatchCompleteEvent();
            }
            return true;
        }
        //移除匹配池
        public bool Remove(MatchEntity matchEntity)
        {
            MatchEntity entity;
            return pool.TryRemove(matchEntity.teamId, out entity);
        }

        /// <summary>
        /// 匹配完成事件
        /// </summary>
        private void MatchCompleteEvent()
        {
            Debug.Log($"匹配完成!");
            List<MatchEntity> teamA = new List<MatchEntity>();
            List<MatchEntity> teamB = new List<MatchEntity>();
            //把人家到2 個隊伍內
            for (int i = 0; i < number*2; i++)
            {
                MatchEntity entity = pool.ElementAt(i).Value;
                if (teamA.Count < number) teamA.Add(entity);
                else if (teamB.Count < number) teamB.Add(entity);
            }

            //把2個隊伍傳給房間
            RoomManager.Instance.Add(teamA, teamB);
        }
    }
}
