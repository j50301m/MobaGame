using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private PlayerCtrl playerCtrl;
    private PlayerInfo playerInfo;

    private Dictionary<KeyCode, int> skillId = new Dictionary<KeyCode, int>();
    private Dictionary<KeyCode, float> coolingConfig = new Dictionary<KeyCode, float>();

    public  void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
        playerInfo = playerCtrl.playerInfo;

        //技能配置訊息
        HeroSkillEntity skill=HeroSkillConfig.GetInstance(playerInfo.HeroID);
        skillId[KeyCode.Q] = skill.Q_ID;
        skillId[KeyCode.W] = skill.W_ID;
        skillId[KeyCode.E] = skill.E_ID;
        skillId[KeyCode.R] = skill.R_ID;
        skillId[KeyCode.A] = skill.A_ID;

        //D F按鍵
        skillId[KeyCode.D] = playerInfo.SkillA;
        skillId[KeyCode.F] = playerInfo.SkillB;
        //回城
        skillId[KeyCode.B] = 4; //回家 技能ID=4

        //技能冷卻時間
        coolingConfig[KeyCode.Q] = AllSkillConfig.Get(skill.Q_ID).CoolingTime;
        coolingConfig[KeyCode.W] = AllSkillConfig.Get(skill.W_ID).CoolingTime;
        coolingConfig[KeyCode.E] = AllSkillConfig.Get(skill.E_ID).CoolingTime;
        coolingConfig[KeyCode.R] = AllSkillConfig.Get(skill.R_ID).CoolingTime;
        coolingConfig[KeyCode.A] = 0.5f;

        coolingConfig[KeyCode.D] = 180;
        coolingConfig[KeyCode.F] = 180;
        coolingConfig[KeyCode.B] = 4;  //回城時間

        //最後按下的時間
    }

    private Dictionary<KeyCode, float> keyDownTime = new Dictionary<KeyCode, float>();

    //技能冷卻
    public void DoCooling(KeyCode key, Action<float>action)
    {
        keyDownTime[key] = Time.time;
        if (action!=null)
        {
            action(keyDownTime[key]);
        }
    }

    //剩餘時間
    public float SurplusTime(KeyCode key)
    {
        //冷卻時間-(現在的的時間-按下的時間)=剩下的冷卻時間
        float time = coolingConfig[key] - (Time.time - keyDownTime[key]);
        if (time <= 0)
        {
            time = 0;
        }
        return time;
    }

    //是否正在CD
    public bool IsCooling(KeyCode key)
    {
        return SurplusTime(key) > 0?true:false;
    }

    //獲取SkillId
    public int GetSkillId(KeyCode key)
    {
        return skillId[key];
    }
}
