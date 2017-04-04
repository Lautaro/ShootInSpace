using UnityEngine;
using System.IO;


// Creates an physical enum script file with all assets from source folder and saves it in outPut path. 
public class EnumGeneratorTool : MonoBehaviour
{
    /// <summary>
    /// Sfxs must be below a folder called Resources which must be below Assets folder
    /// If SFX is in "Assets/Resources/Sfxs" then source path should be set to "Sfxs"    
    /// </summary>
    [SerializeField]
    string sourcePath;


/// <summary>
/// Path to directory where enum file will be stored. 
/// If you want to 
/// </summary>
    [SerializeField]
    string outputPath;

    [SerializeField]
    string enumName;


    [ContextMenu("Create Enum")]
     void CreateEnum()
    {

        // Open enum
        var enumText = "public enum " + enumName + " { \n";

        var allSfxInFolder = Resources.LoadAll<AudioClip>(sourcePath);

        // Go through alls sfxa and make enums
        for (int i = 0; i < allSfxInFolder.Length; i++)
        {
            var name = allSfxInFolder[i].name;

            //if name contains spaces, throw error
            if (name.Contains(" "))
            {
                Debug.LogError("File names can not contain spaces");
            }

            enumText += name;

            if (i < allSfxInFolder.Length -1)
            {// If its not the last sfx
                enumText += ", ";
            }

            enumText += " \n";
        }


        // Close enum
        enumText += "\n }";

        var savePath = "";

        if (!string.IsNullOrEmpty( outputPath))
        {
            savePath = outputPath.Trim() + "/";
        }

        var path = Application.dataPath + "/" + savePath + enumName + ".cs";

        Debug.Log("[" + enumName.ToUpper() + "]");
        Debug.Log("Enum created. Nr of sfx: " + allSfxInFolder.Length );
        Debug.Log("File saved in path " + path);
        Debug.Log("Unity can take a moments to update and show the file. If it doesnt try and open the folder in the file explorer. ");

        File.WriteAllText(path, enumText);
                
    }
}
