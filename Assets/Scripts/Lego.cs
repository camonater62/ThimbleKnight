using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : Enemy
{
    private bool _awake = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetDistance() < distance && !awake) {
            WakeUp();
        }
        if(moving) {
            Move();
        }
    }
    }

