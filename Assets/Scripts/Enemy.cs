using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Movement Variables")]
    [SerializeField] protected int direction = 0;
    [SerializeField] protected float distance = 10f;

    [Header("Target")]
    [SerializeField] protected GameObject player;

    protected SpriteRenderer spriteRenderer;

    protected bool stunned = false;
    protected bool moving = false;
    protected bool awake = false;

    public float GetDistance() {return Vector3.Distance(transform.position, player.transform.position);}


    protected override void Awake()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public override void Move() {

        direction = transform.position.x < player.transform.position.x ? 1 : -1;
        rb.AddForce(Vector2.right * direction * speed, ForceMode2D.Force);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), 0);
        spriteRenderer.flipX = direction == 1 ? true : false;
    }

    public void WakeUp() {
        moving = true;
        direction = transform.position.x < player.transform.position.x ? 1 : -1;
        anim.SetBool("Moving", true);
        awake = true;
    }

    public void TakeDamage(Weapon weapon)
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
            rb.AddForce(new Vector2(weapon.GetKnockback(), 0) * (-direction), ForceMode2D.Impulse);
            StartCoroutine(Stunned());
        }
    }

    IEnumerator Stunned()
    {
        yield return new WaitForSeconds(1);
        moving = true;
        stunned = false;
    }


    public override void Attack()
    {
        Player p = player.GetComponent<Player>();
        p.SetHP(p.GetHP() - damage);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Attack();
        }
    }
}