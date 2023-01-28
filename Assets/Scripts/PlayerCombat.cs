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
    [SerializeField] protected float damage;

    [Header("Items:")]
    [SerializeField] protected GameObject weapon;
    //Possible stat depending on field
    //[SerializeField] protected float attackSpd;
    // Start is called before the first frame update
    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        anim = weapon.GetComponent<Animator>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += Attack;
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Attack(InputAction.CallbackContext context) {
        anim.CrossFadeInFixedTime("attack", 0.1f);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("attack");
        if(other.tag == "enemy") {
            Enemy script = other.gameObject.GetComponent<Enemy>();
            script.SetHP(script.GetHP() - damage);
        }
    }
}
