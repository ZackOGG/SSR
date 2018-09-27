using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;
using SS.Equipment;

namespace SS.AI
{
    public class Zarok_Battle_Controller : MonoBehaviour {

        [Header("Boss Move Locations")]
        [SerializeField] Transform endWakeLocation;
        [SerializeField] Transform topOfRoom;
        [SerializeField] Transform nEPlatform;
        [SerializeField] Transform centerPlatform;
        [Header("Start Wait Timers")]
        [SerializeField] float pointTwoFloatToTop = 6f;
        [SerializeField] float pointThreeBeforeFF = 5f;
        [SerializeField] float toStageOneOneTimer = 20f;

        [Header("Boss Stage Timers")]
        [SerializeField] float stageOneTimer; //How long stage one will last before it goes to one one
        [SerializeField] float stageOneOneTimer; // How long stage one one will last before it goes to one
        [SerializeField] int healthTillStageTwo;
        [SerializeField] float stageTwoTimer;
        [SerializeField] float stageTwoOneTimer;
        [SerializeField] int healthTillStageThree;
        [SerializeField] float stageThreeTimer;
        [SerializeField] float stageThreeOneTimer;

        [Header("Attack Prefabs")]
        [SerializeField] GameObject fireBall;


        [Header("Attack Locations")]
        [SerializeField] Transform[] topAttackSpawnLocations;
        [SerializeField] Transform[] leftAttackSpawnLocations;
        [SerializeField] Transform[] rightAttackSpawnLocations;

        [Header("Attack timers")]
        [SerializeField] float stageOneFireBallAttackTimerTop;
        [SerializeField] float stageOneFireBallAttackTimerSide;
        private float stageOneFireBallCurrentTimerTop = 0;
        private float stageOneFireBallCurrentTimerSide = 0;

        [Header("Attack Stats")]
        [SerializeField] int fireBallDamage;
        [SerializeField] GameObject lightningSpin;
        [SerializeField] IceFloorMove iceFloor;
        [SerializeField] Lightning_Shield_Controller lightningShielCon;


        private float moveSpeed;
        private bool preStage = true; // This is the stage before the player enters the room
        public bool stageZero;// This is the stage where the boss awake is played and set himself up for the fight
        public bool stageOne; // This is the Stage where he shoots fire up and down the platforms
        public bool stageOneOne; // this is the stage where he goes to a platform and shoots fire
        public bool stageTwo; // This is the Stage where he shoots lightning accross the platforms
        public bool stageTwoOne;
        public bool stageThree; // This is the Stage where he shoots both fire down the platforms and lightning across the lanes
        public bool stageThreeOne;
        private bool dying;
        private Animator anim;
        private Character_Stats characterStats;
        private Transform targetLocation;
        public void SetTargetLocation(Transform newTarget)
        {
            targetLocation = newTarget;
        }
        // Use this for initialization
        void Start()
        {
            SetRefrences();
            SetStats();
        }
        private void SetRefrences()
        {
            anim = this.GetComponent<Animator>();
            characterStats = this.GetComponent<Character_Stats>();
            lightningShielCon = this.GetComponentInChildren<Lightning_Shield_Controller>();
        }
        private void SetStats()
        {
            
            targetLocation = this.transform;
        }
        private int GetHeathPercentAsInt()
        {
            float currentHealth = characterStats.GetCurrentHealth();
            float startingHealth = characterStats.GetMaxHealth() * 4;;
            float answer = (currentHealth / startingHealth) * 100;
            return Mathf.RoundToInt(answer);
        }

        // Update is called once per frame
        void Update()
        {            
            moveSpeed = characterStats.GetMovementSpeed();
            MoveToLocation();
            if (stageOne)
            {
                ShootingFireBallsFromRoof();
                ShootingFireBallsFromSides();
            }            
            if(stageThree)
            {
                ShootingFireBallsFromRoof();
                ShootingFireBallsFromSides();
            }

            if(GetHeathPercentAsInt() < healthTillStageTwo && !stageTwo && !stageTwoOne && !stageThree && !stageThreeOne && !dying)
            {
                StopAllCoroutines();
                TurnOffAllStages();
                stageTwo = true;
                StartStageTwo();
            }
            if (GetHeathPercentAsInt() < healthTillStageThree && !stageThree && !stageThreeOne && !dying)
            {                
                StopAllCoroutines();    
                StartStageThree();
            }
            if(characterStats.GetCurrentHealth() <= 0 && !dying)
            {
                StopAllCoroutines();
                TurnOffAllStages();
                TurnOffAllAnimations();
                Die();
            }


        }
        private void TurnOffAllStages()
        {
            preStage = false;
            stageZero = false;
            stageOne = false;
            stageOneOne = false;
            stageTwo = false;
            stageTwoOne = false;
            stageThree = false;
            stageThreeOne = false;
        }
        private void TurnOffAllAnimations()
        {
            anim.SetBool("FightStarted", false);
            anim.SetBool("CastingFire", false);
            anim.SetBool("Moving", false);
            anim.SetBool("CastingLightning", false);
            anim.SetBool("Resting", false);
            anim.SetBool("BlastingOffAgain", false);
        }
        private void MoveToLocation()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetLocation.position, moveSpeed * Time.deltaTime);
        }
        public void StartStageZero() //1
        {
            TurnOffAllStages();
            characterStats.SetInvunerable(true);
            stageZero = true;
            targetLocation = endWakeLocation;
            anim.SetBool("FightStarted", true);

            StartCoroutine("EndStageZero");
        }
        IEnumerator EndStageZero() //2
        {
            //SAY SOME WORDS HERE
            yield return new WaitForSeconds(pointTwoFloatToTop);
            targetLocation = topOfRoom;
            yield return new WaitForSeconds(pointThreeBeforeFF);
            StartStageOne();
        }
        private void StartStageOne()
        {
            targetLocation = topOfRoom;
            iceFloor.MoveUp();
            characterStats.SetInvunerable(true);
            lightningShielCon.SetTurnOn(true);
            anim.SetBool("CastingFire", false);
            StartCoroutine("StageOne");
        }
        IEnumerator StageOne()
        {
            yield return new WaitForSeconds(5f); // going to top of room
            TurnOffAllStages();
            stageOne = true;
            anim.SetBool("Resting", false);
            anim.SetBool("CastingFire", true);
            lightningShielCon.SetTurnOn(true);
            characterStats.SetInvunerable(true);
            yield return new WaitForSeconds(stageOneTimer);
            //Stage One One
            anim.SetBool("CastingFire", false);
            TurnOffAllStages();
            stageOneOne = true;
            targetLocation = centerPlatform;
            yield return new WaitForSeconds(7f);// moving down
            anim.SetBool("Resting", true);
            characterStats.SetInvunerable(false);
            lightningShielCon.SetTurnOn(false);
            yield return new WaitForSeconds(stageOneOneTimer);
            StartStageOne();

        }
        private void StartStageTwo()
        {
            characterStats.SetInvunerable(true);
            lightningShielCon.SetTurnOn(true);
            SetTargetLocation(centerPlatform);
            StartCoroutine("StageTwo");
        }
        IEnumerator StageTwo()
        {
            anim.SetBool("Resting", false);
            anim.SetBool("CastingLightning", true);
            characterStats.SetInvunerable(true);
            lightningShielCon.SetTurnOn(true);
            yield return new WaitForSeconds(2f); // Winding up        
            EnableLightningSpin();
            yield return new WaitForSeconds(stageTwoTimer);
            //Stage Two Two
            DisableLightningSpin();
            anim.SetBool("CastingLightning", false);
            TurnOffAllStages();
            stageTwoOne = true;
            anim.SetBool("Resting", true);
            characterStats.SetInvunerable(false);
            lightningShielCon.SetTurnOn(false);
            yield return new WaitForSeconds(stageOneOneTimer);
            StartStageTwo();
        }
        private void StartStageThree()
        {
            TurnOffAllStages();
            stageThree = true;
            characterStats.SetInvunerable(true);
            lightningShielCon.SetTurnOn(true);
            StartCoroutine("StageThree");
        }
        IEnumerator StageThree()
        {
            anim.SetBool("Resting", false);
            anim.SetBool("CastingFire", true);
            characterStats.SetInvunerable(true);
            lightningShielCon.SetTurnOn(true);
            yield return new WaitForSeconds(2f); // Winding up        
            EnableLightningSpin();
            yield return new WaitForSeconds(stageThreeTimer);
            //Stage Three One
            DisableLightningSpin();
            anim.SetBool("CastingFire", false);
            TurnOffAllStages();
            stageThreeOne = true;
            anim.SetBool("Resting", true);
            characterStats.SetInvunerable(false);
            lightningShielCon.SetTurnOn(false);
            yield return new WaitForSeconds(stageThreeOneTimer);
            StartStageThree();
        }


        private Transform RandomShootLocationTop()
        {
            int ranNum = Random.Range(0, topAttackSpawnLocations.Length);
            return topAttackSpawnLocations[ranNum];
        }
        private Transform RandomShootLocationLeftSide()
        {
            int ranNum = Random.Range(0, leftAttackSpawnLocations.Length);
            return leftAttackSpawnLocations[ranNum];
        }
        private Transform RandomShootLocationRightSide()
        {
            int ranNum = Random.Range(0, rightAttackSpawnLocations.Length);
            return rightAttackSpawnLocations[ranNum];
        }
        private void ShootingFireBallsFromRoof()
        {
            stageOneFireBallCurrentTimerTop -= Time.deltaTime;
            if (stageOneFireBallCurrentTimerTop <= 0)
            {
                stageOneFireBallCurrentTimerTop = stageOneFireBallAttackTimerTop;
                ShootFireBall(RandomShootLocationTop(), Vector2.down);
            }            
        }
        private void ShootingFireBallsFromSides()
        {
            stageOneFireBallCurrentTimerSide -= Time.deltaTime;
            int randomNumber;

            randomNumber = Random.Range(0, 2);
            if(randomNumber == 0 && stageOneFireBallCurrentTimerSide <= 0)
            {                
                stageOneFireBallCurrentTimerSide = stageOneFireBallAttackTimerSide;
                ShootFireBall(RandomShootLocationLeftSide(), Vector2.right);


            }
            if (randomNumber == 1 && stageOneFireBallCurrentTimerSide <= 0)
            {
                stageOneFireBallCurrentTimerSide = stageOneFireBallAttackTimerSide;
                ShootFireBall(RandomShootLocationRightSide(), Vector2.left);
            }
        }        
        
        private void ShootFireBall(Transform target, Vector2 newDirection)
        {
            GameObject fireBallObj = (GameObject)Instantiate(fireBall, target);
            Projectile fireBallProj = fireBallObj.GetComponent<Projectile>();
            fireBallProj.SetDamage(fireBallDamage);           
            fireBallProj.MoveDirection(newDirection);
            

        }
        private void EnableLightningSpin()
        {
            print("Enable lightning");
            lightningSpin.SetActive(true);
        }
        private void DisableLightningSpin()
        {
            lightningSpin.SetActive(false);
        }

        private void Die()
        {
            characterStats.SetInvunerable(true);
            anim.SetBool("BlastingOffAgain", true);
            dying = true;
            DisableLightningSpin();
            TurnOffAllStages();
            StopAllCoroutines();
            StartCoroutine("DestroyBoss");
        }
        IEnumerator DestroyBoss()
        {            
            yield return new WaitForSeconds(3.85f);
            Destroy(this.gameObject);
        }
    }
}