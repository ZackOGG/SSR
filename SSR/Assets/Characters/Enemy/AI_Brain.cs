using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;
using System;


namespace SS.AI
{
    public class AI_Brain : MonoBehaviour
    {
        [SerializeField] float agroRange = 2.5f;
        [SerializeField] bool hordeMode = false;

        public bool inCombat = false;
        public bool patrolling = true;

        //Scripts
        private NPC_Waypoint wayPointSrpt;
        private AI_Combat_Standard aICombat;
        public GameObject target;

        //=====Getters and setters=====
        public float GetAgroRange() { return agroRange; }
        public GameObject GetTarget() { return target; }

        //=============================
        
        // Use this for initialization
        void Start()
        {
            SetRefrences();
            
            if(!hordeMode && !target)
            {
                Patrol();
            }
            else
            {
                SetHordeMode();
            }
            
        }

        private void SetRefrences()
        {
            wayPointSrpt = this.GetComponent<NPC_Waypoint>();
            aICombat = this.GetComponent<AI_Combat_Standard>();
        }
        private void SetHordeMode()
        {
            Combat();
            target = GameObject.FindGameObjectWithTag("Player");
            aICombat.SetTarget(target);
            
        }


        // Update is called once per frame
        void Update()
        {
            DetectEnemy();
            if(!hordeMode && target)
            {
                InAgroRange();
            }

            print(target);

        }

        private void Patrol()
        {
            print("Patrol");
            TurnOffScripts();
            patrolling = true;
            wayPointSrpt.enabled = true;
        }
        private void Combat()
        {
            print("Combat");
            TurnOffScripts();
            inCombat = true;
            aICombat.enabled = true;
        }

        public void TurnOffScripts()
        {
            wayPointSrpt.enabled = false;
            patrolling = false;
            aICombat.enabled = false;
            inCombat = false;
        }

        private void InAgroRange()
        {
            var distance = target.transform.position - this.transform.position;
            if(distance.magnitude < agroRange)
            {
                if (!inCombat)
                {
                    Combat();
                }                
            }
            else
            {
                if (!patrolling)
                {
                    Patrol();
                }
            }
        }
        private void DetectEnemy()
        {
            bool foundTarget = false;
            bool targetStillInRange = false;
            Collider2D[] hitColliders;
            hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), agroRange);
            
            foreach (Collider2D hit in hitColliders)
            {
                if (hit.tag == "Player" && !target)
                {
                    print("Player!");
                    target = hit.gameObject;
                    foundTarget = true;                    
                    TurnOffScripts();
                    Combat();
                    aICombat.SetTarget(hit.gameObject);
                    
                }
                if(hit.tag == "Player")
                {
                    foundTarget = true;
                }
                if(foundTarget == false)
                {
                    target = null;
                }
            }
            if(!target)
            {
                target = null;
                if(!patrolling)
                {
                    Patrol();
                }
                
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, agroRange);

        }
    }
}