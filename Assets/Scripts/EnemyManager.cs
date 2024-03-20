using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class EnemyManager : CharacterManager
    {

        EnemyLocomotionManager enemyLocomotionManager;
        bool isPerformingAction;


        [Header ("A.I Settings")]
        public float detectionRadius;
        //The higher, and lower, respectively these angles are, the greater detection Field of view (basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        private void Update()
        {
             HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            
            
             enemyLocomotionManager.HandleDectection();
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red; //replace red with whatever color you prefer
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

    }
}

