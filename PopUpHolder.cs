using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[System.Serializable]
public class PopUpHolder : MonoBehaviour
{

    //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox


    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr window_handle, string message, string cap, uint type); //Import outside method (basically forward declaration but not really)

    public enum TypeOfButtons
    {

        OK_Button = 0x00000000,
        YESNO_Button = 0x00000004

    }

    public enum TypeOfIcon
    {

        NONE = 0, 
        ERROR_Icon = 0x00000010,
        QUESTION_Icon = 0x00000020,
        WARNING_Icon = 0x00000030,
        EXCLAMATION_Icon = 0x00000040

    }


    public int CreatePopUp(IntPtr window_handle, TypeOfButtons button_type, TypeOfIcon icon_type, string title, string content) {

        if(icon_type == TypeOfIcon.NONE)
            return MessageBox(window_handle, content, title, (uint)button_type);
        else
            return MessageBox(window_handle, content, title, (uint)button_type | (uint)icon_type);


    }
}
