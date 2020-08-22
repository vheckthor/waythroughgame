using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class rocket : MonoBehaviour
{
    [SerializeField] float rotatroy = 100f;
    [SerializeField] float thrustUp = 20f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem mainEngineFire;
    [SerializeField] ParticleSystem death;
    [SerializeField] ParticleSystem success;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dead, Transcending}

    State state = State.Alive;
    bool collisionStateDisabled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();  
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionStateDisabled = !collisionStateDisabled;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ThrustingUp();
        }
        else
        {
            audioSource.Stop();
            mainEngineFire.Stop();
        }

    }

    private void ThrustingUp()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustUp);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineFire.Play();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || collisionStateDisabled)
        {
            return; 
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do this and this
                print("Good Bad");
                break;
            case "Finish":
                //do something
                state = State.Transcending;
                audioSource.PlayOneShot(successSound);
                success.Play();
                Invoke("LoadNextScene", 1f);

                break;
            default:
                //die
                state = State.Dead;
                audioSource.PlayOneShot(deathSound);
                death.Play();
                Invoke("LoadDefaultLevel", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        var next = SceneManager.GetActiveScene().buildIndex +1;
        if(next == SceneManager.sceneCountInBuildSettings)
        {
            next = 0;
        }
        SceneManager.LoadScene(next);
    }

    private void LoadDefaultLevel()
    {
        var current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);

    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        var rot = rotatroy * Time.deltaTime;
       
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward *rot);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rot);
        }

        rigidBody.freezeRotation = false;
    }
}
