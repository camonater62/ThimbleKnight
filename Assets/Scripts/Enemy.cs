using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header ("Stats:")]
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float speed;
    [SerializeField] protected float hp;
    [SerializeField] protected float damage;

    //-1 is for left and 1 is for right
    [SerializeField] protected int direction;
    public enum State{
        Patrol,
        Chase,
        Fight
    }
    [SerializeField] protected State state = State.Patrol;
    [SerializeField] protected GameObject player;
    public virtual float GetMaxSpd() { return maxSpeed; }
    public virtual float GetSpeed() { return speed; }
    public virtual float GetHP() { return hp; }
    public virtual float GetDamage() { return damage;}
    
    /// <summary>
    /// Moves the player around the map depending on their state
    /// </summary>
    public void Move()
    {
        if(state == State.Patrol)
        {
           transform.Translate(direction * speed * Time.deltaTime, 0, 0);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }

    }

    public void ChangeState()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist < 2)
        {
            state = State.Fight;
        } else if(dist < 5)
        {
            state = State.Chase;
        } else
        {
            state = State.Patrol;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Wall")
        {
            direction *= -1;
        }
    }

}
