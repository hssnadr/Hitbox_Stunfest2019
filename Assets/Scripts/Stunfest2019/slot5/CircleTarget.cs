using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTarget : MonoBehaviour
{
    public SpriteRenderer innerSprite;
    public SpriteRenderer outerSprite;

    public Animator animator;

    public Transform tfm;

    public delegate void OnDestroyEventDelegate();
    public static OnDestroyEventDelegate OnDestroyEvent;

    void OnEnable()
    {
        Color c = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        innerSprite.color = c;
        outerSprite.color = c;

        tfm.localScale = Vector3.one * (Random.Range(0.5f, 1.5f));
    }

    public void OnHit()
    {
        animator.SetTrigger("Hit");
    }

    public void DisableGoAE()
    {
        if (OnDestroyEvent != null)
            OnDestroyEvent.Invoke();

        Destroy(gameObject);
    }
}
