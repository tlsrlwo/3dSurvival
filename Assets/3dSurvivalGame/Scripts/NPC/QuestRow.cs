using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class QuestRow : MonoBehaviour
    {
        public TextMeshProUGUI questName;
        public TextMeshProUGUI questGiver;

        public Button trackingButton;

        public bool isActive;
        public bool isTracking;

        public TextMeshProUGUI coinAmount;

        public Image firstReward;
        public TextMeshProUGUI firstRewardAmount;

        public Image secondReward;
        public TextMeshProUGUI secondRewardAmount;

        public Quest thisQuest;

        private void Start()
        {
            trackingButton.onClick.AddListener(() =>
            {
                if (isActive)       // 퀘스트가 활성화 되어있는지
                {
                    if (isTracking)
                    {
                        isTracking = false;
                        trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Track";
                        QuestManager.Instance.UnTrackQuest(thisQuest);
                    }
                    else
                    {
                        isTracking = true;
                        trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Tracking";
                        QuestManager.Instance.TrackQuest(thisQuest);
                    }
                }

            });
        }


    }
}
