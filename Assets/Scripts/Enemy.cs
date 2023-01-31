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

    private Rigidbody rigidBody;
    public virtual float GetMaxSpd() { return maxSpeed; }
    public virtual float GetSpeed() { return speed; }
    public virtual float GetHP() { return hp; }
    public virtual float GetDamage() { return damage; }
    public virtual void SetHP(float newHP){ hp = newHP; }
    private void Awake() {
        hp = maxHP;
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Update() {
        Move();
        ChangeState();
        Debug.Log(hp);
    }
    /// <summary>
    /// Moves the player around the map depending on their state
    /// </summary>
    public void Move() {
        if (state == State.Patrol) {
            transform.Translate(direction * speed * Time.deltaTime, 0, 0);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }

    }

    public void ChangeState() {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < 2) {
            state = State.Fight;
        } else if (dist < 5) {
            state = State.Chase;
        } else {
            state = State.Patrol;
        }
    }

    public void TakeDamage(Weapon weapon) {
        hp -= weapon.GetDmg();
        if (hp <= 0) {
            this.gameObject.SetActive(false);
        } else {
            rigidBody.AddForce(new Vector3(weapon.GetKnockback(), 0, 0) * (-direction), ForceMode.Impulse);
        }
    }

    private void Attack() {
        PlayerCombat p = player.GetComponent<PlayerCombat>();
        p.SetHP(p.GetHP() - damage);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "wall") {
            direction *= -1;
        }
        if(other.tag == "player") {
            Attack();
        }
    }

}
