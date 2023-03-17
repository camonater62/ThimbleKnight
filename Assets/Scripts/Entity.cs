using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    [Header("Stats:")]
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float speed;
    [SerializeField] protected float maxHP;
    [SerializeField] protected float hp;
    [SerializeField] protected float damage;
   [SerializeField] protected int direction = 0;

    protected bool stunned = false;
    protected Rigidbody2D rb = null;
    protected Animator anim = null;
    protected SpriteRenderer spriteRenderer = null;

    public virtual float GetMaxSpd() { return maxSpeed; }
    public virtual float GetSpeed() { return speed; }
    public virtual float GetHP() { return hp; }
    public virtual float GetDamage() { return damage; }
    public virtual void SetHP(float newHP){ hp = newHP; }
    public virtual int GetDirection() {return direction;}

    protected virtual void Awake() {}
    public virtual void Move() {}
    public virtual void TakeDamage() {}
    public virtual void Attack() {}
}