using System.IO;

namespace GameMain.Metagame
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class WindowIconChanger
    {
        public static WindowIconChanger Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new WindowIconChanger();
                return _instance;
            }
        }

        private static WindowIconChanger _instance;

        #region WIN32API

        delegate bool EnumWindowsCallBack(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        static extern bool SetWindowTextW(IntPtr hwnd, string title);

        //回调指针，值

        [DllImport("user32")]
        static extern int EnumWindows(EnumWindowsCallBack lpEnumFunc, IntPtr lParam);

        [DllImport("user32")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref IntPtr lpdwProcessId);

        [DllImport("user32")]
        static extern bool CloseWindow(IntPtr hwnd);

        [DllImport("user32")]
        static extern int SendMessageA(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("shell32")]
        static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);

        #endregion

        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        public static IntPtr myWindowHandle;

        public WindowIconChanger()
        {
#if UNITY_STANDALONE_WIN

            IntPtr handle = (IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id; //获取进程ID

            EnumWindows(new EnumWindowsCallBack(EnumWindCallback), handle); //枚举查找本窗口

#endif
        }
        

        /// <summary>
        /// 改变窗口标题
        /// </summary>
        /// <param name="title"></param>
        public static void ChangeTitle(string title)

        {
#if UNITY_STANDALONE_WIN

            SetWindowTextW(myWindowHandle, title); //设置窗口标题

#endif
        }

        /// <summary>
        /// 更改窗口图标
        /// </summary>
        /// <param name="icon">图标路径</param>
        public  void ChangeICON(string icon)

        {
            icon = Path.Combine(Application.streamingAssetsPath, icon);
#if UNITY_STANDALONE_WIN

            IntPtr result = ExtractIcon(0, icon, 0);

            if (result != IntPtr.Zero)

            {
                SendMessageA(myWindowHandle, 0x80, 0, result);
                Debug.Log($"change success {icon}");
            }

            else
                Debug.Log("SetIconFail");

#endif
        }


        private bool EnumWindCallback(IntPtr hwnd, IntPtr lParam)

        {
#if UNITY_STANDALONE_WIN

            IntPtr pid = IntPtr.Zero;

            GetWindowThreadProcessId(hwnd, ref pid);

            if (pid == lParam) //判断当前窗口是否属于本进程

            {
                myWindowHandle = hwnd;

                return false;
            }

            return true;

#endif

            return false;
        }
    }
}