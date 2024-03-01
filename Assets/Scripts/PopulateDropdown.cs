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
        List<string> options = new List<string>();
        options.Add("Bathroom");
        options.Add("Beds");
        options.Add("Cabinets & Racks");
        options.Add("Lights");
        options.Add("Mirrors");
        options.Add("Modular Kitchen");
        options.Add("Sofas & Chairs");
        options.Add("Tables");
        options.Add("Vases");
        dropdown.AddOptions(options);
    }
}
