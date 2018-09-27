using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour {

    public Transform parentTrans;

	// Use this for initialization
	void Start ()
    {
        StartJitter();
        
    }
	
	// Update is called once per frame
	private void StartJitter()
    {
        StopAllCoroutines();
        StartCoroutine("TheJitter");
    }
    IEnumerator TheJitter()
    {
        this.transform.position = new Vector3((parentTrans.position.x + 0.01f), this.transform.position.y, this.transform.position.z);
        yield return new WaitForSeconds(0.1f);
        this.transform.position = new Vector3((parentTrans.position.x - 0.01f), this.transform.position.y, this.transform.position.z);
        yield return new WaitForSeconds(0.1f);
        StartJitter();
    }
}
