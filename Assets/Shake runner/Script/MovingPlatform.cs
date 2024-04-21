using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class MovingPlatform : MonoBehaviour
    {

        [SerializeField]
        float speed;

        [SerializeField]
        Transform startpoint, endPoint;

        [SerializeField]
        float changeDirectionDelay;

        private Transform destinationTarget, departTarget;

        private float startTime;

        private float journeyLength;

        bool isWaiting;

        
        void Start()
        {
            departTarget = startpoint;
            destinationTarget = endPoint;

            startTime = Time.time;
            journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!isWaiting)
            {
                if(Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
                {
                    float distCovered = (Time.time - startTime) * speed;

                    float fractionOfJourney = distCovered / journeyLength;

                    transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
                }
                else
                {
                    isWaiting = true;
                    StartCoroutine(changeDeley());
                }
            }
           
        }

        private void ChangeDestination()
        {
            if (departTarget == endPoint && destinationTarget == startpoint)
            {
                departTarget = startpoint;
                destinationTarget = endPoint;
            }
            else
            {
                departTarget = endPoint;
                destinationTarget = startpoint;
            }
        }
        IEnumerator changeDeley()
        {
            yield return new WaitForSeconds(changeDirectionDelay);
            ChangeDestination();
            startTime = Time.time;
            journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
            isWaiting = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                other.transform.parent = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.parent = transform;
            }
        }
    }
}

