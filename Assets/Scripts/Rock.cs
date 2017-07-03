using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    public int minResource;
    public int maxResource;
    public int quantityResource;

    public float minRotateSpeed;
    public float maxRotateSpeed;
    float rotateSpeed;

    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float speed;

    Vector3 target;
    SpriteRenderer sr;

    void Awake()
    {
        quantityResource = Random.Range(minResource, maxResource);
        speed = Random.Range(minSpeed, maxSpeed);
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);

        float x, y;

        if (transform.position.x < 0)
        {
            x = transform.position.x + 10f;
        }
        else
        {
            x = transform.position.x - 10f;
        }

        if (transform.position.y < 0)
        {
            y = transform.position.y + 10f;
        }
        else
        {
            y = transform.position.y - 10f;
        }

        target = new Vector2(x, y);
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (quantityResource <= 0)
        {
            Destroy(gameObject);
        }

        if (!sr.isVisible)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, rotateSpeed * Time.time);
    }

    public int getMined(int quantityMined)
    {
        int quantityReturn;

        if (quantityMined <= quantityResource)
        {
            quantityResource -= quantityMined;
            quantityReturn = quantityMined;
        }else
        {
            quantityReturn = quantityResource;
            quantityResource = 0;
        }

        return quantityReturn;
    }


}
