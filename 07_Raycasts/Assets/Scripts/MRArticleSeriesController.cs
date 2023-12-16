using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Taikonauten.Unity.ArticleSeries
{
    public class MRArticleSeriesController : MonoBehaviour
    {
        public ActionBasedController controller;
        public InputActionReference buttonAction;
        public XRRayInteractor rayInteractor;

        void OnEnable()
        {
            Debug.Log("MRArticleSeriesController -> OnEnable()");
            buttonAction.action.performed += OnButtonPressed;
        }

        void OnDisable()
        {
            Debug.Log("MRArticleSeriesController -> OnDisable()");
            buttonAction.action.performed -= OnButtonPressed;
        }

        private void OnButtonPressed(InputAction.CallbackContext context)
        {
            Debug.Log("MRArticleSeriesController -> OnButtonPressed()");

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Debug.Log(hit);
            }
        }
    }
}
