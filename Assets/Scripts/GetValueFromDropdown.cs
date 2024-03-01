using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetValueFromDropdown : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;

    public void GetDropdownValue()
    {
        int selectedOptionIndex = dropdown.value;
        string selectedOption = dropdown.options[selectedOptionIndex].text;
        Debug.Log(selectedOption);
    }
}
