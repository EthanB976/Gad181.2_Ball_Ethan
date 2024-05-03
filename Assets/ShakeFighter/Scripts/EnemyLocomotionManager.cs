using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace EB
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        NavMeshAgent navMeshAgent;

        public Rigidbody enemyRigidBody;

        public CharacterStats currentTarget;
        public LayerMask dectectionLayer;

        public float distanceFromTarget;
        public float stoppingDistance = 1.2f;

        public float rotationSpeed = 50;


        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();

            if (navMeshAgent == null)
            {
                Debug.Log("NavMeshAgent component not found");
                navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            }
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidBody.isKinematic = false;
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

        public void HandleMoveToTarget()
        {
            if (enemyManager.isPerformingAction && distanceFromTarget > stoppingDistance)
            {
                return;
            }

            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);   
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

         

            //if we are performing an action, stop movement
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }



            HandleRotateTowardsTarget();

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

        }

        private void HandleRotateTowardsTarget()
        {
            //rotate manuallly
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            }
            //rotate with pathfinding (navmesh)
            else
            {
                Vector3 realtiveDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVeclocity = enemyRigidBody.velocity;

                navMeshAgent.enabled = true;
                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(currentTarget.transform.position);
                }
                else
                {
                    Debug.Log("NavMeshAgent is not enabled");
                }
                
                enemyRigidBody.velocity = targetVeclocity;
                transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }

            
        }

    }
}

