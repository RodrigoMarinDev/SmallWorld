using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject collectorPrefab;
    public GameObject shipPrefab;

    [Header("Station")]
    public int quantityResource;
    public int maxResources;
    public int quantityShips;
    public int maxShips;
    public float rotateSpeed;

    [Header("UI")]
    public Text resourceText;
    public Text shipText;
    public Text scoreText;
    public GameObject upgradeButton;
    public Text costUpdateText;
    public int score;
    public GameObject gameOver;

    [Header("Audio")]
    public AudioClip clickAudio;

    [Header("HealthPoints")]
    public int hp = 1;

    [Header("Upgrades")]
    public int collectorResources = 100;
    public int shipResources = 200;
    public int hpLvl1 = 1;
    public int hpLvl2 = 1;
    public int hpLvl3 = 1;
    public int resorucesLvl1 = 1;
    public int resorucesLvl2 = 1;
    public int resorucesLvl3 = 1;
    public int shipsLvl1 = 1;
    public int shipsLvl2 = 1;
    public int shipsLvl3 = 1;
    public Sprite spriteLvl2;
    public Sprite spriteLvl3;
    int actualLvl = 1;
    SpriteRenderer sr;

    void Awake()
    {
        hp = hpLvl1;
        maxResources = resorucesLvl1;
        maxShips = shipsLvl1;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        resourceText.text = quantityResource.ToString() + "/" + maxResources.ToString();
        shipText.text = quantityShips.ToString() + "/" + maxShips.ToString();
        scoreText.text = score.ToString();

        if (hp <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotateSpeed * Time.time);
    }

    public void CreateCollector()
    {
        AudioManager.instance.PlaySound(clickAudio, gameObject);
        if (quantityResource >= collectorResources && quantityShips < maxShips)
        {
            quantityResource -= collectorResources;
            Instantiate(collectorPrefab, transform.position, Quaternion.identity);
            score += collectorResources;
            quantityShips++;
        }
    }

    public void CreateShip()
    {
        AudioManager.instance.PlaySound(clickAudio, gameObject);
        if (quantityResource >= shipResources && quantityShips < maxShips)
        {
            quantityResource -= shipResources;
            Instantiate(shipPrefab, transform.position, Quaternion.identity);
            score += shipResources * 2;
            quantityShips++;
        }
    }

    public void UpgradeStation()
    {
        AudioManager.instance.PlaySound(clickAudio, gameObject);
        if (quantityResource >= resorucesLvl1 && actualLvl == 1)
        {
            quantityResource -= resorucesLvl1;
            sr.sprite = spriteLvl2;
            actualLvl = 2;
            hp = hpLvl2;
            maxResources = resorucesLvl2;
            costUpdateText.text = resorucesLvl2.ToString();
            upgradeButton.GetComponent<Image>().sprite = spriteLvl3;
            score += resorucesLvl1 * 2;
            maxShips = shipsLvl2;
        }

        if (quantityResource >= resorucesLvl2 && actualLvl == 2)
        {
            quantityResource -= resorucesLvl2;
            sr.sprite = spriteLvl3;
            actualLvl = 3;
            hp = hpLvl3;
            maxResources = resorucesLvl3;
            //costUpdateText.text = resorucesLvl3.ToString();
            upgradeButton.SetActive(false);
            score += resorucesLvl2 * 5;
            maxShips = shipsLvl3;
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }else
        {
            Time.timeScale = 0;
        }
    }

    public void getHitted(int damage)
    {
        hp -= damage;
    }
}
