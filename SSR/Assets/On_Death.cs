using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Death : MonoBehaviour {

    [SerializeField] GameObject restartBTN;

    public void OnDestroy()
    {
        ShowRestartBTN();
    }

    private void ShowRestartBTN()
    {
        restartBTN.SetActive(true);
    }
}
