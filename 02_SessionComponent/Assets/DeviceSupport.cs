using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Taikonauten.Unity.ArticleSeries
{
    public class DeviceSupport : MonoBehaviour
    {
        [SerializeField] ARSession session;

        IEnumerator Start()
        {
            session.attemptUpdate = true;

            if (session == null)
            {
                Debug.LogError("DeviceSupport -> Start(): session is null");

                yield break;
            }

            if ((ARSession.state == ARSessionState.None) ||
                (ARSession.state == ARSessionState.CheckingAvailability))
            {
                Debug.Log("DeviceSupport -> Start(): check availability");

                yield return ARSession.CheckAvailability();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                // Start some fallback experience for unsupported devices
                Debug.Log("DeviceSupport -> Start(): unsupported device");
            }
            else
            {
                // Start the AR session
                Debug.Log("DeviceSupport -> Start(): start AR session");

                session.enabled = true;
            }
        }
    }
}
