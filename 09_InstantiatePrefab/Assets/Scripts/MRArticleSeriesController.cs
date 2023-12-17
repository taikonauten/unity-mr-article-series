using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace Taikonauten.Unity.ArticleSeries
{
    public class MRArticleSeriesController : MonoBehaviour
    {
        public ActionBasedController controller;
        public InputActionReference buttonAction;
        public XRRayInteractor rayInteractor;
        public ARAnchorManager anchorManager;
        public GameObject door;

        void OnEnable()
        {
            Debug.Log("MRArticleSeriesController -> OnEnable()");
            buttonAction.action.performed += OnButtonPressedAsync;
        }

        void OnDisable()
        {
            Debug.Log("MRArticleSeriesController -> OnDisable()");
            buttonAction.action.performed -= OnButtonPressedAsync;
        }

        private async void OnButtonPressedAsync(InputAction.CallbackContext context)
        {
            Debug.Log("MRArticleSeriesController -> OnButtonPressed()");

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Pose pose = new(hit.point, Quaternion.identity);
                Result<ARAnchor> result = await anchorManager.TryAddAnchorAsync(pose);

                result.TryGetResult(out ARAnchor anchor);

                if (anchor != null)
                {
                    // Instantiate the door Prefab
                    GameObject _door = Instantiate(door, hit.point, Quaternion.identity);

                    // Unity recommends parenting your content to the anchor
                    // instead of adding a ARAnchor component to the GameObject
                    _door.transform.parent = anchor.transform;
                }
            }
        }
    }
}
