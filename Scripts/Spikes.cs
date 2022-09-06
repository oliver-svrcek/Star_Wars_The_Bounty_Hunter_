using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private int Damage { get; set; }

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Spikes> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }

        Damage = int.MaxValue;
    }
    
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player;
            if ((player = other.gameObject.GetComponentInParent<Player>()) is not null)
            { 
                AudioManagement.PlayClipAtPoint("SpikeHitSound", player.transform.position);
                player.TakeDamage(Damage);
            }
        }
    }
}
