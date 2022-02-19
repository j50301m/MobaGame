using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoTest : MonoBehaviour
{
    private USocket uSocket;
    private void Start()
    {
        //建立Socket
        uSocket = new USocket(DispatchNetEvent);

        TestSend();
    }

    private void TestSend()
    {

        //假資料
        UserInfo userInfo = new UserInfo();
        userInfo.Account = "11111";
        userInfo.Password = "123456";
        //發送
        UserRegisterC2S userRegisterC2S = new UserRegisterC2S();
        userRegisterC2S.UserInfo = userInfo;
        BufferEntity buffer = BufferFactory.CreateAndSendPackage(1001, userInfo);
    }

    private void DispatchNetEvent(BufferEntity bufferEntity)
    {
        //解包邏輯
    }

    private void Update()
    {
        //處理接收到的報文
        if (uSocket!=null)
        {
            uSocket.Handle();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TestSend();
        }
    }

}
