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

        if (canLeft || canRight) {
            rb.velocity = new Vector2(inputVector.x * speed, rb.velocity.y);

        }
        if (inputVector != Vector2.zero && _col.onGround) {
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
            spriteRenderer.flipX = rb.velocity.x > 0 ? false : true;
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
        if(hp <= 0) {
            Debug.Log("You lose");
            gameObject.SetActive(false);
        }
    }


}