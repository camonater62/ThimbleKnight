using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : Entity
{
    private PlayerInputActions _playerInputActions;
    // private Animator _anim;
    private Collision _col;
    private SpringJoint2D _SpringJoint;
    private bool _immune = false;
    [SerializeField] private float _knockback;
    [SerializeField] protected float jumpForce = 5f;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _col = GetComponent<Collision>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _SpringJoint = GetComponent<SpringJoint2D>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += Jump;
        _playerInputActions.Player.Attack.performed += Attack;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        weapon.GetComponent<Weapon>().Attack();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        bool canLeft = rb.velocity.x > -maxSpeed || inputVector.x > 0;
        bool canRight = rb.velocity.x < maxSpeed || inputVector.x < 0;

        if ((canLeft || canRight) && !stunned) {
            rb.velocity = new Vector2(inputVector.x * speed, rb.velocity.y);

        }
        if (inputVector != Vector2.zero && _col.onGround && !stunned) {
            anim.PlayInFixedTime("TK_Walk_Anim");
        }

        // _SpringJoint.enabled = false;
        // if (Input.GetKey(KeyCode.Mouse1)) {
        //     if(transform.position.y < _SpringJoint.connectedAnchor.y
        //     && _SpringJoint.distance > 1) {
        //         _SpringJoint.enabled = true;
        //     }
        // } else {
        //     _SpringJoint.connectedAnchor = transform.position;
        // }
        

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Mathf.Abs(rb.velocity.x) > 0) {
            if(!stunned) {
                transform.eulerAngles = rb.velocity.x > 0 ? Vector2.zero : new Vector2(0, 180);
            }
            direction = rb.velocity.x > 0 ? 1 : -1;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_col.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void TakeDamage(Enemy enemy)
    {
        hp -= enemy.GetDamage();
        stunned = true;
        _immune = true;
        if(hp <= 0) {
            Debug.Log("You lose");
            gameObject.SetActive(false);
        } else {
            rb.AddForce(new Vector2(direction * _knockback, 0), ForceMode2D.Impulse);
            StartCoroutine(Stunned());
        }
    }

    IEnumerator Stunned() {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        stunned = false;
        _immune = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!_immune) {
            if(other.tag == "MeleeEnemy") {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                TakeDamage(enemy);
            } else if (other.tag == "Projectile") {
                Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
                TakeDamage(enemy);
                Destroy(other.gameObject);
            }
        }
    }

}