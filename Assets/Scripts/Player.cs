using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity {
    private bool _isGrounded = true;
    private PlayerInputActions playerInputActions;
    private Animator anim;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    protected override void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Attack.performed += Attack;
    }

    public void Attack(InputAction.CallbackContext context) {
        weapon.GetComponent<Weapon>().Attack();
    }

    void Start() {
        // Start is called before the first frame update
        maxSpeed = 50.0f;
        speed = 10.0f;
    }

    // Update is called once per frame
    void Update() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        bool canLeft = rb.velocity.x > -maxSpeed || inputVector.x > 0;
        bool canRight = rb.velocity.x < maxSpeed || inputVector.x < 0;

        if (canLeft || canRight) {
            rb.velocity += new Vector2(inputVector.x, 0) * speed * Time.deltaTime;
            
        }
        if (inputVector != Vector2.zero && _isGrounded) {
            anim.PlayInFixedTime("TK_Walk_Anim");
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < transform.position.x) {
            transform.eulerAngles = new Vector2(0, 180);
        } else {
            transform.eulerAngles = Vector2.zero;
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (_isGrounded) {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Floor") {
            _isGrounded = true;
        } 
        if(collision.gameObject.tag == "Barricade") {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider) ;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.tag == "Floor") {
            _isGrounded = false;
        }
    }


}