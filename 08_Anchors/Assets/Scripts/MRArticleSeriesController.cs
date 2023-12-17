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

                Debug.Log(anchor);
            }
        }
    }
}
