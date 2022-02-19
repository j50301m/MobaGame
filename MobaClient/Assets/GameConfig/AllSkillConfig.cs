
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AllSkillConfig
{

    static AllSkillConfig()
    {
        
       AllSkillEntity AllSkillEntity0 = new AllSkillEntity();
       AllSkillEntity0.ID = 101;
       AllSkillEntity0.SkillName = @"懲戒";
       AllSkillEntity0.Info = @"懲戒可以對野怪或者小兵造成180傷害";
       AllSkillEntity0.CoolingTime = 180f;
       AllSkillEntity0.SkillType = 1;
       AllSkillEntity0.AttackDistance = -1f;

        if (!entityDic.ContainsKey(AllSkillEntity0.ID))
        {
          entityDic.Add(AllSkillEntity0.ID, AllSkillEntity0);
        }

       AllSkillEntity AllSkillEntity1 = new AllSkillEntity();
       AllSkillEntity1.ID = 102;
       AllSkillEntity1.SkillName = @"傳送";
       AllSkillEntity1.Info = @"傳送可以將英雄送到友軍目標附近";
       AllSkillEntity1.CoolingTime = 180f;
       AllSkillEntity1.SkillType = 0;
       AllSkillEntity1.AttackDistance = 4f;

        if (!entityDic.ContainsKey(AllSkillEntity1.ID))
        {
          entityDic.Add(AllSkillEntity1.ID, AllSkillEntity1);
        }

       AllSkillEntity AllSkillEntity2 = new AllSkillEntity();
       AllSkillEntity2.ID = 103;
       AllSkillEntity2.SkillName = @"點燃";
       AllSkillEntity2.Info = @"3秒內對敵方共造成150真實傷害(50/每秒)";
       AllSkillEntity2.CoolingTime = 180f;
       AllSkillEntity2.SkillType = 0;
       AllSkillEntity2.AttackDistance = -1f;

        if (!entityDic.ContainsKey(AllSkillEntity2.ID))
        {
          entityDic.Add(AllSkillEntity2.ID, AllSkillEntity2);
        }

       AllSkillEntity AllSkillEntity3 = new AllSkillEntity();
       AllSkillEntity3.ID = 104;
       AllSkillEntity3.SkillName = @"淨化";
       AllSkillEntity3.Info = @"淨化可以解除身上的負面效果";
       AllSkillEntity3.CoolingTime = 180f;
       AllSkillEntity3.SkillType = 0;
       AllSkillEntity3.AttackDistance = -1f;

        if (!entityDic.ContainsKey(AllSkillEntity3.ID))
        {
          entityDic.Add(AllSkillEntity3.ID, AllSkillEntity3);
        }

       AllSkillEntity AllSkillEntity4 = new AllSkillEntity();
       AllSkillEntity4.ID = 105;
       AllSkillEntity4.SkillName = @"屏障";
       AllSkillEntity4.Info = @"屏障可以給自己臨時增加一個護盾,3秒內可以抵抗200傷害";
       AllSkillEntity4.CoolingTime = 180f;
       AllSkillEntity4.SkillType = 0;
       AllSkillEntity4.AttackDistance = -1f;

        if (!entityDic.ContainsKey(AllSkillEntity4.ID))
        {
          entityDic.Add(AllSkillEntity4.ID, AllSkillEntity4);
        }

       AllSkillEntity AllSkillEntity5 = new AllSkillEntity();
       AllSkillEntity5.ID = 106;
       AllSkillEntity5.SkillName = @"閃現";
       AllSkillEntity5.Info = @"瞬間向指定方向傳送一小段距離,最大為2米,如無法穿越障礙物,則會發生撞牆事故!";
       AllSkillEntity5.CoolingTime = 180f;
       AllSkillEntity5.SkillType = 0;
       AllSkillEntity5.AttackDistance = 4f;

        if (!entityDic.ContainsKey(AllSkillEntity5.ID))
        {
          entityDic.Add(AllSkillEntity5.ID, AllSkillEntity5);
        }

       AllSkillEntity AllSkillEntity6 = new AllSkillEntity();
       AllSkillEntity6.ID = 107;
       AllSkillEntity6.SkillName = @"虛弱";
       AllSkillEntity6.Info = @"虛弱可以讓對方的英雄持續2秒減少移動速度50%";
       AllSkillEntity6.CoolingTime = 180f;
       AllSkillEntity6.SkillType = 0;
       AllSkillEntity6.AttackDistance = 4f;

        if (!entityDic.ContainsKey(AllSkillEntity6.ID))
        {
          entityDic.Add(AllSkillEntity6.ID, AllSkillEntity6);
        }

       AllSkillEntity AllSkillEntity7 = new AllSkillEntity();
       AllSkillEntity7.ID = 108;
       AllSkillEntity7.SkillName = @"治療";
       AllSkillEntity7.Info = @"可以瞬間恢復自己200血量,並使自己3米範圍內最近的友軍得到100血量的治療";
       AllSkillEntity7.CoolingTime = 180f;
       AllSkillEntity7.SkillType = 0;
       AllSkillEntity7.AttackDistance = -1f;

        if (!entityDic.ContainsKey(AllSkillEntity7.ID))
        {
          entityDic.Add(AllSkillEntity7.ID, AllSkillEntity7);
        }

       AllSkillEntity AllSkillEntity8 = new AllSkillEntity();
       AllSkillEntity8.ID = 2;
       AllSkillEntity8.SkillName = @"普通物理攻擊";
       AllSkillEntity8.Info = @"對英雄造成相當於自身攻擊力的傷害";
       AllSkillEntity8.CoolingTime = 0.5f;
       AllSkillEntity8.SkillType = 1;
       AllSkillEntity8.AttackDistance = 4f;

        if (!entityDic.ContainsKey(AllSkillEntity8.ID))
        {
          entityDic.Add(AllSkillEntity8.ID, AllSkillEntity8);
        }

       AllSkillEntity AllSkillEntity9 = new AllSkillEntity();
       AllSkillEntity9.ID = 3;
       AllSkillEntity9.SkillName = @"普通法術攻擊";
       AllSkillEntity9.Info = @"對英雄造成相當於自身法術強度的傷害";
       AllSkillEntity9.CoolingTime = 0.5f;
       AllSkillEntity9.SkillType = 1;
       AllSkillEntity9.AttackDistance = 4f;

        if (!entityDic.ContainsKey(AllSkillEntity9.ID))
        {
          entityDic.Add(AllSkillEntity9.ID, AllSkillEntity9);
        }

       AllSkillEntity AllSkillEntity10 = new AllSkillEntity();
       AllSkillEntity10.ID = 4;
       AllSkillEntity10.SkillName = @"回城";
       AllSkillEntity10.Info = @"施放技能回到出生泉水";
       AllSkillEntity10.CoolingTime = 4f;
       AllSkillEntity10.SkillType = 1;
       AllSkillEntity10.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity10.ID))
        {
          entityDic.Add(AllSkillEntity10.ID, AllSkillEntity10);
        }

       AllSkillEntity AllSkillEntity11 = new AllSkillEntity();
       AllSkillEntity11.ID = 100101;
       AllSkillEntity11.SkillName = @"火炎刃";
       AllSkillEntity11.Info = @"測試..";
       AllSkillEntity11.CoolingTime = 8f;
       AllSkillEntity11.SkillType = 1;
       AllSkillEntity11.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity11.ID))
        {
          entityDic.Add(AllSkillEntity11.ID, AllSkillEntity11);
        }

       AllSkillEntity AllSkillEntity12 = new AllSkillEntity();
       AllSkillEntity12.ID = 100102;
       AllSkillEntity12.SkillName = @"三陽真火訣";
       AllSkillEntity12.Info = @"測試..";
       AllSkillEntity12.CoolingTime = 5f;
       AllSkillEntity12.SkillType = 1;
       AllSkillEntity12.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity12.ID))
        {
          entityDic.Add(AllSkillEntity12.ID, AllSkillEntity12);
        }

       AllSkillEntity AllSkillEntity13 = new AllSkillEntity();
       AllSkillEntity13.ID = 100103;
       AllSkillEntity13.SkillName = @"五方浩風訣";
       AllSkillEntity13.Info = @"測試..";
       AllSkillEntity13.CoolingTime = 7f;
       AllSkillEntity13.SkillType = 1;
       AllSkillEntity13.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity13.ID))
        {
          entityDic.Add(AllSkillEntity13.ID, AllSkillEntity13);
        }

       AllSkillEntity AllSkillEntity14 = new AllSkillEntity();
       AllSkillEntity14.ID = 100104;
       AllSkillEntity14.SkillName = @"六合寒水訣";
       AllSkillEntity14.Info = @"測試..";
       AllSkillEntity14.CoolingTime = 30f;
       AllSkillEntity14.SkillType = 1;
       AllSkillEntity14.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity14.ID))
        {
          entityDic.Add(AllSkillEntity14.ID, AllSkillEntity14);
        }

       AllSkillEntity AllSkillEntity15 = new AllSkillEntity();
       AllSkillEntity15.ID = 100201;
       AllSkillEntity15.SkillName = @"七星聚首";
       AllSkillEntity15.Info = @"測試..";
       AllSkillEntity15.CoolingTime = 8f;
       AllSkillEntity15.SkillType = 1;
       AllSkillEntity15.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity15.ID))
        {
          entityDic.Add(AllSkillEntity15.ID, AllSkillEntity15);
        }

       AllSkillEntity AllSkillEntity16 = new AllSkillEntity();
       AllSkillEntity16.ID = 100202;
       AllSkillEntity16.SkillName = @"索神決";
       AllSkillEntity16.Info = @"測試..";
       AllSkillEntity16.CoolingTime = 5f;
       AllSkillEntity16.SkillType = 1;
       AllSkillEntity16.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity16.ID))
        {
          entityDic.Add(AllSkillEntity16.ID, AllSkillEntity16);
        }

       AllSkillEntity AllSkillEntity17 = new AllSkillEntity();
       AllSkillEntity17.ID = 100203;
       AllSkillEntity17.SkillName = @"九玄天元訣";
       AllSkillEntity17.Info = @"測試..";
       AllSkillEntity17.CoolingTime = 7f;
       AllSkillEntity17.SkillType = 1;
       AllSkillEntity17.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity17.ID))
        {
          entityDic.Add(AllSkillEntity17.ID, AllSkillEntity17);
        }

       AllSkillEntity AllSkillEntity18 = new AllSkillEntity();
       AllSkillEntity18.ID = 100204;
       AllSkillEntity18.SkillName = @"八荒地煞訣";
       AllSkillEntity18.Info = @"測試..";
       AllSkillEntity18.CoolingTime = 30f;
       AllSkillEntity18.SkillType = 1;
       AllSkillEntity18.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity18.ID))
        {
          entityDic.Add(AllSkillEntity18.ID, AllSkillEntity18);
        }

       AllSkillEntity AllSkillEntity19 = new AllSkillEntity();
       AllSkillEntity19.ID = 100301;
       AllSkillEntity19.SkillName = @"焚心術";
       AllSkillEntity19.Info = @"測試..";
       AllSkillEntity19.CoolingTime = 8f;
       AllSkillEntity19.SkillType = 1;
       AllSkillEntity19.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity19.ID))
        {
          entityDic.Add(AllSkillEntity19.ID, AllSkillEntity19);
        }

       AllSkillEntity AllSkillEntity20 = new AllSkillEntity();
       AllSkillEntity20.ID = 100302;
       AllSkillEntity20.SkillName = @"百鬼夜行";
       AllSkillEntity20.Info = @"測試..";
       AllSkillEntity20.CoolingTime = 5f;
       AllSkillEntity20.SkillType = 1;
       AllSkillEntity20.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity20.ID))
        {
          entityDic.Add(AllSkillEntity20.ID, AllSkillEntity20);
        }

       AllSkillEntity AllSkillEntity21 = new AllSkillEntity();
       AllSkillEntity21.ID = 100303;
       AllSkillEntity21.SkillName = @"陰陽符";
       AllSkillEntity21.Info = @"測試..";
       AllSkillEntity21.CoolingTime = 7f;
       AllSkillEntity21.SkillType = 1;
       AllSkillEntity21.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity21.ID))
        {
          entityDic.Add(AllSkillEntity21.ID, AllSkillEntity21);
        }

       AllSkillEntity AllSkillEntity22 = new AllSkillEntity();
       AllSkillEntity22.ID = 100304;
       AllSkillEntity22.SkillName = @"脫胎換骨";
       AllSkillEntity22.Info = @"測試..";
       AllSkillEntity22.CoolingTime = 30f;
       AllSkillEntity22.SkillType = 1;
       AllSkillEntity22.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity22.ID))
        {
          entityDic.Add(AllSkillEntity22.ID, AllSkillEntity22);
        }

       AllSkillEntity AllSkillEntity23 = new AllSkillEntity();
       AllSkillEntity23.ID = 100401;
       AllSkillEntity23.SkillName = @"破天擊";
       AllSkillEntity23.Info = @"測試..";
       AllSkillEntity23.CoolingTime = 8f;
       AllSkillEntity23.SkillType = 1;
       AllSkillEntity23.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity23.ID))
        {
          entityDic.Add(AllSkillEntity23.ID, AllSkillEntity23);
        }

       AllSkillEntity AllSkillEntity24 = new AllSkillEntity();
       AllSkillEntity24.ID = 100402;
       AllSkillEntity24.SkillName = @"金戈吟";
       AllSkillEntity24.Info = @"測試..";
       AllSkillEntity24.CoolingTime = 5f;
       AllSkillEntity24.SkillType = 1;
       AllSkillEntity24.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity24.ID))
        {
          entityDic.Add(AllSkillEntity24.ID, AllSkillEntity24);
        }

       AllSkillEntity AllSkillEntity25 = new AllSkillEntity();
       AllSkillEntity25.ID = 100403;
       AllSkillEntity25.SkillName = @"驚魂擊";
       AllSkillEntity25.Info = @"測試..";
       AllSkillEntity25.CoolingTime = 7f;
       AllSkillEntity25.SkillType = 1;
       AllSkillEntity25.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity25.ID))
        {
          entityDic.Add(AllSkillEntity25.ID, AllSkillEntity25);
        }

       AllSkillEntity AllSkillEntity26 = new AllSkillEntity();
       AllSkillEntity26.ID = 100404;
       AllSkillEntity26.SkillName = @"天地震";
       AllSkillEntity26.Info = @"測試..";
       AllSkillEntity26.CoolingTime = 30f;
       AllSkillEntity26.SkillType = 1;
       AllSkillEntity26.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity26.ID))
        {
          entityDic.Add(AllSkillEntity26.ID, AllSkillEntity26);
        }

       AllSkillEntity AllSkillEntity27 = new AllSkillEntity();
       AllSkillEntity27.ID = 100501;
       AllSkillEntity27.SkillName = @"爍空勁";
       AllSkillEntity27.Info = @"測試..";
       AllSkillEntity27.CoolingTime = 8f;
       AllSkillEntity27.SkillType = 1;
       AllSkillEntity27.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity27.ID))
        {
          entityDic.Add(AllSkillEntity27.ID, AllSkillEntity27);
        }

       AllSkillEntity AllSkillEntity28 = new AllSkillEntity();
       AllSkillEntity28.ID = 100502;
       AllSkillEntity28.SkillName = @"鎮龍擊";
       AllSkillEntity28.Info = @"測試..";
       AllSkillEntity28.CoolingTime = 5f;
       AllSkillEntity28.SkillType = 1;
       AllSkillEntity28.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity28.ID))
        {
          entityDic.Add(AllSkillEntity28.ID, AllSkillEntity28);
        }

       AllSkillEntity AllSkillEntity29 = new AllSkillEntity();
       AllSkillEntity29.ID = 100503;
       AllSkillEntity29.SkillName = @"滄瀾破";
       AllSkillEntity29.Info = @"測試..";
       AllSkillEntity29.CoolingTime = 7f;
       AllSkillEntity29.SkillType = 1;
       AllSkillEntity29.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity29.ID))
        {
          entityDic.Add(AllSkillEntity29.ID, AllSkillEntity29);
        }

       AllSkillEntity AllSkillEntity30 = new AllSkillEntity();
       AllSkillEntity30.ID = 100504;
       AllSkillEntity30.SkillName = @"赤烏墜";
       AllSkillEntity30.Info = @"測試..";
       AllSkillEntity30.CoolingTime = 30f;
       AllSkillEntity30.SkillType = 1;
       AllSkillEntity30.AttackDistance = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity30.ID))
        {
          entityDic.Add(AllSkillEntity30.ID, AllSkillEntity30);
        }

    }

    
    static Dictionary<int, AllSkillEntity> entityDic = new Dictionary<int, AllSkillEntity>();
    public static AllSkillEntity Get(int key)
    {
        if (entityDic.ContainsKey(key))
        {
            return entityDic[key];
        }
        return null;
    }

    
   
    public static AllSkillEntity GetInstance(int key)
    {
        AllSkillEntity instance = new AllSkillEntity();
        if (entityDic.ContainsKey(key))
        {
            
            instance.ID = entityDic[key].ID;
            instance.SkillName = entityDic[key].SkillName;
            instance.Info = entityDic[key].Info;
            instance.CoolingTime = entityDic[key].CoolingTime;
            instance.SkillType = entityDic[key].SkillType;
            instance.AttackDistance = entityDic[key].AttackDistance;

        }
        return instance;
    }
}


public class AllSkillEntity
{
    //TemplateMember
    public int ID;//技能ID
    public string SkillName;//名稱
    public string Info;//介紹
    public float CoolingTime;//冷卻時長
    public int SkillType;//技能類型:0瞬發,沒有動畫 1:帶動畫,由關鍵幀觸發
    public float AttackDistance;//攻擊距離(米/半徑)

}
