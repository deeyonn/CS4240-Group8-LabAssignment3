using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulateDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<TMP_Dropdown.OptionData> options = new();

    private void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.RefreshShownValue();
    }
}
