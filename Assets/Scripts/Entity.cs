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

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    public virtual float GetMaxSpd() { return maxSpeed; }
    public virtual float GetSpeed() { return speed; }
    public virtual float GetHP() { return hp; }
    public virtual float GetDamage() { return damage; }
    public virtual void SetHP(float newHP){ hp = newHP; }

    protected virtual void Awake() {}
    public virtual void Move() {}
    public virtual void TakeDamage() {}
    public virtual void Attack() {}
}