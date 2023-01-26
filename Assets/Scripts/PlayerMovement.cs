using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rigidBody;
    private PlayerInput playerInput;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        
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
        float speed = 5.0f;
        if (Mathf.Abs(rigidBody.velocity.x) < speed) {
            rigidBody.AddForce(new Vector2(inputVector.x, 0) * speed, ForceMode.Force);
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        rigidBody.AddForce(Vector2.up * 10, ForceMode.Impulse);
    }
}
