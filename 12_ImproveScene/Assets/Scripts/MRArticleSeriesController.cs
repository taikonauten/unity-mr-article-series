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
        private GameObject doorInstance;

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

            if (doorInstance != null)
            {
                Debug.Log("MRArticleSeriesController -> OnButtonPressedRightAsync(): Door already instantiated");

                return;
            }

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Pose pose = new(hit.point, Quaternion.identity);
                Result<ARAnchor> result = await anchorManager.TryAddAnchorAsync(pose);

                result.TryGetResult(out ARAnchor anchor);

                if (anchor != null)
                {
                    // Instantiate the door Prefab
                    doorInstance = Instantiate(door, hit.point, Quaternion.identity);

                    // Unity recommends parenting your content to the anchor.
                    doorInstance.transform.parent = anchor.transform;

                    // Make the door face the user after instantiating
                    doorInstance.transform.LookAt(new Vector3(
                        Camera.main.transform.position.x,
                        doorInstance.transform.position.y,
                        Camera.main.transform.position.z
                    ));

                    doorInstance.GetComponentInChildren<MimicCamera>().DoorTransform = doorInstance.transform;
                }
            }
        }

        private void OnButtonPressedLeft(InputAction.CallbackContext context)
        {
            Debug.Log("MRArticleSeriesController -> OnButtonPressedLeft()");

            ActivateVoiceService();
        }

        public void OpenDoor()
        {
            Debug.Log("MRArticleSeriesController -> OpenDoor()");

            if (doorInstance == null)
            {
                Debug.Log("MRArticleSeriesController -> OpenDoor(): no door instantiated yet.");

                return;
            }

            Animator animator = doorInstance.GetComponentInChildren<Animator>();
            animator.Play("DoorAnimation", -1, 0);
        }
    }
}
