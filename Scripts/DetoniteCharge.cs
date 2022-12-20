using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetoniteCharge : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    private SpriteRenderer DetoniteChargeSprite { get; set; } = null;
    private SpriteRenderer HitRadiusSprite { get; set; } = null;
    private CircleCollider2D HitRadiusCollider  { get; set; } = null;
    private ParticleSystem ParticleSystem { get; set; } = null;
    private Animator Animator { get; set; } = null;
    
    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
            );
            Application.Quit(1);
        }
        
        if (this.gameObject.transform.Find("HitRadius") is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + "/HitRadius game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }

        if ((HitRadiusSprite = this.gameObject.transform.Find("HitRadius").GetComponent<SpriteRenderer>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + "/HitRadius game object is " +
                "missing SpriteRenderer component."
            );
            Application.Quit(1);
        }
        
        if ((DetoniteChargeSprite = this.gameObject.GetComponent<SpriteRenderer>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + " game object is " +
                "missing SpriteRenderer component."
            );
            Application.Quit(1);
        }
        
        if ((HitRadiusCollider = this.gameObject.GetComponent<CircleCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.name +
                " game object is missing CircleCollider2D component."
            );
            Application.Quit(1);
        }
        
        if (this.gameObject.transform.Find("ExplosionParticles") is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + "/ExplosionParticles game " +
                "object was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((ParticleSystem = this.gameObject.transform.Find(
                "ExplosionParticles"
                ).GetComponent<ParticleSystem>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + "/ExplosionParticles game " +
                "object is missing ParticleSystem component."
            );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <DetoniteCharge> - " + this.gameObject.transform.name + " game object is " +
                "missing Animator component."
            );
            Application.Quit(1);
        }
    }
    
    private void Start()
    {
        ParticleSystem.Stop();
        Animator.enabled = false;
        HitRadiusSprite.enabled = false;
        HitRadiusCollider.enabled = false;
    }

    public void Activate()
    {
        StartCoroutine(Explode());
    }
    
    public IEnumerator Explode()
    {
        Coroutine hitRadiusBlinkingCoroutine = StartCoroutine(HitRadiusBlinking());
        Animator.enabled = true;
        AudioManagement.PlayClipAtPoint("DetoniteChargeSound", this.gameObject.transform.position);
        
        yield return new WaitForSeconds(2.5f);
        
        StopCoroutine(hitRadiusBlinkingCoroutine);
        DetoniteChargeSprite.enabled = false;
        HitRadiusSprite.enabled = false;
        ParticleSystem.Play();
        HitRadiusCollider.enabled = true;

        yield return new WaitForSeconds(0.8f);
        
        AudioManagement.RemoveFromMainAudioManagement();
        Destroy(this.gameObject);
    }

    private IEnumerator HitRadiusBlinking()
    {
        while (true)
        {
            HitRadiusSprite.enabled = !HitRadiusSprite.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player;
            if ((player = other.gameObject.GetComponentInParent<Player>()) is not null)
            { 
                AudioManagement.PlayClipAtPoint("HitmarkerSound", player.transform.position);
                player.TakeDamage(int.MaxValue);
                AudioManagement.RemoveFromMainAudioManagement();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player;
            if ((player = other.gameObject.GetComponentInParent<Player>()) is not null)
            { 
                AudioManagement.PlayClipAtPoint("HitmarkerSound", player.transform.position);
                player.TakeDamage(int.MaxValue);
                AudioManagement.RemoveFromMainAudioManagement();
            }
        }
    }
}
