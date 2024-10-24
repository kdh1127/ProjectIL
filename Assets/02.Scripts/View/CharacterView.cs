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

 
}