using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Door_Controller : MonoBehaviour {

    public GameObject obj;
    private Animator anim;
    public bool v_OpenDoor;
    public bool v_CloseDoor;
    public bool isDoorOpen;

    private void Start()
    {
        Assert.AreNotEqual(v_CloseDoor, v_OpenDoor);
        anim = obj.GetComponentInChildren<Animator>();
        if(v_OpenDoor)
        {
            isDoorOpen = false;
        }
        else
        {
            isDoorOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {               
        if(v_OpenDoor && !isDoorOpen)
        {
            isDoorOpen = true;
            OpenDoor();
        }
        if(v_CloseDoor && isDoorOpen)
        {
            isDoorOpen = false;
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        anim.SetTrigger("OpenDoor");
    }
    public void CloseDoor()
    {
        anim.SetTrigger("CloseDoor");
    }

}
	
	
