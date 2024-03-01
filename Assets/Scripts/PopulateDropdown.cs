// This script is used to populate the TextMeshPro dropdown with the folder
// names in the BigFurniturePack folder in the Assets folder. It ignores folder
// names that start with an underscore.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateDropdown : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;
    public string folderName = "BigFurniturePack";

    void Start()
    {
        Populate();
    }

    public void Populate()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        string[] folders = System.IO.Directory.GetDirectories("Assets/" + folderName);
        foreach (string folder in folders)
        {
            string folderName = folder.Substring(folder.LastIndexOf('\\') + 1);
            if (!folderName.StartsWith("_"))
            {
                options.Add(folderName);
            }
        }
        // Print the folder names to the console
        foreach (string option in options)
        {
            Debug.Log(option);
        }
        dropdown.AddOptions(options);
    }
}
