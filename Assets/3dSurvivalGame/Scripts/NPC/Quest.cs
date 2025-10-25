using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [System.Serializable]
    public class Quest 
    {
        public string questName;
        public string questGiver;
        public string questDescription;


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
