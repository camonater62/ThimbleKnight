using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant: Enemy
{
   private bool _collided = false;
    public void Start()
    {
        
    }

    public void Update() {
      if (GetDistance() < distance && !attacked && !stunned)
      {
         moving = true;
         anim.SetBool("Moving", true);

      }
      if(moving) {
         Move();
      }
    }

   // private void OnCollisionEnter2D(Collision2D col) {
   //    if(col.gameObject.tag == "Player") {
   //       // Player p = other.gameObject.GetComponent<Player>();
   //       if(!_collided) {
   //          // p.TakeDamage(this);
   //          _collided = true;
   //          AttackDelay();
   //       }
   //    }
   // }

   IEnumerator AttackDelay() {
      moving = false;
      yield return new WaitForSeconds(10f);
      _collided = false;
      moving = true;
   }
}
