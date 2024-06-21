using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectTitlePrefab;
    [SerializeField] float projectTitleSpeed = 10f;
    [SerializeField] float projectTitleLifeTime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateValiance = 0f;
    [SerializeField] float miniumFiringRate = 0.1f;
    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

   
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
           firingCoroutine =  StartCoroutine(FireCountinously());
        }
        else if(!isFiring &&  firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireCountinously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectTitlePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectTitleSpeed;
            }
            Destroy(instance,projectTitleLifeTime);
            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateValiance,
                baseFiringRate + firingRateValiance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, miniumFiringRate, float.MaxValue);
            
            audioPlayer.PlayShootingClip();
            // audioPlayer.GetInstance().PlayShootingClip();
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
