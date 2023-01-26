using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1: Enemy
{
    public void Start()
    {
        
    }

    public void Update()
    {
        Move();
        ChangeState();
    }

}
