using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;


namespace EB
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;


        public CharacterStats currentTarget;
        public LayerMask dectectionLayer;


        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        public void HandleDectection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, dectectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    //Check for team id

                    Vector3 targetdirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetdirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStats;
                        Debug.Log("Player detected");
                    }
                }
            }
        }


    }
}

