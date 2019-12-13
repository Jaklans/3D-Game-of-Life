using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChange : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] music;

    // Start is called before the first frame update
    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            source.Stop();
            source.clip = music[0];
            source.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            source.Stop();
            source.clip = music[1];
            source.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            source.Stop();
            source.clip = music[2];
            source.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            source.Stop();
            source.clip = music[3];
            source.Play();
        }
    }
}
