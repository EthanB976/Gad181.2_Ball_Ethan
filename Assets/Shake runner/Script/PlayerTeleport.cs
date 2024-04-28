using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class PlayerTeleport : MonoBehaviour
    {
        [SerializeField] Transform destination;

        [SerializeField] GameObject Player;

        

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                StartCoroutine(Teleport());
            }
            
        }

        IEnumerator Teleport()
        {
            yield return new WaitForSeconds(1);
            Player.transform.position = new Vector3 (destination.transform.position.x, destination.transform.position.y, destination.transform.position.z);
        }

    }
}

