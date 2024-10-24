namespace GameMain.Metagame
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class WindowIconChanger
    {
        // 导入 Windows API，用于获取窗口句柄和发送消息
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        // 常量
        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0; // 小图标
        private const int ICON_BIG = 1; // 大图标
        
        
        // 查找窗口句柄
        private static IntPtr GetWindowHandle()
        {
            // Unity 窗口类名默认为 "UnityWndClass"
            IntPtr hWnd = FindWindow("UnityWndClass", null);
            if (hWnd == IntPtr.Zero)
            {
                Debug.LogError("Failed to find Unity window.");
            }
        
            return hWnd;
        }
        
        // 修改窗口标题
        private static void SetWindowTitle(string title)
        {
            IntPtr hWnd = GetWindowHandle();
            if (hWnd != IntPtr.Zero)
            {
                SetWindowText(hWnd, title);
                Debug.Log("Window title changed to: " + title);
            }
        }
        
        // 设置窗口图标
        public static void SetWindowIcon(string iconPath)
        {
            IntPtr hWnd = GetWindowHandle();
            if (hWnd == IntPtr.Zero) return;
        
            iconPath = System.IO.Path.Combine(Application.streamingAssetsPath, iconPath);
            IntPtr hIcon = ExtractIcon(IntPtr.Zero, iconPath, 0);
            if (hIcon == IntPtr.Zero)
            {
                Debug.LogError("Failed to load icon: " + iconPath);
                return;
            }
        
            // 设置大图标和小图标
            SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_BIG, hIcon);
            SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_SMALL, hIcon);
        
            Debug.Log("Window icon set successfully.");
        }
    }
}