using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class PlayerTeleport : MonoBehaviour
    {
        [SerializeField] Transform destination;

        PlayerMovement pm;

        void Start ()
        {
            pm = GetComponent<PlayerMovement>();    
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                Debug.Log("Teleporting");
                pm.Teleport(destination.position, destination.rotation);

            }
        }

    }
}

