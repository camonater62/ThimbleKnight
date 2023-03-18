using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryThimble : Enemy
{
    private bool _attached = false;
    // Update is called once per frame
    void Update()
    {
        if (GetDistance() < distance && !_attached && !stunned)
        {
            moving = true;
            anim.SetBool("Moving", true);
        } else {
            anim.SetBool("Moving", false);
        }
        if (moving)
        {
            Move();
        }
        if (_attached)
        {
            transform.position = player.transform.position + new Vector3(0, 2, 0);
        }
    }
    public override void TakeDamage(Weapon weapon)
    {
        _attached = false;
        base.TakeDamage(weapon);
        player.GetComponent<Player>().attached = false;
    }
    public override void Attack()
    {
        if (!_attached)
        {
            _attached = true;
            moving = false;
            attacked = true;
            anim.SetBool("Moving", false);
            Player p = player.GetComponent<Player>();
            p.TakeDamage(this);
            p.attached = true;
        }
    }
}
