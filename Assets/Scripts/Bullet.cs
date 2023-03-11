using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private float _bulletForce;
  [SerializeField] private float _dmg;
  private Rigidbody2D _rb;
  private float _direction;

  public float GetDamage() { return _dmg; }
  public float GetDirection() { return _direction;}

  public void Fire(int direction)
  {
    _direction = direction;
    _rb = GetComponent<Rigidbody2D>();
    _rb.AddForce(new Vector2(direction * _bulletForce, 0), ForceMode2D.Impulse);
  }

  private void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.tag != "RangedEnemy")
    {
      Destroy(gameObject);
    }
  }
}
