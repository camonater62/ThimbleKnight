using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _bulletForce;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(int direction) {
        _rb.AddForce(new Vector2(direction * _bulletForce, 0), ForceMode2D.Impulse);
    }

    public void OnColissionEnter2D(Collision2D col) {
        Debug.Log("Collision");
    }
}
