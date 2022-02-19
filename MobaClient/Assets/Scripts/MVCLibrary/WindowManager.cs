using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.View;
using System;

/// <summary>
/// 窗體類型
/// </summary>
public enum WindowType
{
    LoginWindow,
    RolesWindow,
    LobbyWindow,
    BattleWindow,
    StoreWindow,
    TipsWindow,//提示窗口
    RoomWindow
}
/// <summary>
/// 場景類型:提供每個場景預先加載UI組件
/// </summary>
public enum ScenesType
{
    None,
    Login,
    Battle,
}

public class WindowManager : MonoSingleton<WindowManager>
{
    //所有的UI介面 存成字典
    Dictionary<WindowType, BaseWindow> windowDIC;

    /// <summary>
    /// 建構子:初始化
    /// </summary>
    public WindowManager()
    {
        windowDIC = new Dictionary<WindowType, BaseWindow>();

        //註冊UI window
        // windowDIC.Add(WindowType.StoreWindow,new StoreWindow()); //測試用
        windowDIC.Add(WindowType.LoginWindow, new LoginWindow());
        windowDIC.Add(WindowType.TipsWindow, new TipsWindow());
        windowDIC.Add(WindowType.RolesWindow, new RolesWindow());
        windowDIC.Add(WindowType.LobbyWindow, new LobbyWindow());
        windowDIC.Add(WindowType.RoomWindow, new RoomWindow());

    }
    /// <summary>
    /// 每一個間隔間 UI 要做的事
    /// </summary>
    public void Update()
    {
        foreach (var window in windowDIC.Values)
        {
            if (window.IsVisible())
            {
                window.Update(Time.deltaTime);
            }
        }
            
    }
    
    /// <summary>
    /// 提供外部 打開UI Window
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public BaseWindow OpenWindow(WindowType type)
    {
        BaseWindow window;
        if (windowDIC.TryGetValue(type, out window))
        {
            window.Open();
            return window;
        }
        else
        {
            Debug.LogError($"Open Error:{type}");
            return null;
        }
    }

    /// <summary>
    /// 提供外部 關閉UI Window
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void CloseWindow(WindowType type) {
        BaseWindow window;
        if (windowDIC.TryGetValue(type, out window))
        {
            window.Close();
        }
        else
        {
            Debug.LogError($"Open Error:{type}");
        }
    }

    /// <summary>
    /// 預先加載 場景的UI
    /// </summary>
    /// <param name="type"></param>
    public void PreLoadWindow(ScenesType type)
    {
        foreach (var item in windowDIC.Values)
        {
            if (item.GetScenesType()==type)
            {
                item.PreLoad();
            }
        }
    }

    /// <summary>
    /// 關閉場景的所有UI
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isDestroy"></param>
    public void HideAllWindow(ScenesType type,bool isDestroy=false) {

        foreach (var item in windowDIC.Values)
        {
            if (item.GetScenesType() == type)
            {
                item.Close(isDestroy);
            }
        }
    }

    /// <summary>
    /// 顯示提示窗體
    /// </summary>
    public void ShowTips(string text,Action enterBtnAction=null,Action closeBtnAction=null)
    {
        TipsWindow tipsWindow =(TipsWindow)Instance.OpenWindow(WindowType.TipsWindow);
        tipsWindow.ShowTips(text, enterBtnAction, closeBtnAction);
    }
}
