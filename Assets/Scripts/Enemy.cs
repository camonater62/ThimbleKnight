using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
   [Header("Movement Variables")]

   [SerializeField] protected float distance = 10f;

   [Header("Target")]
   [SerializeField] protected GameObject player;


   protected bool moving = false;

   public float GetDistance() { return Vector3.Distance(transform.position, player.transform.position); }


   protected override void Awake()
   {
      hp = maxHP;
      rb = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      anim = GetComponent<Animator>();
   }

   public override void Move()
   {

      direction = transform.position.x < player.transform.position.x ? 1 : -1;
      rb.AddForce(Vector2.right * direction * speed, ForceMode2D.Force);
      rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), 0);
      spriteRenderer.flipX = direction == 1 ? true : false;
   }

   public void TakeDamage(Weapon weapon)
   {
      hp -= weapon.GetDmg();
      Debug.Log("taking damage");
      moving = false;
      stunned = true;
      if (hp <= 0)
      {
         gameObject.SetActive(false);
      }
      else
      {
         StartCoroutine(Stunned(weapon));
      }
   }

   IEnumerator Stunned(Weapon weapon)
   {
      yield return new WaitForSeconds(0.5f);
      rb.AddForce(new Vector2(weapon.GetKnockback(), 0) * (-direction), ForceMode2D.Impulse);
      yield return new WaitForSeconds(1);

      moving = true;
      stunned = false;
      rb.velocity = Vector2.zero;
   }


   // private void OnTriggerEnter2D(Collider2D other)
   // {
   //     if (other.tag == "Player")
   //     {
   //         Attack();
   //     }
   // }
}