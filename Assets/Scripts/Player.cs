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
    [SerializeField] protected float jumpForce = 5f;
    // [SerializeField] protected float fallMultiplier = 2.5f;
    // [SerializeField] protected float _lowJumpMultiplier = 2f;
    [SerializeField] protected float slideSpeed = -1;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _col = GetComponent<Collision>();

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

        if (canLeft || canRight)
        {
            rb.velocity = new Vector2(inputVector.x * speed, rb.velocity.y);

        }
        if (inputVector != Vector2.zero && _col.onGround)
        {
            anim.PlayInFixedTime("TK_Walk_Anim");
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.eulerAngles = mousePos.x < transform.position.x ? new Vector2(0, 180) : Vector2.zero;

        if(_col.onWall && !_col.onGround) {
            if(_playerInputActions.Player.WallGrab.IsPressed()) {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            } else {
                if (_col.onLeftWall) {
                rb.velocity = inputVector.x == 1 ? new Vector2(0, slideSpeed) : new Vector2(inputVector.x * speed, slideSpeed);
                } else if (_col.onRightWall) {
                rb.velocity = inputVector.x == -1 ? new Vector2(0, slideSpeed) : new Vector2(inputVector.x * speed, slideSpeed);
                }
                rb.gravityScale = 3;
            }
        }
        
        if(!_playerInputActions.Player.Jump.IsPressed() && rb.velocity.y > 0) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_col.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }


}