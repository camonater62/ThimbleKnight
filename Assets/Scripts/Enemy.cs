using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {
    [Header("Movement Variables")]
    [SerializeField] protected int direction = 0;
    [SerializeField] private float _distance = 10f;

    [Header("Target")]
    [SerializeField] protected GameObject player;

    protected SpriteRenderer _spriteRenderer;

    private bool _stunned = false;


    protected override void Awake() {
        hp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < _distance) {
            if (transform.position.x < player.transform.position.x) { direction = 1; } else if (transform.position.x > player.transform.position.x) { direction = -1; }
        }
        if(!_stunned) { 
            rb.velocity = new Vector2(speed * direction, 0);
            if (rb.velocity.x < 0) {
                _spriteRenderer.flipX = false;
            } else {
                _spriteRenderer.flipX = true;
            }
        }


    }

    public void TakeDamage(Weapon weapon) {
        hp -= weapon.GetDmg();
        _stunned = true;
        if (hp <= 0) {
            this.gameObject.SetActive(false);
        } else {
            rb.AddForce(new Vector2(weapon.GetKnockback(), 0) * (-direction), ForceMode2D.Impulse);
            StartCoroutine(Stunned());
        }
    }

    IEnumerator Stunned() {
        yield return new WaitForSeconds(1);
        _stunned= false;
    }


    public override void Attack() {
        Player p = player.GetComponent<Player>();
        p.SetHP(p.GetHP() - damage);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Attack();
        }
    }
}