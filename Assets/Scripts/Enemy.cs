using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Movement Variables")]
    [SerializeField] protected int direction = 0;
    [SerializeField] private float _distance = 10f;

    [Header("Target")]
    [SerializeField] protected GameObject player;

    protected SpriteRenderer _spriteRenderer;
    private Animator _anim;

    private bool _stunned = false;


    protected override void Awake()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    public void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < _distance)
        {
            direction = transform.position.x < player.transform.position.x ? 1 : -1;
        }
        if (!_stunned)
        {
            if(Mathf.Abs(rb.velocity.x) > 0) {
                _anim.PlayInFixedTime("Ant_001_Walking_001");
            } else {
                _anim.PlayInFixedTime("Ant_001_Idle");
            }
            rb.AddForce(Vector2.right * direction * speed, ForceMode2D.Force);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), 0);
            _spriteRenderer.flipX = rb.velocity.x < 0 ? false : true;
        }


    }

    public void TakeDamage(Weapon weapon)
    {
        hp -= weapon.GetDmg();
        _stunned = true;
        if (hp <= 0)
        {
            this.gameObject.SetActive(false);
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
        _stunned = false;
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