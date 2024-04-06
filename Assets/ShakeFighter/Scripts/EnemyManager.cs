using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class EnemyManager : CharacterManager
    {

        EnemyLocomotionManager enemyLocomotionManager;
        public bool isPerformingAction;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;
        EnemyAnimatorManager enemyAnimatorManager;


        [Header ("A.I Settings")]
        public float detectionRadius;
        //The higher, and lower, respectively these angles are, the greater detection Field of view (basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Update()
        {
            HandleRecoveryTime();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();

            
             
        }

        private void HandleCurrentAction()
        {
            //if (enemyLocomotionManager != null)
            //{
            //    enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            //}
            
            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDectection();
            }
            else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
            else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            {
                AttackTarget();
            }



        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        #region Attacks
        private void GetNewAttack()
        {
            Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporarySCore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumAttackAngle)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }

                        temporarySCore += enemyAttackAction.attackScore;

                        if (temporarySCore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        private void AttackTarget()
        {
            if (isPerformingAction)
            {
                return;
            }



            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red; //replace red with whatever color you prefer
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

    }
}

