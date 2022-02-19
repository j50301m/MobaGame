using Game.Ctrl;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class LoginWindow : BaseWindow
    {

        private InputField AccountInput;
        private InputField PwdInput;

        public LoginWindow()
        {
            selfType = WindowType.LoginWindow;
            scenesType = ScenesType.Login;
            resident = false;
            resName = "UIPrefab/User/LoginWindow";
    }

        protected override void Awake()
        {
            base.Awake();

            AccountInput = transform.Find("UserBack/AccountInput").GetComponent<InputField>();
            PwdInput = transform.Find("UserBack/PwdInput").GetComponent<InputField>();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }



        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1000, HandleUserRegisterS2C);
            NetEvent.Instance.AddEventListener(1001, HandleUserLoginS2C);
        }

        private void HandleUserLoginS2C(BufferEntity obj)
        {
            UserLoginS2C s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(obj.proto);

            switch (s2cMSG.Result)
            {

                case 0://登入成功
                    Debug.Log("登入成功");
                    if (s2cMSG.RolesInfo != null)
                    {
                        //保存數據
                        LoginCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
                        //打開大廳
                        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
                    }
                    else
                    {
                        //跳轉到RolesWindow
                        WindowManager.Instance.OpenWindow(WindowType.RolesWindow);
                    }
                    Close();//關閉自己
                    break;

                case 1://帳號不存在
                    Debug.Log("帳號不存在");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("帳號不存在");
                    break;


                case 2://密碼部正確
                    Debug.Log("密碼不正確");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("密碼不正確");
                    break;

                case 3://帳號凍結中
                    Debug.Log("帳號停權中");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("帳號停權中");
                    break;
                case 4://永久被Ban
                    Debug.Log("帳號已被永久封禁");
                    WindowManager.Instance.ShowTips("帳號被永久封禁");
                    break;
                //打開提示窗

                default:
                    break;
            }
        }

        private void HandleUserRegisterS2C(BufferEntity obj)
        {
            UserRegisterS2C s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(obj.proto);

            switch (s2cMSG.Result)
            {
                
                case 0://註冊成功
                    Debug.Log("註冊成功");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("註冊成功!!");
                    break;
                
                case 1://帳號存在SQL敏感詞
                    Debug.Log("帳號存在SQL敏感詞");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("帳號不合法");
                    break;

               
                case 2://帳號密碼長度不足
                    Debug.Log("密碼長度不足");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("密碼長度不足");
                    break;
                
                case 3://帳號已被註冊
                    Debug.Log("帳號已被註冊");
                    //打開提示窗
                    WindowManager.Instance.ShowTips("帳號已被註冊!!");
                    break;
                default:
                    break;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    //註冊
                    case "RegisterBtn":
                        buttonList[i].onClick.AddListener(RegisterBtnOnClick);
                        break;
                    //登入
                    case "LoginBtn":
                        buttonList[i].onClick.AddListener(LoginBtnOnClick);
                        break;

                    default:
                        break;
                }
            }
        }

        private void LoginBtnOnClick()
        {
            if (string.IsNullOrEmpty(AccountInput.text) || string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("帳號及密碼不能為空");
                return;
            }

            UserLoginC2S c2sMSG = new UserLoginC2S();
            c2sMSG.UserInfo = new UserInfo
            {
                Account = AccountInput.text,
                Password = Str2MD5(PwdInput.text)
            };

            //發送封包
            BufferFactory.CreateAndSendPackage(1001, c2sMSG);
        }

        private void RegisterBtnOnClick()
        {
            if (string.IsNullOrEmpty(AccountInput.text) || string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("帳號及密碼不能為空");
                return;
            }

            UserRegisterC2S c2sMSG = new UserRegisterC2S();
            c2sMSG.UserInfo = new UserInfo
            {
                Account = AccountInput.text,
                Password = Str2MD5(PwdInput.text)
            };

            //發送封包
            BufferFactory.CreateAndSendPackage(1000, c2sMSG);
        }

        /// <summary>
        /// String 轉成MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Str2MD5(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();


            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

