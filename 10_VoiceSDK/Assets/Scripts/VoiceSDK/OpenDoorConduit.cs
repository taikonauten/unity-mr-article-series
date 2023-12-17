using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using UnityEngine;

namespace Taikonauten.Unity.ArticleSeries
{
    public class OpenDoorConduit : MonoBehaviour
    {
        [SerializeField] private MRArticleSeriesController mRArticleSeriesController;
        private const string OPEN_DOOR_INTENT = "open_door";

        [MatchIntent(OPEN_DOOR_INTENT)]
        public void OpenDoor(string[] values)
        {
            Debug.Log("OpenDoorConduit -> OpenDoor()");

            string action = values[0];
            string entity = values[1];

            if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(entity))
            {
                Debug.Log("OpenDoorConduit -> OpenDoor(): match");
            }
        }
    }
}
