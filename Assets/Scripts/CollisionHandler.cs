using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip successAudio;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    AudioSource audioSound;
    Movement movementScript;

    bool isTransitioning = false;
    bool collisonDisabled = false;

    private void Start()
    {
        audioSound = GetComponent<AudioSource>();
        movementScript = GetComponent<Movement>();
    }
    private void Update()
    {
        RespondToDebugKeys();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisonDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("nic nie rob");
                break;
            case "Fuel":
                Debug.Log("Paliwo");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSound.Stop();
        successParticle.Play();
        movementScript.enabled = false;
        audioSound.PlayOneShot(successAudio);
        Invoke("NextScene", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSound.Stop();
        deathParticle.Play();
        movementScript.enabled = false;
        audioSound.PlayOneShot(deathAudio);
        Invoke("ReloadLevel", levelLoadDelay);        
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
    void NextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisonDisabled = !collisonDisabled;
        }
    }
}
