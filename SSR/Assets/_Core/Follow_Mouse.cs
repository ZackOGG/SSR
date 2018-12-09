using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Mouse : MonoBehaviour {

	[SerializeField] static Vector3 mousePosision;
    public static GameObject theMouseCursor;

    public Vector2 GetPositionVTwo()
    {
        return transform.position;
    }

    // Update is called once per frame
    private void Start()
    {
        theMouseCursor = this.gameObject;
    }
    void Update ()
    {

        mousePosision = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePosision;
	}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(this.transform.position, 1f);
    }
}
