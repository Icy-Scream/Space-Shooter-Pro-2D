using
    System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _laser;
    [SerializeField] private float _cooldown;
    [SerializeField] private AudioSource _Audiomanager;
    [SerializeField] private AudioSource _Audiomanager2;
    [SerializeField] private AudioClip _bossEnter;
    [SerializeField] private AudioClip _bossEngine;
    [SerializeField] private AudioClip _bossRocket;
    [SerializeField] private AudioClip _bossLaser;
    private bool _fireReady = true;
    private bool _bossIsDead=false;

    void Update()
    {
        Health();
        Attack();
        EnterScene();
    }

    private void EnterScene()
    {
        if (transform.position.y > 2.5) 
        {
            _Audiomanager.clip = _bossEnter;
            _Audiomanager.Play();
            transform.Translate(Vector3.down * 4 * Time.deltaTime);
        }
    }

    private void Attack() 
    {
        if(transform.position.y <= 2.7) 
        { 
            if (_fireReady) 
            { 
                _fireReady = false;
                StartCoroutine(LaserParentChangeRoutine());
                StartCoroutine(RocketParentChangeRoutine());
                StartCoroutine(CoolDownSystem());
            }
        }
    }

    public bool BossDead() 
    {
        return _bossIsDead;
    }
        
        
    private void Health()
    {
        if (_health <= 0) 
        {
            Instantiate(_explosion, transform.position, Quaternion.identity,this.gameObject.transform);
            _bossIsDead = true;
            Destroy(gameObject,3);
        }
    }

    IEnumerator CoolDownSystem() 
    {
        yield return new WaitForSeconds(_cooldown);
        _fireReady = true;
    }

    IEnumerator RocketParentChangeRoutine()
    {
        yield return new WaitForSeconds(2f);
        _Audiomanager.clip = _bossRocket;
        _Audiomanager.Play();
        GameObject _rocket1 = Instantiate(_missile, transform.GetChild(0).transform.position, Quaternion.identity, this.transform);
        GameObject _rocket2 = Instantiate(_missile, transform.GetChild(1).transform.position, Quaternion.identity, this.transform);
        if (_rocket1 == null || _rocket2 == null)
        {
            Debug.Log("Rocket Missing");
        }
        else
        {
            yield return new WaitForSeconds(0.0001f); ;
            _rocket1.transform.parent = transform.parent;
            _rocket2.transform.parent = transform.parent;
        }
    }
    IEnumerator LaserParentChangeRoutine()
    {
        yield return new WaitForSeconds(2f);
        GameObject laser = Instantiate(_laser, transform.position, Quaternion.identity, this.transform);
        _Audiomanager2.clip = _bossLaser;
        _Audiomanager2.Play();
        if (laser == null)
        {
            Debug.Log("Laser Missing");
        }
        else
        {
            yield return new WaitForSeconds(0.0001f); ;
            laser.transform.parent = transform.parent;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser" || collision.gameObject.tag == "Rocket")
        {
            _health -= 20;
            Destroy(collision.gameObject);
        }
    }
}
