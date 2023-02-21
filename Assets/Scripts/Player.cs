using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : Entity
{
    private PlayerInputActions _playerInputActions;
    private Animator _anim;
    private Collision _col;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
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

    void Start()
    {
        // Start is called before the first frame update
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
            _anim.PlayInFixedTime("TK_Walk_Anim");
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = Vector2.zero;
        }
        //handles the better jump
        // if (rb.velocity.y < 0)
        // {
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        // }
        // else if (rb.velocity.y > 0 && !_playerInputActions.Player.Jump.IsPressed())
        // {
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        // }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_col.onGround)
        {
            // rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * _jumpForce;
            _col.onGround = false;
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {

    // }


}