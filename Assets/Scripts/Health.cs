using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField] bool isPlayer;
   [SerializeField] int health = 50;
   [SerializeField] int score = 50;
   [SerializeField] ParticleSystem hitEffect;
   [SerializeField] bool applyCameraShake;
   CameraShake cameraShake;

   AudioPlayer audioPlayer;
   ScoreKeeper scorekeeper;
   LevelManager levelManager;
   void Awake()
   {
      cameraShake = Camera.main.GetComponent<CameraShake>();
      audioPlayer = FindObjectOfType<AudioPlayer>();
      scorekeeper = FindObjectOfType<ScoreKeeper>();
      levelManager = FindObjectOfType<LevelManager>();
   }

   void OnTriggerEnter2D(Collider2D other)
   {
      Damage damageDealer = other.GetComponent<Damage>();
      if (damageDealer != null)
      {
         //take damge
         TakeDamge(damageDealer.GetDamage());
         PlayHitEffect();
         audioPlayer.PlayDamageClip();
         ShakeCamera();
         damageDealer.Hit();
      }
   }

   public int GetHealth()
   {
      return health;
   }

  void TakeDamge(int damge)
  {
     health -= damge;
     if (health <= 0)
     {
        Die();
     }
  }

  void Die()
  {
     if (!isPlayer)
     {
        scorekeeper.ModifyScore(score);
     }
     else
     {
        levelManager.LoadGameOver();
     }
     Destroy(gameObject);
  }

  void PlayHitEffect()
  {
     ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
     Destroy(instance.gameObject,instance.main.duration + instance.main.startLifetime.constantMax);
  }

  void ShakeCamera()
  {
     if (cameraShake != null && applyCameraShake)
     {
        cameraShake.Play();
     }
  }
}
