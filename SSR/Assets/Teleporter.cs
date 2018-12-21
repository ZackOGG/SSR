using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.Systems
{
    public class Teleporter : MonoBehaviour
    {


        [SerializeField] bool autoTeleport = false; //If true then the player will auto teliport when they come into contact with this
        [SerializeField] float fadeTime;
        [SerializeField] float fadeSpeed;

        private Transform destination;
        private GameObject collidedPlayer;
        private GameObject collidedPlayerCam;
        private bool playerClose;
        private Fade_Controller fadeContorller;
        private Player_Movement_TwoDSS portingCharacterMove;
        // Use this for initialization
        void Start()
        {
            SetRefrences();
        }

        private void SetRefrences()
        {
            destination = this.GetComponentInChildren<GizmoMaker>().transform;
            collidedPlayerCam = GameObject.FindGameObjectWithTag("CamController");
            fadeContorller = GameObject.FindObjectOfType<Fade_Controller>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.E) && playerClose || autoTeleport && playerClose)
            {
                StartTeleport();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                collidedPlayer = collision.gameObject;
                playerClose = true;
                portingCharacterMove = collision.GetComponent<Player_Movement_TwoDSS>();
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                collidedPlayer = null;
                playerClose = false;
                //portingCharacterStats = null;
            }
        }

        private void StartTeleport()
        {
            print("Teleporting");

            StartCoroutine("Teleport");
        }

        IEnumerator Teleport()
        {
            fadeContorller.StartFade(fadeTime, fadeSpeed);
            portingCharacterMove.SetCantMove(true);
            yield return new WaitForSeconds(fadeSpeed);
            collidedPlayer.transform.position = destination.transform.position;
            portingCharacterMove.SetCantMove(false);

        }

    }
}