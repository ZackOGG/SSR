using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Character
{
    public class JumpUpDetector : MonoBehaviour
    {
        private Player_Movement_TwoDSS playerMove;
        private GameObject player;
        

        // Use this for initialization
        void Start()
        {
            playerMove = this.GetComponentInParent<Player_Movement_TwoDSS>();
            player = playerMove.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (this.gameObject.tag == "JumpUpBox" && collision.gameObject.tag == "Ground")
            {
                StartCoroutine("HitBoxOffOn");
                
            }
        }
        IEnumerator HitBoxOffOn()
        {

            player.layer = LayerMask.NameToLayer("Player_NoPlatforms");
            yield return new WaitForSeconds(0.08f);
            //playerMove.TurnColliderOn();
        }

    }
}