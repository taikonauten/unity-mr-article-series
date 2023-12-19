using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Taikonauten.Unity.ArticleSeries
{
    public class MimicCamera : MonoBehaviour
    {
        public float offsetDistance = 30.0f;
        [SerializeField] private GameObject sceneCamera;
        private Camera mainCamera;
        public Transform DoorTransform { get; set; } = null;

        void Awake() {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (DoorTransform == null) {
                return;
            }

            // Calculate the direction and position offset from the door to the main camera
            Vector3 directionToCamera = mainCamera.transform.position - DoorTransform.position;
            Vector3 sceneCameraPosition = DoorTransform.position + directionToCamera.normalized * offsetDistance;

            sceneCameraPosition.y = sceneCameraPosition.y > 2f ? 2f : sceneCameraPosition.y;

            // Update the position and rotation of the scene camera
            sceneCamera.transform.position = sceneCameraPosition;
            sceneCamera.transform.rotation = Quaternion.LookRotation(DoorTransform.position - sceneCameraPosition);
        }
    }
}
