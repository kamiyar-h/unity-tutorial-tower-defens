using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TowerScript : MonoBehaviour
{

    private SphereCollider _coll;
    private Transform _target;
    private Dictionary<string, Transform> _targetsHolder = new Dictionary<string, Transform>();

    public Transform weaponBody;
    public float range = 20f;
    public float attackDelay = 2f;
    private float _attackDelayHold = 0;

    public Transform weapon;
    public GameObject bullet;
    public float speed = 5;

    public int damage = 2;
    
    // Start is called before the first frame update
    void Awake()
    {
        _coll = transform.GetComponent<SphereCollider>();
        _coll.radius = range;
        _coll.isTrigger = true;
        _targetsHolder = new Dictionary<string, Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Enemy")) return;
        
        _targetsHolder.Add(other.GetComponent<EnemyScript>().GetUniqueID(), other.transform);

        if (_target == null)
        {
            TargetInit(other.transform);
        }
    }

    private void TargetInit(Transform target)
    {
        _target = target;
        
        LookAtEnemy();
        
        _target.GetComponent<EnemyScript>().AddTowerAttacker(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Enemy")) return;

        RefreshTarget(other.GetComponent<EnemyScript>().GetUniqueID());
    }

    private void RefreshTarget(string hashCode)
    {

        _targetsHolder.Remove(hashCode);
        
        if (_targetsHolder.Count <= 0)
        {
            _target = null;
        }
        else
        {
            TargetInit(_targetsHolder.First().Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_target == null) return;
        
        // Attack to Enemy
        AttackToEnemy();

        //LookAtEnemy();

    }

    void AttackToEnemy()
    {
        _attackDelayHold += Time.deltaTime;
        if (_attackDelayHold >= attackDelay)
        {
            print("Attack to :" + _target.tag);
            
            GameObject _bullet = Instantiate(bullet, weapon.position, Quaternion.identity);
            BulletScript _bullScript = _bullet.GetComponent<BulletScript>();
            _bullScript.Init(_target, speed, damage);
            
            
            _attackDelayHold = 0;
        }
    }

    public void EnemyDead(string hashCode)
    {
        RefreshTarget(hashCode);
    }

    void LookAtEnemy()
    {
        weaponBody.DOLookAt(_target.position, 1);
    }
}
