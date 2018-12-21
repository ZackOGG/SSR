using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Systems
{
    public class Level_Manager : MonoBehaviour {

        //[SerializeField] BoxCollider2D levelTriggerOne;
        [SerializeField] GameObject wall;
        [SerializeField] HordeSpawner[] hordeSpawners; 

        private bool levelStarted = false;

        // Use this for initialization
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }

        public void LevelStartTriggerd()
        {
            if (!levelStarted)
            {
                print("Starting Level");
                levelStarted = true;
                wall.SetActive(true);
                StartHordeSpawners();
            }

        }

        private void StartHordeSpawners()
        {
            foreach(HordeSpawner spawner in hordeSpawners)
            {
                spawner.StartSpawningMobs();
            }
        }
    }
}