using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDelete : MonoBehaviour {
    void Start() {
        StartCoroutine("Delete");
    }

    IEnumerator Delete() {
        yield return new WaitForSecondsRealtime(2.0f);
        GameObject.Destroy(this.gameObject);
    }
}
