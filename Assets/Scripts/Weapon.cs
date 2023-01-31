using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Stats:")]
    [SerializeField] protected float damage;
    [SerializeField] protected float knockback;

    private Animator anim;

    private bool attacking = false;

    public virtual float GetKnockback() { return knockback; }
    public virtual float GetDmg() { return damage; }
    private void Awake() {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack() {
        anim.CrossFadeInFixedTime("attack", 0.1f);
        attacking = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (attacking) {
                enemy.TakeDamage(this);
                attacking = false;
            }
        }
    }
}
