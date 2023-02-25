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
      if (GetDistance() < distance)
      {
         moving = true;
         anim.SetBool("Moving", true);

      }
      if(moving) {
         Move();
      }
    }

   // private void OnTriggerEnter2D(Collider2D other) {
   //    if(other.tag == "Player") {
   //       Player p = other.gameObject.GetComponent<Player>();
   //       if(!_collided) {
   //          p.TakeDamage(this);
   //          _collided = true;
   //       }
   //    }
   // }

   IEnumerator AttackDelay() {
      yield return new WaitForSeconds(1f);
      _collided = false;
   }
}
