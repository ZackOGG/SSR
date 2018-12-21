using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Systems
{
    public class Trigger_Controller : MonoBehaviour
    {

        private Level_Manager levelManager;


        // Use this for initialization
        void Start()
        {
            levelManager = this.GetComponentInParent<Level_Manager>();
        }

        // Update is called once per frame

        private void OnTriggerEnter2D(Collider2D collision)
        {
            levelManager.LevelStartTriggerd();
        }
    }
}