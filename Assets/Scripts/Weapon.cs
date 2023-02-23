using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   [Header("Stats:")]
   [SerializeField] protected float damage;
   [SerializeField] protected float knockback;

   private Animator anim;
   private Animation a;

   private bool attacking = false;

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
      if (!attacking) {
         anim.CrossFadeInFixedTime("attack", 0.1f);
         attacking = true;
         // StartCoroutine(Attacking());
      }
   }

   private void OnTriggerStay2D(Collider2D other) {
      if (other.tag == "Enemy") {
         Enemy enemy = other.gameObject.GetComponent<Enemy>();
         if (attacking) {
            enemy.TakeDamage(this);
         }
      }
   }

   // IEnumerator Attacking() {
   //    // yield return new WaitForSeconds(anim.);
   //    attacking = false;
   // }
}
