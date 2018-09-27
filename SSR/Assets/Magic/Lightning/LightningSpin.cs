using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

public class LightningSpin : MonoBehaviour {

    [SerializeField] float spinSpeed = 1f;
    [SerializeField] int damage = 1;
    public void SetDamage(int newDamage) { damage = newDamage; }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Spin();

    }
    public void Spin()
    {
        float degreesPerSecond = (Time.deltaTime * spinSpeed) * 6;
        this.transform.RotateAround(this.transform.position, transform.forward, -degreesPerSecond);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Character_Stats>())
        {
            collision.GetComponent<Character_Stats>().TakeDamage(damage);
        }
    }
}
