using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Camera_UI
{
    public class Camera_Follow : MonoBehaviour
    {

        public GameObject followObject; // This will be the player object for the camera to follow.

        // Use this for initialization
        void Start()
        {
            //followObject = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void LateUpdate()
        {

            this.transform.position = new Vector3(followObject.transform.position.x, Mathf.Clamp(followObject.transform.position.y,0f,100f), this.transform.position.z);


        }
    }
}
