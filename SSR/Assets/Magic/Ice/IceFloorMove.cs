using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloorMove : MonoBehaviour {

    [SerializeField] Transform[] stopLocations;    
    private int currentLocation = 0;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();

    }
    public void MoveUp()
    {
        if(currentLocation < stopLocations.Length)
        {
            currentLocation++;
        }
        
    }
    private void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, stopLocations[currentLocation].position, 1f * Time.deltaTime);
    }
}
