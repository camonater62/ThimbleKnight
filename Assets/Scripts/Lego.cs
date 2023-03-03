using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : Enemy
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _bulletPosition;
    [SerializeField] private GameObject _muzzleFlash;
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
            transform.eulerAngles = transform.position.x < player.transform.position.x ? new Vector2(0, 180) : Vector2.zero;
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
        GameObject bullet = Instantiate(_bullet, _bulletPosition.transform.position, gameObject.transform.rotation);
        _muzzleFlash.GetComponent<Animator>().PlayInFixedTime("LegoEnemyBullet_Fire");
        bullet.GetComponent<Bullet>().Fire(direction);
        _canFire = false;
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(2);
        _canFire = true;

        yield return new WaitForSeconds(2);
        Destroy(bullet);
    }
}
