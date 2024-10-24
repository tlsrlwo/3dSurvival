using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    // Scriptable Objects are saved in the Assets Folder. CreaseAssetMenu is a method for creating this type of asset.
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
    public class QuestInfo : ScriptableObject
    {
        [TextArea(5, 10)]
        public List<string> initialDialogue;

        //--------------------------------------------------------------Options
        [Header("Options")]
        [TextArea(5, 10)]
        public string acceptOption;             // 수락 옵셕
        [TextArea(5, 10)]
        public string acceptAnswer;             // 수락 시 답변

        [TextArea(5, 10)]
        public string declineOption;            // 거절 옵션
        [TextArea(5, 10)]
        public string declineAnswer;            // 거절 시 답변

        [TextArea(5, 10)]
        public string comebackAfterDecline;     // 거절했다가 다시 돌아온 경우
        [TextArea(5, 10)]
        public string comebackInProgress;       // 요구사항을 다 채우지 못한 상태로 돌아온 경우
        [TextArea(5, 10)]
        public string comebackCompleted;        // 

        [TextArea(5, 10)]
        public string finalWords;

        //--------------------------------------------------------------Rewards
        [Header("Rewards")]
        public int coinReward;        
        public string rewardItem1;        
        public string rewardItem2;

        //--------------------------------------------------------------Requirements
        [Header("Requirements")]        
        public string firstRequirementItem;
        public int firstRequirementAmount;
                
        public string secondRequirementItem;
        public int secondRequirementAmount;

    }
}