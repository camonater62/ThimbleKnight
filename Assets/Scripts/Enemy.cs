using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    [Header("Stats:")]
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float speed;
    [SerializeField] protected float maxHP;
    protected float hp;
    [SerializeField] protected float damage;

    //-1 is for left and 1 is for right
    [SerializeField] protected int direction;
    public enum State {
        Patrol,
        Chase,
        Fight
    }
    [SerializeField] protected State state = State.Patrol;
    [SerializeField] protected GameObject player;

    private Rigidbody2D rigidBody;
    private SpriteRenderer _spriteRenderer;
    public virtual float GetMaxSpd() { return maxSpeed; }
    public virtual float GetSpeed() { return speed; }
    public virtual float GetHP() { return hp; }
    public virtual float GetDamage() { return damage; }
    public virtual void SetHP(float newHP){ hp = newHP; }
    private void Awake() {
        hp = maxHP;
        rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        Move();
        ChangeState();
    }
    /// <summary>
    /// Moves the player around the map depending on their state
    /// </summary>
    public void Move() {
        if (rigidBody.velocity.x < maxSpeed) {
            rigidBody.velocity += new Vector2(direction * speed * Time.deltaTime, 0);
        }
        if (direction == 1) {
            _spriteRenderer.flipX = true;    
        } else if(direction == -1) {
            _spriteRenderer.flipX = false;
        }
    }

    public void ChangeState() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(player.transform.position);
        if (dist < 2) {
            state = State.Fight;
        } else if (dist < 4) {
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

    public void TakeDamage(Weapon weapon) {
        hp -= weapon.GetDmg();
        if (hp <= 0) {
            this.gameObject.SetActive(false);
        } else {
            rigidBody.AddForce(new Vector3(weapon.GetKnockback(), 0, 0) * (-direction), ForceMode2D.Impulse);
        }
    }

    private void Attack() {
        PlayerCombat p = player.GetComponent<PlayerCombat>();
        p.SetHP(p.GetHP() - damage);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            direction *= -1;
            rigidBody.velocity = Vector2.zero;
        }
        if(other.tag == "Player") {
            Attack();
        }
    }

}
