using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("Objetives")]
    public float checkRadiusIncrease = 0;
    public float checkRadius = 0;
    public LayerMask whatIsEnemy;
    public Transform target;

    [Header("Station")]
    Transform station;
    public LayerMask whatIsStation;
    public float rotateDistance;
    public float rotateSpeed;
    float rotateSpeedAux;

    [Header("Ship")]
    public Transform nearShip;
    public LayerMask whatIsShip;

    [Header("Shoot")]
    public float shootDistance;
    public float shootDelay;
    float lastShoot;
    public GameObject shootPrefab;

    [Header("HealthPoints")]
    public int hp = 1;

    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float speed;

    void Awake()
    {
        station = GameObject.Find("Station").transform;
        speed = Random.Range(minSpeed, maxSpeed);
        rotateSpeedAux = rotateSpeed;
    }

    void Update()
    {
        if (target == null)
        {
            FindEnemy();
        }

        FindShip();

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Move();

            if (DistanteToEnemy() < shootDistance && Time.time > lastShoot)
            {
                lastShoot = Time.time + shootDelay;
                Transform shoot = Instantiate(shootPrefab, transform.position, Quaternion.identity).transform;
                shoot.GetComponent<Shoot>().SetTarget(target);

                RotateToTarget(shoot, target);
            }
        }
        else
        {
            MoveToStation();
        }
    }

    void FindEnemy()
    {
        checkRadius = 0;
        do
        {
            if (Physics2D.OverlapCircle(transform.position, checkRadius, whatIsEnemy) != null)
            {
                target = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsEnemy).transform;
            }
            checkRadius += checkRadiusIncrease;
        } while (target == null && checkRadius < 15);
    }

    void FindShip()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, whatIsShip) != null)
        {
            nearShip = Physics2D.OverlapCircle(transform.position, 0.3f, whatIsShip).transform;
            if (nearShip == gameObject.transform)
            {
                nearShip = null;
            }
        }else
        {
            nearShip = null;
        }
    }

    void Move()
    {
        RotateToTarget(transform, target);

        if (DistanteToEnemy() > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        }
    }

    void MoveToStation()
    {
        if (DistanteToStation() > rotateDistance)
        {
            RotateToTarget(transform, station);
            transform.position = Vector2.MoveTowards(transform.position, station.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            RotateToTarget(transform, station);
            if (DistanteToStation() < rotateDistance - 0.05f)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(station.position.x + rotateDistance, station.position.y + rotateDistance), -speed * Time.fixedDeltaTime);
            }
            else
            {
                if (nearShip != null)
                {
                    if (nearShip.GetComponent<Ship>().rotateSpeed != 0)
                        rotateSpeed = 0;
                }
                else
                {
                    rotateSpeed = rotateSpeedAux;
                }

                transform.RotateAround(station.position, new Vector3(0, 0, 1), rotateSpeed);
            }
        }
    }

    void RotateToTarget(Transform origin, Transform target)
    {
        Vector3 diff = target.position - origin.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        origin.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    float DistanteToEnemy()
    {
        float distance = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, 10, whatIsEnemy);

        if (hit)
        {
            distance = hit.distance;
        }

        return distance;
    }

    float DistanteToStation()
    {
        float distance = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, station.position - transform.position, 10, whatIsStation);

        if (hit)
        {
            distance = hit.distance;
        }

        return distance;
    }

    public void getHitted(int damage)
    {
        hp -= damage;
    }
}
