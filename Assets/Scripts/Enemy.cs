using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Movement Variables")]

    [SerializeField] protected float distance = 10f;

    [Header("Target")]
    [SerializeField] protected GameObject player;


    protected bool moving = false;
    protected bool attacked = false;

    public float GetDistance() { return Vector3.Distance(transform.position, player.transform.position); }



    protected override void Awake()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public override void Move()
    {

        direction = transform.position.x < player.transform.position.x ? 1 : -1;
        rb.AddForce(Vector2.right * direction * speed, ForceMode2D.Force);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), 0);
        transform.eulerAngles = direction == 1 ? new Vector2(0, 180) : Vector2.zero;
    }

    public virtual void TakeDamage(Weapon weapon)
    {
        hp -= weapon.GetDmg();
        moving = false;
        stunned = true;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Stunned(weapon));
        }
    }

    IEnumerator Stunned(Weapon weapon)
    {
        anim.SetBool("Damaged", true);
        yield return new WaitForSeconds(0.25f);
        rb.AddForce(new Vector2(weapon.GetKnockback(), 0) * player.GetComponent<Player>().GetDirection(), ForceMode2D.Impulse);
        anim.SetBool("Damaged", false);
        yield return new WaitForSeconds(1);
        moving = true;
        stunned = false;
        rb.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D>(), false);
    }

    protected IEnumerator Attacked()
    {
        moving = false;
        attacked = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f);
        attacked = false;
        // moving = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "RangedEnemy" || col.gameObject.tag == "MeleeEnemy" || col.gameObject.tag == "Projectile")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        if (col.gameObject.tag == "Player")
        {
            Attack();
        }
    }
}