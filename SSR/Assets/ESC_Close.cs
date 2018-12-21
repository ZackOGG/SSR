using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESC_Close : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyUp(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
	}
}
