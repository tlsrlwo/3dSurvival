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

    }
}
