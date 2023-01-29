using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour {
    private Rigidbody rigidBody;
    private PlayerInput playerInput;
    
    private PlayerInputActions playerInputActions;
    private Animator anim;
    

    [Header("Stats:")]
    [SerializeField] protected float hp;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;

    //Possible stat depending on field
    //[SerializeField] protected float attackSpd;
    // Start is called before the first frame update
    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += Attack;

    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < transform.position.x) {
            transform.eulerAngles = new Vector3(0, 180, 0);
        } else {
            transform.eulerAngles = Vector3.zero;
        }
    }

    public void Attack(InputAction.CallbackContext context) {
        weapon.GetComponent<Weapon>().Attack();
    }
}
