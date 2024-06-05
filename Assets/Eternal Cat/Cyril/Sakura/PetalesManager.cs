using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalesManager : MonoBehaviour
{
    private Collider2D collider;
    private GameObject PetalParticleSystem;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        PetalParticleSystem = GameObject.Find("PetalParticleSystem");

        if (PetalParticleSystem != null)
        {
            PetalParticleSystem.SetActive(false);
            Debug.Log("PetalParticleSystem trouvé et désactivé au démarrage");
        }
        else
        {
            Debug.LogError("PetalParticleSystem n'a pas été trouvé au démarrage");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PetalParticleSystem != null)
            {
                PetalParticleSystem.SetActive(true);
                Debug.Log("PetalParticleSystem activé à l'entrée du trigger");
            }
            else
            {
                Debug.LogError("PetalParticleSystem est null à l'entrée du trigger");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PetalParticleSystem != null)
            {
                PetalParticleSystem.SetActive(false);
                Debug.Log("PetalParticleSystem désactivé à la sortie du trigger");
            }
            else
            {
                Debug.LogError("PetalParticleSystem est null à la sortie du trigger");
            }
        }
    }
}
