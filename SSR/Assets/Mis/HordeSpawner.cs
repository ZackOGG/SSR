using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.Systems
{
    public class HordeSpawner : MonoBehaviour
    {

        [SerializeField] float spawnTimer;
        [SerializeField] GameObject mob;
        [SerializeField] bool automaited = true;

        public int amountOfMobs = 0;

        // Use this for initialization
        void Start()
        {
            if (!automaited)
            {
                InvokeRepeating("SpawnMob", spawnTimer, spawnTimer);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                CallculateAmountOfMobs();
            }
        }

        public void StartSpawningMobs()
        {
            InvokeRepeating("SpawnMob", spawnTimer, spawnTimer);
        }


        private void SpawnMob()
        {
            GameObject theMob = Instantiate(mob, this.transform).gameObject;
            theMob.SetActive(true);
            theMob.transform.position = this.transform.position;
        }

        private void CallculateAmountOfMobs()
        {
            amountOfMobs = transform.childCount;
        }
    }

}