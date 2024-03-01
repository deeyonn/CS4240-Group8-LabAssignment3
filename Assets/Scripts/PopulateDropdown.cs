using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateDropdown : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;

    void Start()
    {
        Populate();
    }

    public void Populate()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "Bathroom",
            "Beds",
            "Cabinets & Racks",
            "Lights",
            "Mirrors",
            "Modular Kitchen",
            "Sofas & Chairs",
            "Tables",
            "Vases"
        };
        dropdown.AddOptions(options);
    }
}
