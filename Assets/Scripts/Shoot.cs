using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float shootSpeed;
    public int damage = 1;
    Transform target;
    public LayerMask whatIsEnemy;
    SpriteRenderer sr;
    public Transform explosionPrefab;

    [Header("Audio")]
    public AudioClip shootAudio;
    public AudioClip hitAudio;

    void Awake()
    {
        transform.SetParent(GameObject.Find("TempInstances").transform);
        AudioManager.instance.PlaySound(shootAudio, gameObject);
        sr = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 4);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * shootSpeed * Time.smoothDeltaTime);

        if (target != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, 10, whatIsEnemy);

            if (hit.distance < 0.001f)
            {
                if (target.GetComponent<Enemy>() != null) {
                    target.GetComponent<Enemy>().getHitted(damage);
                }

                if (target.GetComponent<Ship>() != null)
                {
                    target.GetComponent<Ship>().getHitted(damage);
                }

                if (target.GetComponent<Collector>() != null)
                {
                    target.GetComponent<Collector>().getHitted(damage);
                }

                if (target.GetComponent<Station>() != null)
                {
                    target.GetComponent<Station>().getHitted(damage);
                }

                AudioManager.instance.PlaySound(hitAudio, gameObject);
                Instantiate(explosionPrefab, new Vector2(hit.point.x, hit.point.y), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
