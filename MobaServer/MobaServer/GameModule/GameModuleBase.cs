using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
    public class GameModuleBase<T> where T:new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null) instance = new T();
                return instance;
            }
        
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            AddListener();
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        public virtual void Release()
        {
            RemoveListener();
        }

        /// <summary>
        /// 註冊監聽
        /// </summary>
        public virtual void AddListener()
        {

        }

        /// <summary>
        /// 移除監聽
        /// </summary>
        public virtual void RemoveListener()
        {

        }
    }
}
