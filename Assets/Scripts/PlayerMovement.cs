using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float maxSpeed = 5.0f;

    [SerializeField]
    private float acceleration = 1000f;

    private Rigidbody2D rigidBody;
    private PlayerInput playerInput;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        float force = acceleration * Time.deltaTime;
        if (Mathf.Abs(rigidBody.velocity.x) < maxSpeed) {
            rigidBody.AddForce(new Vector2(inputVector.x, 0) * force, ForceMode2D.Force);
        }
        

    }

    public void Jump(InputAction.CallbackContext context) {
        rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
}
