using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class OpeningErrorScript : MonoBehaviour
{
    //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr window_handle, string message, string cap, uint type); //Import outside method (basically forward declaration but not really)
    const uint MB_ICONERROR = 0x00000010;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [SerializeField]private string PopUpTitle = "Fatal Error!";
   
    void Awake()
    {

        string path = Directory.GetCurrentDirectory();

        MessageBox(GetActiveWindow(), "The file in directory " + "\n'" + path + "' " + "\nis corrupted!\nRemove it and launch program again! \n[Cat out of bounds!]", PopUpTitle, 0 | MB_ICONERROR | 0x00002000);

    }


}
