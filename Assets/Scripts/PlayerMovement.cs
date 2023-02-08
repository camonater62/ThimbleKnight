using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float maxSpeed = 50.0f;
    [SerializeField] private float speed = 10.0f;
    private bool _isGrounded = true;

    private Rigidbody2D rigidBody;

    private PlayerInputActions playerInputActions;

    private Animator anim;


    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        bool canLeft = rigidBody.velocity.x > -maxSpeed || inputVector.x > 0;
        bool canRight = rigidBody.velocity.x < maxSpeed || inputVector.x < 0;

        if (canLeft || canRight) {
            rigidBody.velocity += new Vector2(inputVector.x, 0) * speed * Time.deltaTime;
            
        }
        if (inputVector != Vector2.zero && _isGrounded) {
            anim.PlayInFixedTime("TK_Walk_Anim");
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (_isGrounded) {
            rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
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
