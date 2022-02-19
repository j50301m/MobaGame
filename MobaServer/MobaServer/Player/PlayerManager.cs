using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Player
{
    /// <summary>
    /// 管理PlayerEntity
    /// </summary>
    public static class PlayerManager
    {
        //key1=sessionId ,key2=rolesId ,value=Player實體
        private static ConcurrentDictionary<Tuple<int, int>, PlayerEntity> playerList = new ConcurrentDictionary<Tuple<int, int>, PlayerEntity>();

        
        public static void Add(int sessionId, int rolesId, PlayerEntity player)
        {
            playerList.TryAdd(new Tuple<int, int>(sessionId, rolesId), player);
        }
        public static bool Update(int sessionId, int rolesId, PlayerEntity player)
        {
            PlayerEntity oldValue;
            playerList.TryGetValue(new Tuple<int, int>(sessionId, rolesId),out oldValue);
            return playerList.TryUpdate(new Tuple<int, int>(sessionId, rolesId), player, oldValue);
        }
        public static bool RemovePlayerEntityFromSession(int sessionId)
        {
            PlayerEntity player;
            bool result = false;
            foreach (Tuple<int, int> e in playerList.Keys)
            {
                if (e.Item1.Equals(sessionId))
                {
                    result=playerList.TryRemove(e, out player);
                }
            }
            return result;
        }
        public static bool RemovePlayerEntityFromRoles(int rolesId)
        {
            PlayerEntity player;
            bool result = false;
            foreach (Tuple<int, int> e in playerList.Keys)
            {
                if (e.Item2.Equals(rolesId))
                {
                    result = playerList.TryRemove(e, out player);
                }
            }
            return result;
        }
        public static PlayerEntity GetPlayerEntityFromSession(int sessionId)
        {
            PlayerEntity player=null;
            foreach (Tuple<int, int> e in playerList.Keys)
            {
                if (e.Item1.Equals(sessionId))
                {
                    playerList.TryGetValue(e, out player);
                }
            }
            return player;
        }
        public static PlayerEntity GetPlayerEntityFromRoles(int rolesId)
        {
            PlayerEntity player = null;
            foreach (Tuple<int, int> e in playerList.Keys)
            {
                if (e.Item2.Equals(rolesId))
                {
                    playerList.TryGetValue(e, out player);
                }
            }
            return player;
        }


        /*
         //key=角色ID value=角色實體
        private static ConcurrentDictionary<int, PlayerEntity> playerList = new ConcurrentDictionary<int, PlayerEntity>();
        //kwy= 會話ID value= 角色實體
        private static ConcurrentDictionary<int, PlayerEntity> playerSession = new ConcurrentDictionary<int, PlayerEntity>();

        /// <summary>
        /// 添加用戶訊息到Server本地
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="playerEntity"></param>
        public static void Add(int sessionId, int userId, PlayerEntity playerEntity)
        {
            playerList.TryAdd(userId, playerEntity);
            playerSession.TryAdd(sessionId, playerEntity);
        }
        /// <summary>
        /// 從連線清單移除player
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool RemoveFromSession(int sessionId)
        {
            PlayerEntity playerEntity;
            return playerSession.TryRemove(sessionId,out playerEntity);
        }
        /// <summary>
        /// 從角色清單移除player
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public static bool RemoveFromRolesID(int rolesId)
        {
            PlayerEntity playerEntity;
            return playerList.TryRemove(rolesId, out playerEntity);
        }
        /// <summary>
        /// 利用SessionId獲取Player
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static PlayerEntity GetPlayerEntityFromSession(int sessionId)
        {
            PlayerEntity playerEntity;
            if (playerSession.TryGetValue(sessionId, out playerEntity)) return playerEntity;
            else return null;
        }
        /// <summary>
        /// 利用RolesId 獲取Player
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public static PlayerEntity GetPlayerEntityFromRoles(int rolesId)
        {
            PlayerEntity playerEntity;
            if (playerList.TryGetValue(rolesId, out playerEntity)) return playerEntity;
            else return null;
        }
        */
    }
}
