using Game.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public USocket uSocket;

    private void Start()
    {
        uSocket = new USocket(DisPatchNetEvent);
        //打開登入介面
        WindowManager.Instance.OpenWindow(WindowType.LoginWindow);
    }
    private void Update()
    {
        //處理消息
        if (uSocket != null)
        {
            uSocket.Handle();
        }
    }



    private void DisPatchNetEvent(BufferEntity buffer)
    {
        NetEvent.Instance.Dispatch(buffer.messageID, buffer);
    }

}

