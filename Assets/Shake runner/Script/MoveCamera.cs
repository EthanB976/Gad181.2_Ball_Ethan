using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;
        void Start()
        {
            transform.SetParent(cameraPosition, false);

        }
    }
}

