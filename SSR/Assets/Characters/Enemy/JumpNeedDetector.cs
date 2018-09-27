﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.AI
{
    public class JumpNeedDetector : MonoBehaviour
    {
        public float jumpRadius;
        private AI_Movement aIMovement;

        private void Start()
        {
            aIMovement = this.GetComponentInParent<AI_Movement>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Ground" && this.gameObject.tag == "JumpBox" || collision.gameObject.tag == "Platform" && this.gameObject.tag == "JumpBox")
            {
                print("Hit wall");
                aIMovement.ProcessJump();
            }
        }
    }
}