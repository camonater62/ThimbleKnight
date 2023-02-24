using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant: Enemy
{
    public void Start()
    {
        
    }

    public void Update() {
      if (GetDistance() < distance && !awake)
      {
         WakeUp();

      }
      if(moving) {
         Move();
      }
    }
}
