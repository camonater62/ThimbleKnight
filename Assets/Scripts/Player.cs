using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class Player : Entity
{
  private PlayerInputActions _playerInputActions = null;
  // private Animator _anim;
  private Collision _col;
  private DistanceJoint2D _DistanceJoint;
  private AudioSource[] _audio;
  private GameObject _enemies;
  private bool _immune = false;
  private bool inAir = false;
  private bool walking = false;
  [SerializeField] private float _knockback;
  [SerializeField] protected float jumpForce = 5f;
  [SerializeField] protected float acceleration = 10f;
  [SerializeField] protected float drag = 0.01f;
  [SerializeField] protected float maxRunSpeed = 10;
  [SerializeField] protected List<GameObject> hearts;
  public bool attached = false;
  public bool attacking = false;
  [Header("Items:")]
  [SerializeField] protected GameObject weapon;
  [SerializeField] private float _attackDelay = 0.5f;
  [SerializeField] private AnimationClip attackAnim;

  protected override void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    Animator[] temp = GetComponentsInChildren<Animator>();
    foreach (Animator a in temp)
    {
      if (a.name == "PlayerSprite")
      {
        anim = a;
      }
    }
    _col = GetComponent<Collision>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    _DistanceJoint = GetComponent<DistanceJoint2D>();

    _playerInputActions = new PlayerInputActions();
    _playerInputActions.Player.Enable();
    _playerInputActions.Player.Jump.performed += Jump;
    _playerInputActions.Player.Attack.performed += Attack;
    _playerInputActions.Player.ToggleEnemies.performed += ToggleEnemies;
    _playerInputActions.Player.Exit.performed += Exit;

    _attackDelay = attackAnim.length * 0.66f;

    _audio = GetComponents<AudioSource>();

    _enemies = GameObject.Find("Enemies");
  }
  
  public void Exit(InputAction.CallbackContext context) {
    Application.Quit();
  }
  public void ToggleEnemies(InputAction.CallbackContext context) {
    if(_enemies != null) {
      _enemies.SetActive(!_enemies.activeInHierarchy);
    }
  }
  public void Attack(InputAction.CallbackContext context)
  {
    if (!stunned && !attacking)
    {
      attacking = true;
      //cancel any playing animations
      anim.SetBool("inAir", false);
      anim.SetBool("attacking", true);
      anim.SetBool("walking", false);
      weapon.GetComponent<Weapon>().Attack(_attackDelay);
      StartCoroutine(Attacking());
      _audio[1].Play();
    }
  }

  IEnumerator Attacking()
  {
    yield return new WaitForSeconds(_attackDelay);
    anim.SetBool("attacking", false);
    attacking = false;
  }

  // Update is called once per frame
  void Update() {
    Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

    bool canLeft = rb.velocity.x > -maxSpeed || inputVector.x > 0;
    bool canRight = rb.velocity.x < maxSpeed || inputVector.x < 0;

    if ((canLeft || canRight) && !stunned)
    {
      rb.velocity = new Vector2(rb.velocity.x + (inputVector.x * acceleration * Time.deltaTime), rb.velocity.y);
      // Clamp velocity in x axis only
      Vector2 horizontalVelocity = new Vector2(rb.velocity.x, 0);
      horizontalVelocity = attached ? Vector2.ClampMagnitude(horizontalVelocity, maxRunSpeed / 2) : Vector2.ClampMagnitude(horizontalVelocity, maxRunSpeed);  // limits x-axis speed
      rb.velocity = new Vector2(horizontalVelocity.x, rb.velocity.y);               // now y can still fall as fast as possible
    }
    if (inputVector.x == 0 && !stunned && _col.onGround)
    {
      rb.velocity = new Vector2(rb.velocity.x * drag * Time.deltaTime, rb.velocity.y);
    }

    if (_col.onGround && !attacking)
    {
      anim.SetBool("inAir", false);
    }
    else
    {
      anim.ResetTrigger("Jump");
      if(!attacking) {
        anim.SetBool("inAir", true);
      }
    }
    if (inputVector != Vector2.zero && _col.onGround && !stunned && rb.velocity.x != 0)
    {
      walking = true;
      anim.SetBool("walking", true);

    }
    if (inputVector == Vector2.zero)
    {
      walking = false;
      anim.SetBool("walking", false);
    }

    if (Mathf.Abs(rb.velocity.x) > 0 && !stunned && !attacking && inputVector != Vector2.zero)
    {
      transform.eulerAngles = rb.velocity.x > 0 ? Vector2.zero : new Vector2(0, 180);
      transform.GetChild(0).transform.eulerAngles = Vector2.zero;
      direction = rb.velocity.x > 0 ? 1 : -1;
    }
  }

  public void Jump(InputAction.CallbackContext context)
  {
    if (_col.onGround)
    {
      Debug.Log(rb);
      rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);        // add jump to current jump momentum
      anim.SetTrigger("Jump");
      // anim.SetBool("inAir", true);
    }
  }

  public void TakeDamage(Enemy enemy)
  {
    hp -= enemy.GetDamage();
    StartCoroutine(Stunned());
  }

  public void TakeDamage(Bullet bullet)
  {
    hp -= bullet.GetDamage();
    StartCoroutine(Stunned());
  }

  IEnumerator Stunned()
  {
    //edit the last heart in the list
    GameObject heart = hearts[hearts.Count - 1];
    heart.GetComponent<Animator>().SetTrigger("loseHealth");
    stunned = true;
    _immune = true;
    anim.SetBool("stunned", true);
    _audio[0].Play();
    rb.velocity = new Vector2(direction * _knockback, 0); //take knockback
    yield return new WaitForSeconds(0.5f);
    anim.SetBool("stunned", false);
    //heart represents 2 hp so delete the heart if health is % 2
    if (hp % 2 == 0)
    {
      hearts.Remove(heart);
      Destroy(heart);
    }
    if (hp <= 0)
    {
      SceneManager.LoadSceneAsync("LoseScene");  //end game scenario
    }
    heart.GetComponent<Animator>().ResetTrigger("loseHealth");
    rb.velocity = Vector2.zero;
    stunned = false;
    _immune = false;

  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!_immune)
    {
      if (other.tag == "Projectile")
      {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        TakeDamage(bullet);
      }
    }
    if(other.tag == "EndGame") {
      SceneManager.LoadSceneAsync("WinScene");  //end game scenario
    }
  }

  private void OnCollisionEnter2D(Collision2D col) {
    //makes sure that collision while curse thimble is attached doesn't move the player
    if (col.gameObject.tag == "MeleeEnemy") {
      Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
  }

}