using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   [Header("Stats:")]
   [SerializeField] protected float damage;
   [SerializeField] protected float knockback;

   private Animator anim;
   private AudioSource _contact;

   private bool _attacking = false;
   private bool _collided = false;

   private BoxCollider2D _hitbox;

   public float GetKnockback() { return knockback; }
   public float GetDmg() { return damage; }

   public void Start() {
      _hitbox = GetComponent<BoxCollider2D>();
      _contact = GetComponent<AudioSource>();
   }
   public void Attack(float attackDelay) {
      if(!_attacking) {
         StartCoroutine(Attacking(attackDelay));
      }
   }

   private void OnTriggerStay2D(Collider2D other) {
      if (other.tag == "MeleeEnemy" || other.tag == "RangedEnemy") {
         Enemy enemy = other.gameObject.GetComponent<Enemy>();
         if (!_collided && _attacking) { //avoids taking multiple damage
            enemy.TakeDamage(this);
            _collided = true;
            _contact.Play();
         }
      }
   }

   IEnumerator Attacking(float attackDelay) {
      _attacking = true;
      yield return new WaitForSeconds(attackDelay);
      _attacking = false;
      _collided = false;      
   }
}
