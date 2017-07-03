using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Objetives")]
    public float checkRadiusIncrease = 0;
    public float checkRadius = 0;
    public LayerMask whatIsPlayer;
    public Transform target;
    public Station station;

    [Header("HealthPoints")]
    public int hp = 2;
    public int scoreDie = 10;

    [Header("Shoot")]
    public float shootDistance;
    public float shootDelay;
    float lastShoot;
    public GameObject shootPrefab;

    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float speed;

    void Awake()
    {
        station = GameObject.Find("Station").GetComponent<Station>();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        if (target == null)
        {
            FindPlayer();
        }

        if (DistanteToPlayer() < shootDistance && Time.time > lastShoot && target != null)
        {
            lastShoot = Time.time + shootDelay;
            Transform shoot = Instantiate(shootPrefab, transform.position, Quaternion.identity).transform;
            shoot.GetComponent<Shoot>().SetTarget(target);

            RotateToTarget(shoot, target);
        }

        if (hp <= 0)
        {
            station.score += scoreDie;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Move();
        }
    }

    void FindPlayer()
    {
        checkRadius = 0;
        do
        {
            if (Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer) != null)
            {
                target = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer).transform;
            }
            checkRadius += checkRadiusIncrease;
        } while (target == null && checkRadius < 15);
    }

    void Move()
    {
        RotateToTarget(transform, target);

        if (DistanteToPlayer() >= 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        }
    }

    void RotateToTarget(Transform origin, Transform target)
    {
        if (target != null) {
            Vector3 diff = target.position - origin.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            origin.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    float DistanteToPlayer()
    {
        float distance = 0;

        if (target != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, 10, whatIsPlayer);

            if (hit)
            {
                distance = hit.distance;
            }
        }

        return distance;
    }

    public void getHitted(int damage)
    {
        hp -= damage;
    }
}
