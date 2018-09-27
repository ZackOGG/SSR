using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Shield_Controller : MonoBehaviour {


    public float transitionSpeed;
    private SpriteRenderer spriteRen;
    private float currentA;
    private bool turnOn;
    public void SetTurnOn(bool newBool) { turnOn = newBool; }
    private CircleCollider2D forceFieldBox;

    private void Start()
    {
        spriteRen = this.GetComponent<SpriteRenderer>();
        currentA = 1;
        turnOn = true;
        forceFieldBox = this.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (turnOn)
        {
            forceFieldBox.enabled = true;
            if(currentA <= 1)
            {                
                currentA = currentA += (transitionSpeed * Time.deltaTime) / 60;
                spriteRen.color = new Color(255, 255, 255, currentA);
            }
            else
            {
                currentA = 1;
            }
            
        }
        else
        {
            forceFieldBox.enabled = false;
            if (currentA >= 0)
            {
                currentA = currentA -= (transitionSpeed * Time.deltaTime) / 60;
                spriteRen.color = new Color(255, 255, 255, currentA);
            }
            else
            {
                currentA = 0;
            }
        }
        

        spriteRen.color = new Color(255, 255, 255, currentA);
    }

}
