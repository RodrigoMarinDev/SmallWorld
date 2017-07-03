using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject audioSourcePrefab;
    public GameObject button;
    public Sprite muteOn;
    public Sprite muteOff;
    public AudioClip backgroundSound;
    float volume = 100;
    bool mute = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }

        Mute();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameObject.Find("Station") == null)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
        }
    }

    public void PlaySound(AudioClip clip, GameObject objectToPlayOn)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, objectToPlayOn.transform.position, volume);
        }
    }

    public void Mute()
    {
        if (mute)
        {
            volume = 0;
            button.GetComponent<Image>().sprite = muteOff;
        }
        else
        {
            volume = 100;
            button.GetComponent<Image>().sprite = muteOn;
        }

        mute = !mute;
    }
}
