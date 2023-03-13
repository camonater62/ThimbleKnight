using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryThimble : Enemy
{
    private bool _attached = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetDistance() < distance && !attacked && !stunned)
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

    public override void Attack()
    {
        if (!_attached)
        {
            _attached = true;
            moving = false;
            attacked = true;
            anim.SetBool("Moving", false);
            // Attached();
        }
    }

    IEnumerator Attached()
    {
        player.GetComponent<Player>().TakeDamage(this, false);
        yield return new WaitForSeconds(.75f);
        if (_attached)
        {
            Attached();
        }
    }
}
