using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.AI
{
    public class StartBossFight : MonoBehaviour
    {
        public Zarok_Battle_Controller bossCon;
        public float delay = 3f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine("DelayStarting");
        }
        IEnumerator DelayStarting()
        {
            yield return new WaitForSeconds(delay);
            bossCon.StartStageZero();
            StopAllCoroutines();
            Destroy(this);
        }
    }
}