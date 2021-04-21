using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBerry : MonoBehaviour
{
    public AudioClip collectedClip;
    public GameObject pickupParticlePrefab;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            //if (controller.health < controller.maxHealth)
            //{
                controller.ChangeHealth(-3);
                Destroy(gameObject);
                GameObject pickupParticleObject = Instantiate(pickupParticlePrefab, transform.position, Quaternion.identity);
            
                controller.PlaySound(collectedClip);
           // }
        }

    }
}
