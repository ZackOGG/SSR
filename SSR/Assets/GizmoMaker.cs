using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoMaker : MonoBehaviour {

    [SerializeField] float gizmoSize;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(this.transform.position, gizmoSize);
    }
}
