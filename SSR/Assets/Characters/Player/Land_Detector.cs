using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Character
{
    public class Land_Detector : MonoBehaviour
    {
        private Player_Movement_TwoDSS playerMovement;
        private GameObject player;

        private void Start()
        {            
            playerMovement = this.GetComponentInParent<Player_Movement_TwoDSS>();
            player = playerMovement.gameObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(this.gameObject.tag == "TurnOnColliderHitBox" && collision.gameObject.tag == "Ground")
            {
                player.layer = LayerMask.NameToLayer("Player");
                playerMovement.SetGrounded(true);
                playerMovement.SetPlatform(false);
            }
            if(this.gameObject.tag == "TurnOnColliderHitBox" && collision.gameObject.tag == "Platform")
            {
                player.layer = LayerMask.NameToLayer("Player");
                playerMovement.SetGrounded(true);
                playerMovement.SetPlatform(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (this.gameObject.tag == "TurnOnColliderHitBox" && collision.gameObject.tag == "Ground")
            {                
                playerMovement.SetGrounded(false);
                playerMovement.SetPlatform(false);
            }
            if (this.gameObject.tag == "TurnOnColliderHitBox" && collision.gameObject.tag == "Platform")
            {                
                playerMovement.SetGrounded(false);
                playerMovement.SetPlatform(false);
            }

        }
    }
}