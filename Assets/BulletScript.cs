using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public Transform target;

    private float speed;
    private int damage;

    private Tweener tweener;

    public void Init(Transform _target, float _speed, int _damage)
    {
        target = _target;
        speed = _speed;
        damage = _damage;
        
        if(target == null) return;
        
        transform.LookAt(target);

        tweener = transform.DOMove(target.position, speed).SetEase(Ease.Linear).SetSpeedBased();
       
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Enemy")) return;
        
        other.GetComponent<EnemyScript>().TakeDamage(damage);
        
        tweener.Kill();
        
        Destroy(gameObject);
        
    }
}
