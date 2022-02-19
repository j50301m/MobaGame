using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager:Singleton<ResManager>
{
    /// <summary>
    /// 加載UI Window
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject LoadUI(string path)
    {
        GameObject go=Resources.Load<GameObject>($"UIPrefab/{path}");
        if (go == null)
        {
            Debug.LogError($"找不到UI Window{path}");
            return null;
        }
        return GameObject.Instantiate(go);
    }

    /// <summary>
    /// 加載英雄頭像
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Sprite LoadRoundHead(int heroId)
    {
        return Resources.Load<Sprite>($"Image/Round/{heroId}");
    }

    /// <summary>
    /// 加載召喚師技能
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Sprite LoadGeneraSkill(int  skillId)
    {
        return Resources.Load<Sprite>($"Image/GeneralSkill/{skillId}");
    }

    /// <summary>
    /// 加載英雄大圖
    /// </summary>
    /// <param name="heroId"></param>
    /// <returns></returns>
    public Sprite LoadHeroTexture(int heroId)
    {
        return Resources.Load<Sprite>($"Image/HeroTexture/{heroId}");
    }

    /// <summary>
    /// 加載英雄模型
    /// </summary>
    /// <param name="heroId"></param>
    /// <returns></returns>
    public GameObject LoadHeroModel(int heroId)
    {
        GameObject go = Resources.Load<GameObject>($"Hero/{heroId}/Model/{ heroId}");
        return GameObject.Instantiate(go); //克隆物體
    }

    public GameObject LoadHUD()
    {
        GameObject go = Resources.Load<GameObject>($"HUD/HeroHead");
        return GameObject.Instantiate(go); //克隆物體
    }
}
