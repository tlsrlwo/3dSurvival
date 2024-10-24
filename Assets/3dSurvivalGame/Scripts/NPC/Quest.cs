using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [System.Serializable]
    public class Quest 
    {
        [Header("Bools")]
        public bool accepted;
        public bool declined;
        public bool initialDialogueCompleted;
        public bool isCompleted;

        public bool hasNoRequirements;

        [Header("Quest Info")]
        public QuestInfo info;



    }
}
