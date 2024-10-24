using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUR
{
    public class NPC : MonoBehaviour
    {
        // NPC dialogue UI
        public bool playerInRange;

        public bool isTalkingWithPlayer;

        TextMeshProUGUI npcDialogueText;

        Button option1BTN;
        TextMeshProUGUI option1Text;

        Button option2BTN;
        TextMeshProUGUI option2Text;

        // NPC quests related
        public List<Quest> quests;
        public Quest currentActiveQuest = null;
        public int activeQuestsIndex = 0;
        public bool firstTimeInteraction = true;
        public int currentDialogue;



        private void Start()
        {
            npcDialogueText = DialogueSystem.Instance.dialogueText;

            option1BTN = DialogueSystem.Instance.option1BTN;
            option1Text = DialogueSystem.Instance.option1BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

            option2BTN = DialogueSystem.Instance.option2BTN;
            option2Text = DialogueSystem.Instance.option2BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        }



        public void StartConversation()
        {
            isTalkingWithPlayer = true;
            
            LookAtPlayer();

            // NPC 한테 한 종류 이상의 퀘스트가 있을 수도 있음
            if (firstTimeInteraction)
            {
                firstTimeInteraction = false;
                currentActiveQuest = quests[activeQuestsIndex];
                StartQuestInitialDialogue();
                currentDialogue = 0;

            }
            else
            {

            }           
        }

        private void StartQuestInitialDialogue()
        {
            DialogueSystem.Instance.OpenDialogueUI();

            npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];    // initialDiagloue 의 리스트에서 인덱스의 dialogue 를 가져옴
            option1Text.text = "Next";
            option1BTN.onClick.RemoveAllListeners();    // 기존의 버튼 기능을 없애줌(다른 동작을 하지 않게 하기 위해)
            option1BTN.onClick.AddListener(() =>        // 새로운 기능 추가
            {
                currentDialogue++;
                CheckIfDialogueDone();      // 추가 내용이 있는지 확인해주는 함수
            });
            option2BTN.gameObject.SetActive(false);
        }

        private void CheckIfDialogueDone()
        {
            if (currentDialogue == currentActiveQuest.info.initialDialogue.Count - 1)            // 현재 대화가 대화내용(리스트)중에 마지막 -1 이라면
            {
                npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];    // 현재 대화내용을 마지막으로 해줌
                currentActiveQuest.initialDialogueCompleted = true;

                // 옵션 1
                option1Text.text = currentActiveQuest.info.acceptOption;                        // 버튼1 의 내용을 퀘스트수락 내용으로 변경
                option1BTN.onClick.RemoveAllListeners();                                        // 기존 버튼 기능 삭제
                option1BTN.onClick.AddListener(() =>                                           // 새 버튼 기능 
                {
                    AcceptedQuest();
                });

                // 옵션 2
                option2BTN.gameObject.SetActive(true);                                          // 대화가 끝나면 선택지 제공을 위해 옵션2버튼 active
                option2Text.text = currentActiveQuest.info.declineOption;                       // 버튼2 내용 퀘스트 거부
                option2BTN.onClick.RemoveAllListeners();
                option2BTN.onClick.AddListener(() =>
                {
                    DeclinedQuest();
                });
            }
            else        // 아직 대화가 안끝났다면 
            {
                npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];

                option1Text.text = "Next";
                option1BTN.onClick.RemoveAllListeners();
                option1BTN.onClick.AddListener(() =>
                {
                    currentDialogue++;
                    CheckIfDialogueDone();
                });
            }
        }

        private void AcceptedQuest() { print("quest accepted"); }

        private void DeclinedQuest() { print("quest declined"); }




        public void LookAtPlayer()
        {
            var player = PlayerState.Instance.playerBody.transform;
            Vector3 direction = player.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);

            var yRotation = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, yRotation, 0); 

        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }
    }
}
