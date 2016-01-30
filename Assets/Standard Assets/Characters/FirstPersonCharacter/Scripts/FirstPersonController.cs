using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private MouseLook m_MouseLook;

#if UNITY_ANDROID && !UNITY_EDITOR
        private bool vrmode = true;
#else
        private bool vrmode = false;
#endif

        private Camera m_Camera;

        private void Start()
        {
            m_Camera = Camera.main;
			m_MouseLook.Init(transform , m_Camera.transform);
        }

        private void Update()
        {
            if (!vrmode)
            {
                // use mouselook to update the facing of the character and camera
                m_MouseLook.LookRotation(transform, m_Camera.transform);
                // update cursor lock
                m_MouseLook.UpdateCursorLock();
            }
            else
            {
                // (Unity's VR support will update the facing of the camera)
            }

            //we should update the facing of the character to fit the camera
            //GameObject myChild = GameObject.Find("Ethan");
            //myChild.transform.localRotation = m_Camera.transform.localRotation;

            // move the character in the facing direction
            transform.position = transform.position + (m_Camera.transform.forward * m_MoveSpeed);
        }
    }
}
