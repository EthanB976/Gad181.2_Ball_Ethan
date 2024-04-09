using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EB
{
    public class PlayerCam : MonoBehaviour
    {

        public float senX;
        public float senY;

        public Transform orientation;
        public Transform camHolder;

        float xRotation;
        float yRotation;

        private void Start()
        {
            //Lock curser to middle of screen and make it invisable
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            //get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        public void DoFov(float endValue)
        {
            GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
        }

        public void DoTilt(float zTilt)
        {
            transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        }
    }
}

