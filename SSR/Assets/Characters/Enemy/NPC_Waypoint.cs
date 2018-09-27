using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.AI
{
    public class NPC_Waypoint : MonoBehaviour
    {

        [SerializeField] GameObject pointOne;
        [SerializeField] GameObject pointTwo;

        private float movementSpeed = 5f;
        private float jumpPower = 5f;
        //private AI_Movement;
        private Rigidbody2D rb;
        private Character_Stats characterStats;
        private AI_Movement aIMovement;
        private bool movingToPointOne = true;
        // Use this for initialization
        void Start()
        {
            SetRefrences();
            SetStats();
        }

        private void SetRefrences()
        {
            rb = this.GetComponent<Rigidbody2D>();
            characterStats = this.GetComponent<Character_Stats>();
            aIMovement = this.GetComponent<AI_Movement>();
        }

        private void SetStats()
        {            
            jumpPower = characterStats.GetJumpPower();
        }

        // Update is called once per frame
        void Update()
        {
            ProcessMovement();
           
        }

        private void ProcessMovement()
        {
            if (!movingToPointOne) // TODO CHANGE DEPENDING ON THE LOOK DIRECTION
            {
                aIMovement.SetMoveLeft(false);
            }
            if (movingToPointOne)
            {
                aIMovement.SetMoveLeft(true);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject == pointOne)
            {
                movingToPointOne = false;
                
            }
            if(collision.gameObject == pointTwo)
            {
                movingToPointOne = true;
            }
        }

        //=====Direction Facing=====
        private void FaceRight()
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        private void FaceLeft()
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        //==========================

    }
}