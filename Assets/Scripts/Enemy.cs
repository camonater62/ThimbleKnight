using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {
    [SerializeField] protected int direction;
    public enum State {
        Patrol,
        Chase,
        Fight
    }
    [SerializeField] protected State state = State.Patrol;
    [SerializeField] protected GameObject player;

    protected SpriteRenderer _spriteRenderer;

    public void Update() {
        Move();
        ChangeState();
    }

    protected override void Awake() {
        hp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Move() {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed) {
            rb.velocity += new Vector2(direction * speed * Time.deltaTime, 0);
        }
        if (direction == 1) {
            _spriteRenderer.flipX = true;    
        } else if(direction == -1) {
            _spriteRenderer.flipX = false;
        }
    }

    public void TakeDamage(Weapon weapon) {
        hp -= weapon.GetDmg();
        if (hp <= 0) {
            this.gameObject.SetActive(false);
        } else {
            rb.AddForce(new Vector3(weapon.GetKnockback(), 0, 0) * (-direction), ForceMode2D.Impulse);
        }
    }

    public override void Attack() {
        Player p = player.GetComponent<Player>();
        p.SetHP(p.GetHP() - damage);
    }

    public void ChangeState() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < 2) {
            state = State.Fight;
        } else if (dist < 6) {
            state = State.Chase;
            if(player.transform.position.x < transform.position.x) {
                direction = -1;
            } else if(player.transform.position.x > transform.position.x) {
                direction = 1;
            }
        } else {
            state = State.Patrol;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall" || other.tag == "Barricade") {
            direction *= -1;
            rb.velocity = Vector2.zero;
        }
        if(other.tag == "Player") {
            Attack();
        }
    }
}