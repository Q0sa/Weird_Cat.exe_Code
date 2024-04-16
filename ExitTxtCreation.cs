using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExitTxtCreation : MonoBehaviour
{

    [SerializeField] string ContentLine1 = "Ⓨ𝕠υ'Řᵉ ⓐ 𝕄υŘＤᵉŘᵉŘ";
    [SerializeField] string BinaryURLLine1 = "01101000011101000111010001110000011100110011101000101111001011110111100101101111011101010111010001110101001011100110001001100101001011110110000101011010010011100011001101100101011011100100101101110100011011100101101000111000";
    
    void OnApplicationQuit() {

        string path = Directory.GetCurrentDirectory();
        string fileName = @path + "\\( ⓛ ω ⓛ ).txt";


        using (StreamWriter sw = File.CreateText(fileName))
        {
            sw.WriteLine(ContentLine1);
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine(BinaryURLLine1);


        }

    }

}
