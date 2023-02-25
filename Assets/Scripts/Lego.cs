using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : Enemy
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _bulletPosition;
    [SerializeField] private float _bulletForce = 5f;
    private bool _awake = false;
    private bool _canFire = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if(GetDistance() < distance) {
            if(!_awake) {
                anim.PlayInFixedTime("LegoEnemy1_Wake");
                StartCoroutine(WakeUp());
            } else if(_canFire) {
                Attack();
            }
            direction = transform.position.x < player.transform.position.x ? 1: -1;
            spriteRenderer.flipX = transform.position.x < player.transform.position.x ? true : false;
        }
    }
    public override void Attack() {
        Player p = player.GetComponent<Player>();
        StartCoroutine(Fire());
    }

    IEnumerator WakeUp() {
        anim.PlayInFixedTime("LegoEnemy1_Wake");
        _awake = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        _canFire = true;
    }

    IEnumerator Fire() {
        GameObject bullet_obj = Instantiate(_bullet, _bulletPosition.transform);
        bullet_obj.GetComponent<Animator>().Play("LegoEnemyBullet_Fire");
        Rigidbody2D bullet_rb = bullet_obj.GetComponent<Rigidbody2D>();
        bullet_rb.AddForce(new Vector2(_bulletForce * direction , 0), ForceMode2D.Impulse);
        _canFire = false;
        yield return new WaitForSeconds(3);
        _canFire = true;

        yield return new WaitForSeconds(2);
        Destroy(bullet_obj);
    }
}
