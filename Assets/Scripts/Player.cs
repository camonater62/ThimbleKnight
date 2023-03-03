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
    private bool inAir = false;
    private bool walking = false;
    [SerializeField] private float _knockback;
    [SerializeField] protected float jumpForce = 5f;
    [SerializeField] protected float acceleration = 10f;
    [SerializeField] protected float drag = 0.01f;
    [SerializeField] protected float maxRunSpeed = 10;
    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
        anim.PlayInFixedTime("Slash");
        weapon.GetComponent<Weapon>().Attack();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        bool canLeft = rb.velocity.x > -maxSpeed || inputVector.x > 0;
        bool canRight = rb.velocity.x < maxSpeed || inputVector.x < 0;

        if ((canLeft || canRight) && !stunned)
        {
            rb.velocity = new Vector2(rb.velocity.x + (inputVector.x * acceleration * Time.deltaTime), rb.velocity.y);
            // Clamp velocity in x axis only
            Vector2 horizontalVelocity = new Vector2(rb.velocity.x, 0);
            horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, maxRunSpeed);  // limits x-axis speed
            rb.velocity = new Vector2(horizontalVelocity.x, rb.velocity.y);               // now y can still fall as fast as possible
            Debug.Log("celleratin'");

        }
        if (inputVector.x == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * drag * Time.deltaTime, rb.velocity.y);
            Debug.Log("SCREDEEEEEEE!!");
        }
        if (_col.onGround)
        {
            anim.SetBool("inAir", false);
        }
        else
        {
            anim.SetBool("inAir", true);
        }
        if (inputVector != Vector2.zero && _col.onGround && !stunned && rb.velocity.x != 0)
        {
            anim.SetBool("walking", true);
        }
        if (rb.velocity.x == 0)
        {
            anim.SetBool("walking", false);
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
        if (Mathf.Abs(rb.velocity.x) > 0 && !stunned)
        {
            transform.eulerAngles = rb.velocity.x > 0 ? Vector2.zero : new Vector2(0, 180);
            direction = rb.velocity.x > 0 ? 1 : -1;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_col.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);        // add jump to current jump momentum
            anim.PlayInFixedTime("Jump");
            anim.SetBool("inAir", true);
        }
    }

    public void TakeDamage(Enemy enemy)
    {
        hp -= enemy.GetDamage();
        stunned = true;
        _immune = true;
        anim.PlayInFixedTime("Hit");
        if (hp <= 0)
        {
            Debug.Log("You lose");
            gameObject.SetActive(false);
        }
        else
        {
            rb.AddForce(new Vector2(direction * _knockback, 0), ForceMode2D.Impulse);
            StartCoroutine(Stunned());
        }
    }

    IEnumerator Stunned()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        stunned = false;
        _immune = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_immune)
        {
            if (other.tag == "MeleeEnemy")
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                TakeDamage(enemy);
            }
            else if (other.tag == "Projectile")
            {
                Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
                TakeDamage(enemy);
                Destroy(other.gameObject);
            }
        }
    }

}