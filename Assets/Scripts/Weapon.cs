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

   public float GetKnockback() { return knockback; }
   public float GetDmg() { return damage; }
   private void Awake() {
      anim = GetComponent<Animator>();
   }
   // Start is called before the first frame update
   void Start() {

   }

   // Update is called once per frame
   void Update() {
   }

   public void Attack() {
      if(!_attacking) {
         anim.CrossFadeInFixedTime("attack", 0.1f);
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
   }
}
