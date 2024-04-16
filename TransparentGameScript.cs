using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine.XR.WSA.Input;

public class TransparentGameScript : MonoBehaviour
{
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    //https://learn.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey,byte bAlpha, uint dwFlags);

    private struct WINDOW_DIMENSIONS { //sets the margins of stuff, thiese four components are needed 

        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;

    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr window_handle, ref WINDOW_DIMENSIONS _DIMENSIONS); //modifies frame

    const int WindowLongStyle = -20;

    const uint WS_EX_LAYERED = 0x00080000; //Optimises the window
    const uint WS_EX_TRANSPARENT = 0x00000020; //Basically keeps it ontop if I understand it correclty

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); //keeps the game the top most 

    private IntPtr window_handle = new IntPtr(0);

    private bool isClickable = true;

    private void Awake() {

#if !UNITY_EDITOR //If statement exists just in case this is run within the editor, without this the editor will become unclickable 
            window_handle = GetActiveWindow(); //gets game window
#endif

    }
    private void Start()
    {

        string path = Directory.GetCurrentDirectory();
        string fileName = @path + "\\( ⓛ ω ⓛ ).txt";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }


        WINDOW_DIMENSIONS dIMENSIONS = new WINDOW_DIMENSIONS { cxLeftWidth = -1 }; //turns it around, its the equivolent of it showing the clear inside of a view model

            DwmExtendFrameIntoClientArea(window_handle, ref dIMENSIONS);

            SetWindowLong(window_handle, WindowLongStyle, WS_EX_LAYERED | WS_EX_TRANSPARENT);

            SetWindowPos(window_handle, HWND_TOPMOST, 0, 0, 0, 0, 0);

        Application.runInBackground = true;

    }

    private void Update() { //Will only run after message box has been resolved

        CheckIfMouseIsOverButton();

    }

    private void CheckIfMouseIsOverButton() { //Anything interactable needs to have a collider trigger underneath

        if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) == null && isClickable)
        {
            SetWindowLong(window_handle, WindowLongStyle, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            isClickable = false;
        }
        else
        {
            SetWindowLong(window_handle, WindowLongStyle, WS_EX_LAYERED);
            isClickable = true;

        }

    }


}
