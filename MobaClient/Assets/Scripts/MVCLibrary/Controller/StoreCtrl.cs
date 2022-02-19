using Game.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrl
{
    public class StoreCtrl : Singleton<StoreCtrl>
    {
        //
        public void SaveProp(Prop prop)
        {
            StoreModel.Instance.Add(prop);
        }

        public Prop GetProp(int id)
        {
            return StoreModel.Instance.propDic[id];
        }

    }
}
