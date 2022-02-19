using MobaServer.Match;
using ProtoMsg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Room
{
    class RoomManager : Singleton<RoomManager>
    {
        private int roomID = 0; //房間Id ,創建房間時++
        private ConcurrentDictionary<int, RoomEntity> roomList = new ConcurrentDictionary<int, RoomEntity>();

        /// <summary>
        /// 添加房間
        /// </summary>
        /// <param name="teamA"></param>
        /// <param name="teamB"></param>
        public void Add(List<MatchEntity> teamA, List<MatchEntity> teamB)
        {
            roomID+=1;
            //roomInfo
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.ID = roomID;
            for (int i = 0; i < teamA.Count; i++) //teamA
            {
                MatchManager.Instance.Remove(teamA[i]); //移除匹配
                roomInfo.TeamA.Add(teamA[i].player.rolesInfo);
            }
            for (int i = 0; i < teamB.Count; i++)//teamB
            {
                MatchManager.Instance.Remove(teamB[i]);//移除匹配
                roomInfo.TeamB.Add(teamB[i].player.rolesInfo);
            }
            roomInfo.StartTime = TimeHelper.Now(); //設置開始時間
            //roomEntity
            RoomEntity roomEntity = new RoomEntity(roomInfo);
            if (roomList.TryAdd(roomInfo.ID, roomEntity))
            {
                //告訴每一個客戶端 匹配成功 進到房間選英雄
                LobbyUpdateMatchStateS2C s2cMSG = new LobbyUpdateMatchStateS2C();
                s2cMSG.Result = 0; //成功
                s2cMSG.RoomInfo = roomInfo;

                roomEntity.Broadcast(1301,s2cMSG);
            }

            //用戶的隊伍Id、房間實體 緩存
            for (int i = 0; i < teamA.Count; i++)
            {
                teamA[i].player.matchEntity = null;
                teamA[i].player.roomEntity = roomEntity;
                teamA[i].player.TeamId = 0;
            }
            for (int i = 0; i < teamB.Count; i++)
            {
                teamB[i].player.matchEntity = null;
                teamB[i].player.roomEntity = roomEntity;
                teamB[i].player.TeamId = 0;
            }
        }

        /// <summary>
        /// 移除房間
        /// </summary>
        /// <param name="roomID"></param>
        public void Remove(int roomID)
        {
            RoomEntity roomEntity;
            if (roomList.TryRemove(roomID, out roomEntity))
            {
                roomEntity.Close(); //關閉房間 要做的事
                roomEntity = null;
            }
        }

        /// <summary>
        /// 用roomId獲取防加實體
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public RoomEntity GetRoom(int roomId)
        {
            RoomEntity roomEntity;
            if (roomList.TryGetValue(roomId, out roomEntity))
            {
                return roomEntity;
            }
            return null;
        }

        /// <summary>
        /// 關閉所有房間
        /// </summary>
        public void CloseAll()
        {
            foreach (var roomEntity in roomList.Values)
            {

                Remove(roomEntity.roomID);
            }
        }
    }
}
