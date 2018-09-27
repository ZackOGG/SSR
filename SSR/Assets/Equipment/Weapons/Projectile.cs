using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Character;

namespace SS.Equipment
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Stats")]
        [SerializeField] float movementSpeed;
        [SerializeField] int damage;
        public void SetDamage(int newDamage) { damage = newDamage; }
        [SerializeField] float autoDestroyTimer;

        private Vector2 direction;
        public Transform target;
        public void SetTarget(Transform newTarget) { target = newTarget; }
        private Rigidbody2D rb;
        // Use this for initialization
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        private void SetDirection(Vector2 newDirection)
        {
            direction = newDirection;
        }

        // Update is called once per frame
        void Update()
        {
            AutoDestory();
            if (target)
            {
                MoveToTarget();
            }
            else
            {
                MoveDirection(direction);
            }
        }
        private void MoveToTarget()
        {
            //Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
            Vector3 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20f * Time.deltaTime);

            this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, movementSpeed * Time.deltaTime);
        }
        public void MoveDirection(Vector2 newDirection)
        {
            target = null;
            direction = newDirection;;
            if(rb)
            {
                rb.velocity = new Vector2 (direction.x * movementSpeed, direction.y * movementSpeed);
                //rb.velocity = new Vector2(h * movementSpeed, rb.velocity.y);
            }
            
            

            Vector3 lookDirection = new Vector3(newDirection.x,newDirection.y,0);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20f * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character_Stats>())
            {
                collision.GetComponent<Character_Stats>().TakeDamage(damage);
                DestroyThis();
            }                
        }
        private void AutoDestory()
        {
            autoDestroyTimer -= Time.deltaTime;
            if (autoDestroyTimer <= 0)
            {
                DestroyThis();
            }
        }
        private void DestroyThis()
        {
            Destroy(this.gameObject);
        }

    }
}