using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    public enum HandType { LEFT, RIGHT }

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer weapon_r;
    [SerializeField] private SpriteRenderer weapon_l;

    public Animator Animator => animator;
    public bool isCollision = false;

    public void SetWeapon(Sprite sprite, HandType handType = HandType.RIGHT)
    {
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
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            isCollision = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            isCollision = false;
        }
    }
}