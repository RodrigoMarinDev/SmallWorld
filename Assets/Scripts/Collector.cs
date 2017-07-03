using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [Header("Objetives")]
    public float checkRadiusIncrease = 0;
    public float checkRadius = 0;
    public Transform station;
    private Station stationScript;
    public Transform resource;
    public Transform target;
    public LayerMask collisionMask;
    public float hitDistance;

    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float speed;
    float speedAux;
    Vector2 velocity;

    [Header("Resources")]
    public LayerMask whatIsResource;
    public int extractResource;
    public int quantityCanTransport;
    public float waitResource;
    public float rotateSpeed;

    [Header("HealthPoints")]
    public int hp = 1;

    void Awake()
    {
        station = GameObject.Find("Station").transform;
        stationScript = station.GetComponent<Station>();
        speed = Random.Range(minSpeed, maxSpeed);
        speedAux = speed;
    }

    void Update()
    {
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
        }
        else
        {
            FindResources();
        }
    }

    void FindResources()
    {
        checkRadius = 0;
        do
        {
            if (Physics2D.OverlapCircle(transform.position, checkRadius, whatIsResource) != null)
            {
                resource = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsResource).transform;
                if (target == null)
                {
                    target = resource;
                }
            }
            checkRadius += checkRadiusIncrease;
        } while (target == null && checkRadius < 9);

        if (target == null)
        {
            target = station;
        }
    }

    void Move()
    {
        if (resource != null && DistanteToResource(transform.position, resource.position) <= 0.01f)
        {
            if (extractResource == 0)
            {
                extractResource = resource.GetComponent<Rock>().getMined(quantityCanTransport);
            }

            target = station;
        }

        if (transform.position == station.position)
        {
            if (stationScript.quantityResource + extractResource <= stationScript.maxResources)
            {
                stationScript.quantityResource += extractResource;
                stationScript.score += extractResource;
            }
            else
            {
                stationScript.quantityResource = stationScript.maxResources;
            }
            extractResource = 0;
            target = null;
            FindResources();
        }

        if (target != null)
        {
            Vector3 diff = target.position - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        }
    }

    float DistanteToResource(Vector3 origin, Vector3 destiny)
    {
        float distance = 0;
        RaycastHit2D hit = Physics2D.Raycast(origin, destiny - origin, 10, collisionMask);

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

    IEnumerator MineResource()
    {
        transform.RotateAround(resource.position, new Vector3(0, 0, 1), rotateSpeed);

        yield return new WaitForSeconds(waitResource);

        target = station;
        resource = null;
        speed = speedAux;
    }
}
