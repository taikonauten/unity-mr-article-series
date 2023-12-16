using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Taikonauten.Unity.ArticleSeries
{
    public class DeviceSupport : MonoBehaviour
    {
        [SerializeField] ARSession m_Session;

        IEnumerator Start()
        {
            m_Session.attemptUpdate = true;

            if (m_Session == null)
            {
                Debug.LogError("MRCourse -> DeviceSupport (Start): session is null");

                yield break;
            }

            if ((ARSession.state == ARSessionState.None) ||
                (ARSession.state == ARSessionState.CheckingAvailability))
            {
                Debug.Log("MRCourse -> DeviceSupport (Start): check availability");

                yield return ARSession.CheckAvailability();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                // Start some fallback experience for unsupported devices
                Debug.Log("MRCourse -> DeviceSupport (Start): unsupported device");
            }
            else
            {
                // Start the AR session
                Debug.Log("MRCourse -> DeviceSupport (Start): start AR session");

                m_Session.enabled = true;
            }
        }
    }
}
