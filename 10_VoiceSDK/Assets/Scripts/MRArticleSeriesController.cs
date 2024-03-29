using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using Meta.WitAi.Requests;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace Taikonauten.Unity.ArticleSeries
{
    public class MRArticleSeriesController : MonoBehaviour
    {
        [SerializeField] private ARAnchorManager anchorManager;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject uI;
        [SerializeField] private InputActionReference buttonActionLeft;
        [SerializeField] private InputActionReference buttonActionRight;
        [SerializeField] private VoiceService voiceService;
        [SerializeField] private XRRayInteractor rayInteractor;
        private VoiceServiceRequest voiceServiceRequest;
        private VoiceServiceRequestEvents voiceServiceRequestEvents;

        void OnEnable()
        {
            Debug.Log("MRArticleSeriesController -> OnEnable()");

            buttonActionRight.action.performed += OnButtonPressedRightAsync;
            buttonActionLeft.action.performed += OnButtonPressedLeft;
        }

        void OnDisable()
        {
            Debug.Log("MRArticleSeriesController -> OnDisable()");
            buttonActionRight.action.performed -= OnButtonPressedRightAsync;
            buttonActionLeft.action.performed -= OnButtonPressedLeft;
        }

        private void ActivateVoiceService()
        {
            Debug.Log("MRArticleSeriesController -> ActivateVoiceService()");

            if (voiceServiceRequestEvents == null)
            {
                voiceServiceRequestEvents = new VoiceServiceRequestEvents();

                voiceServiceRequestEvents.OnInit.AddListener(OnInit);
                voiceServiceRequestEvents.OnComplete.AddListener(OnComplete);
            }

            voiceServiceRequest = voiceService.Activate(voiceServiceRequestEvents);
        }

        private void DeactivateVoiceService()
        {
            Debug.Log("MRArticleSeriesController -> DeactivateVoiceService()");
            voiceServiceRequest.DeactivateAudio();
        }

        private void OnInit(VoiceServiceRequest request)
        {
            uI.SetActive(true);
        }

        private void OnComplete(VoiceServiceRequest request)
        {
            uI.SetActive(false);
            DeactivateVoiceService();
        }

        private async void OnButtonPressedRightAsync(InputAction.CallbackContext context)
        {
            Debug.Log("MRArticleSeriesController -> OnButtonPressedRightAsync()");

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Pose pose = new(hit.point, Quaternion.identity);
                Result<ARAnchor> result = await anchorManager.TryAddAnchorAsync(pose);

                result.TryGetResult(out ARAnchor anchor);

                if (anchor != null)
                {
                    // Instantiate the door Prefab
                    GameObject _door = Instantiate(door, hit.point, Quaternion.identity);

                    // Unity recommends parenting your content to the anchor.
                    _door.transform.parent = anchor.transform;
                }
            }
        }

        private void OnButtonPressedLeft(InputAction.CallbackContext context)
        {
            Debug.Log("MRArticleSeriesController -> OnButtonPressedLeft()");

            ActivateVoiceService();
        }
    }
}
