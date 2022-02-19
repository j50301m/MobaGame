using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Ctrl;
using ProtoMsg;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    public PlayerInfo playerInfo;
    private bool isSelf = false; //用來判斷這隻角色是不是自己的
    private Vector3 spawnPosition;
    private Vector3 spawnRotation;
    private HeroAttributeEntity currentAttribute; //目前屬性 ex:目前攻擊力、目前血量
    private HeroAttributeEntity totalAttribute; //總屬性 ex: 總攻擊力、總血量
    //人物血條
    private GameObject hud; //人物血條UI
    private Vector3 hudOffect = new Vector3(0, 3.1f, 0); //血量位置
    private Text hpText, mPText, levelText, nickNameText;  //文本
    private Image hpFill, mpFill;
    //技能管理器
    private SkillManager skillManager;
    /// <summary>
    /// 初始化控制器
    /// </summary>
    /// <param name="playerInfo"></param>
    internal void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        //判斷是不是自己的英雄
        isSelf=RolesCtrl.Instance.CheckIsSelf(playerInfo.RolesInfo.RolesID);

        //英雄復活時的位置
        spawnPosition = transform.position;
        spawnRotation = transform.eulerAngles;

        //獲取英雄屬性(當前屬性 和 總屬性)
        currentAttribute = HeroAttributeConfig.GetInstance(playerInfo.HeroID);
        totalAttribute =  HeroAttributeConfig.GetInstance(playerInfo.HeroID);

        RoomCtrl.Instance.SaveHeroAttribute(playerInfo.RolesInfo.RolesID, currentAttribute, totalAttribute); //將資料存到RoomModel
        //設置相機Follow Rotation
        if (isSelf)
        {
            if (playerInfo.TeamID == 0) Camera.main.transform.eulerAngles = new Vector3(45, 180, 0);
            else Camera.main.transform.eulerAngles = new Vector3(45, -180, 0);
        }
        //人物的HUD 血條 MP 暱稱
        hud = ResManager.Instance.LoadHUD();
        hud.transform.position = transform.position + hudOffect;
        hud.transform.eulerAngles = Camera.main.transform.eulerAngles;

        hpFill = hud.transform.Find("HP/Fill").GetComponent<Image>();
        mpFill = hud.transform.Find("MP/Fill").GetComponent<Image>();

        hpText= hud.transform.Find("HP/Text").GetComponent<Text>();
        mPText=hud.transform.Find("MP/Text").GetComponent<Text>();
        nickNameText= hud.transform.Find("NickName").GetComponent<Text>();
        levelText= hud.transform.Find("Level/Text").GetComponent<Text>();
        HUDUpdate(true);

        //技能管理器
        skillManager= this.gameObject.AddComponent<SkillManager>();
        skillManager.Init(this);
        //動畫管理器

        //角色狀態機
        //音效廣裡器


    }
    //更新血條狀態
    public void HUDUpdate(bool isInit=false)
    {
        
        hpText.text = $"{currentAttribute.HP} / {totalAttribute.HP}";
        mPText.text = $"{currentAttribute.MP} / {totalAttribute.MP}";
        levelText.text = currentAttribute.Level.ToString();
        nickNameText.text = playerInfo.RolesInfo.NickName;

        if (isInit)
        {
            hpFill.fillAmount = 1;
            mpFill.fillAmount = 1;
        }
        else
        {
            hpFill.DOFillAmount(currentAttribute.HP / totalAttribute.HP, 0.2f).SetAutoKill(true);
            hpFill.DOFillAmount(currentAttribute.MP / totalAttribute.MP, 0.2f).SetAutoKill(true);
        }
    }
    private void Update()
    {

    }

    private Vector3 cameraOffset = new Vector3(0, 11, 10);
    private void LateUpdate()
    {
        //相機位置
        if (isSelf)
        {
            Camera.main.transform.position = this.transform.position + cameraOffset;
        }
        //血條位置
        if (hud != null)
        {
            hud.transform.position = transform.position + hudOffect;
        }
    }

}
