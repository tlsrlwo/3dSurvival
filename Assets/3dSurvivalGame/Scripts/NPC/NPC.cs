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
        public int activeQuestsIndex = 0;                   // NPC 의 퀘스트 종류 수. 하나 완료하면 또 다른 퀘스트가 있을 경우들
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

            // 첫 만남
            if (firstTimeInteraction)
            {
                firstTimeInteraction = false;
                currentActiveQuest = quests[activeQuestsIndex];
                StartQuestInitialDialogue();
                currentDialogue = 0;

            }
            else // 첫만남이 아닌경우
            {
                // 거절 후 다시 왔을 경우
                if (currentActiveQuest.declined)
                {
                    DialogueSystem.Instance.OpenDialogueUI();

                    npcDialogueText.text = currentActiveQuest.info.comebackAfterDecline;

                    AcceptOrDeclineBTN();
                }

                // 미션을 수락했고, 아직 완료되지 않은 상태 -> 요구사항 충족상태 확인해주는 내용
                if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
                {
                    // 요구사항 충족
                    if (QuestRequirementsCompleted())
                    {
                        SubmitRequiredItems();

                        DialogueSystem.Instance.OpenDialogueUI();

                        npcDialogueText.text = currentActiveQuest.info.comebackCompleted;

                        option1Text.text = "[Take Reward]";
                        option1BTN.onClick.RemoveAllListeners();
                        option1BTN.onClick.AddListener(() =>
                        {
                            ReceiveRewardAndCompleteQuest();
                        });
                    }
                    else // 미션 요구사항이 충족되지 않은 상태에서 돌아왔을 경우
                    {
                        DialogueSystem.Instance.OpenDialogueUI();

                        npcDialogueText.text = currentActiveQuest.info.comebackInProgress;
                        CloseBTNDialogueUI();
                        
                        /* option1Text.text = "[Close]";
                        option1BTN.onClick.RemoveAllListeners();
                        option1BTN.onClick.AddListener(() =>
                        {
                            DialogueSystem.Instance.CloseDialogueUI();
                            isTalkingWithPlayer = false;
                        });*/
                    }

                    
                }

                // 퀘스트가 완료됐을 경우
                if(currentActiveQuest.isCompleted == true)
                {
                    DialogueSystem.Instance.OpenDialogueUI();

                    npcDialogueText.text = currentActiveQuest.info.finalWords;

                    CloseBTNDialogueUI();
                    //option1Text.text = "[Close]";
                    //option1BTN.onClick.RemoveAllListeners();
                    //option1BTN.onClick.AddListener(() =>
                    //{
                    //    DialogueSystem.Instance.CloseDialogueUI();
                    //    isTalkingWithPlayer = false;
                    //});
                }

                // 추가 퀘스트가 있을경우
                if (currentActiveQuest.initialDialogueCompleted == false)
                {
                    StartQuestInitialDialogue();
                }
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
            if (currentDialogue == currentActiveQuest.info.initialDialogue.Count -1)            // 현재 대화가 대화내중에 마지막 (대화내용리스트.카운트의 -1) 이라면
            {
                npcDialogueText.text = currentActiveQuest.info.initialDialogue[currentDialogue];    // 
                currentActiveQuest.initialDialogueCompleted = true;

                AcceptOrDeclineBTN();                
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

        private void AcceptedQuest() 
        {
            // 퀘스트를 수락하면 questManager에 추가됨
            QuestManager.Instance.AddActiveQuest(currentActiveQuest);

            currentActiveQuest.accepted = true;
            currentActiveQuest.declined = false;

            // requirements가 없음 -> 성공함 -> 리워드
            if(currentActiveQuest.hasNoRequirements)
            {
                npcDialogueText.text = currentActiveQuest.info.comebackCompleted;
                option1Text.text = "[Take Reward]";
                option1BTN.onClick.RemoveAllListeners();
                option1BTN.onClick.AddListener(() =>
                {
                    ReceiveRewardAndCompleteQuest();
                });
                option2BTN.gameObject.SetActive(false);
            }
            else // 
            {
                npcDialogueText.text = currentActiveQuest.info.acceptAnswer;

                CloseBTNDialogueUI();
            }

            print("quest accepted"); 
        }

        private void DeclinedQuest()
        {
            currentActiveQuest.declined = true;

            npcDialogueText.text = currentActiveQuest.info.declineAnswer;

            CloseBTNDialogueUI();            

            print("quest declined");
        }

        // 퀘스트에 사용된 아이템 인벤토리에서 제거
        private void SubmitRequiredItems()
        {
            string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
            int firstRequiredItemAmount = currentActiveQuest.info.firstRequirementAmount;

            if (firstRequiredItem != "")
            {
                InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredItemAmount);        
            }

            string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
            int secondRequiredItemAmount = currentActiveQuest.info.secondRequirementAmount;

            if (secondRequiredItem != "")
            {
                InventorySystem.Instance.RemoveItem(secondRequiredItem, secondRequiredItemAmount);
            }
        }



        private bool QuestRequirementsCompleted()
        {
            print("checking requirements");

            // 첫번째 조건
            string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
            int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

            var firstItemCounter = 0;

            foreach (string item in InventorySystem.Instance.itemList)
            {
                if (item == firstRequiredItem)
                {
                    firstItemCounter++;
                }
            }

            // 두번째 조건(있으면)
            string secondRequiredItem = currentActiveQuest.info.firstRequirementItem;
            int secondRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

            var secondItemCounter = 0;

            foreach (string item in InventorySystem.Instance.itemList)
            {
                if (item == secondRequiredItem)
                {
                    secondItemCounter++;
                }
            }

            if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReceiveRewardAndCompleteQuest()
        {
            // 퀘스트 완료하면 questManager 퀘스트 완료 List 에 추가
            QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

            currentActiveQuest.isCompleted = true;

            var coinsReceived = currentActiveQuest.info.coinReward;     // 코인 보상. (숫자값)
            print("You received" + coinsReceived + "gold coins.");

            if(currentActiveQuest.info.rewardItem1 != "")               // 보상(reward item) 칸에 적혀있는게 있다면
            {
                // string 값을 전달받아서 해당 아이템을 보상으로 지급
                InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);       
            }

            if (currentActiveQuest.info.rewardItem2 != "")               
            {                
                InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
            }
                        
            activeQuestsIndex++;                                        // 퀘스트가 여러개 있는 NPC이면 퀘스트Index 증가

            if(activeQuestsIndex < quests.Count)                        // 퀘스트가 남아있다면 (퀘스트가 여러개면 activeQuestsIndex++ 를 해도 퀘스트가 남아있다는 뜻)
            {
                currentActiveQuest = quests[activeQuestsIndex];         // 현재 퀘스트를 n번째 퀘스트로 설정
                currentDialogue = 0;                                    // 대화 정보값을 0번째로 다시 설정
                DialogueSystem.Instance.CloseDialogueUI();
                isTalkingWithPlayer = false;
            }
            else
            {
                DialogueSystem.Instance.CloseDialogueUI();
                isTalkingWithPlayer = false;
                print("no more quests");
            }
        }

        private void AcceptOrDeclineBTN()
        {
            // 수락
            option1Text.text = currentActiveQuest.info.acceptOption;
            option1BTN.onClick.RemoveAllListeners();
            option1BTN.onClick.AddListener(() =>
            {
                AcceptedQuest();
            });

            // 거절
            option2BTN.gameObject.SetActive(true);
            option2Text.text = currentActiveQuest.info.declineOption;
            option2BTN.onClick.RemoveAllListeners();
            option2BTN.onClick.AddListener(() =>
            {
                DeclinedQuest();
            });
        }

        private void CloseBTNDialogueUI()
        {
            option1Text.text = "[Close]";
            option1BTN.onClick.RemoveAllListeners();
            option1BTN.onClick.AddListener(() =>
            {
                DialogueSystem.Instance.CloseDialogueUI();
                isTalkingWithPlayer = false;
            });
            option2BTN.gameObject.SetActive(false);
        }

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
