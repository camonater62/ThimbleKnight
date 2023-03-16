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
        base.TakeDamage(weapon);
        _attached = false;
        player.GetComponent<Player>().attached = false;
    }
    public override void Attack()
    {
        Debug.Log(_attached);
        if (!_attached)
        {
            _attached = true;
            moving = false;
            attacked = true;
            anim.SetBool("Moving", false);
            Player p = player.GetComponent<Player>();
            p.TakeDamage(this);
            p.attached = true;
            // StartCoroutine(Attacked());
        }
    }
}
