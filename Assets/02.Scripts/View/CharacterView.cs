using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharacterView : MonoBehaviour
{
    public enum HandType { LEFT, RIGHT }

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer weapon_r;
    [SerializeField] private SpriteRenderer weapon_l;

    public Animator Animator => animator;
    public bool isCollision = false;
    public GameObject target;
    public Subject<GameObject> AttackSubject = new();

    public void SetAttackSpeed(float speed)
	{
        Animator.SetFloat("AttackSpeed", speed);
	}

    public void SetRunSpeed(float speed)
	{
        Animator.SetFloat("RunSpeed", speed);
	}

    public void SetWeapon(Sprite sprite, HandType handType = HandType.RIGHT)
    {
        weapon_l.transform.localPosition = new Vector3(0, sprite.rect.size.y / 100, 0);
        weapon_r.transform.localPosition = new Vector3(0, sprite.rect.size.y / 100, 0);
        switch (handType)
        {
            case HandType.LEFT:
                weapon_l.sprite = sprite;
                break;
            case HandType.RIGHT:
                weapon_r.sprite = sprite;
                break;
        }
    }

    public void Move(float moveSpeed)
    {
        transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector2.right);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            isCollision = true;
            target = collision.gameObject;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            isCollision = true;
            target = collision.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            isCollision = false;
            target = null;
        }
    }

    public void OnHitAttackAnimation()
    {
        AttackSubject.OnNext(target);
    }
}