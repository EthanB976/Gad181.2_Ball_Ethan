using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class CheckPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var PlayerMovement = other.GetComponent<PlayerMovement>();
            if (PlayerMovement != null)
            {
                PlayerMovement.SetRespawnPoint((Vector3)transform.position);

                Debug.Log("Check Point Set");
            }
        }

    }
}

