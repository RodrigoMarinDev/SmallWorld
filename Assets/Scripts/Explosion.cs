using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float alphaDecrease;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sr.color -= new Color(0, 0, 0, alphaDecrease * Time.deltaTime);

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}