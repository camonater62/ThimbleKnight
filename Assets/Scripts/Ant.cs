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

    public override void Attack() {
        StartCoroutine(Attacked());
    }

   IEnumerator AttackDelay() {
      moving = false;
      yield return new WaitForSeconds(10f);
      _collided = false;
      moving = true;
   }
}
