using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastNotification : MonoBehaviour {
    public static void PopUpMessage(string error) {
        Debug.Log(error);
        Transform current = GameObject.Find("ToastManager").transform;
        GameObject toast = Instantiate((GameObject)Resources.Load("Toast"), current);
        toast.transform.GetChild(0).GetComponent<Text>().text = error;
    }
}
