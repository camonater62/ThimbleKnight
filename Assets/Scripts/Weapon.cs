using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   [Header("Stats:")]
   [SerializeField] protected float damage;
   [SerializeField] protected float knockback;
   [SerializeField] private float _attackDelay = 1f;

   private Animator anim;

   private bool _attacking = false;
   private bool _collided = false;

   private BoxCollider2D _hitbox;

   public float GetKnockback() { return knockback; }
   public float GetDmg() { return damage; }

   public void Start() {
      _hitbox = GetComponent<BoxCollider2D>();
      _hitbox.enabled = false;
   }
   public void Attack() {
      if(!_attacking) {
         _hitbox.enabled = true;
         StartCoroutine(Attacking());
      }
   }

   private void OnTriggerEnter2D(Collider2D other) {
      if (other.tag == "Enemy") {
         Enemy enemy = other.gameObject.GetComponent<Enemy>();
         if (!_collided) {
            enemy.TakeDamage(this);
            _collided = true;
         }
      }
   }

   IEnumerator Attacking() {
      _attacking = true;
      yield return new WaitForSeconds(_attackDelay);
      _attacking = false;
      _collided = false;
      _hitbox.enabled = false;
      
   }
}
