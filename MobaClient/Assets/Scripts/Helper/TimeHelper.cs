using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeHelper 
{
    private static readonly long epoch = new DateTime(1790, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

    /// <summary>
    /// 獲取當前時間戳 (ms)
    /// </summary>
    /// <returns></returns>
    private static long ClientNow()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000; //回傳毫秒級別時間戳
    }

    /// <summary>
    /// 獲取當前時間戳(秒)
    /// </summary>
    /// <returns></returns>
    public static long ClientNowSeconds()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000000;  //回傳秒級別時間戳
    }

    /// <summary>
    /// 獲取當前時間戳 (ms)
    /// </summary>
    /// <returns></returns>
    public static long Now()
    {
        return ClientNow();
    }
}
