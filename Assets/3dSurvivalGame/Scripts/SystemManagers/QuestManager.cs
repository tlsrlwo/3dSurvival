using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace SUR
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; set; }

        public List<Quest> allActiveQuests;             // 퀘스트를 수락하면 이 List에 추가됨
        public List<Quest> allCompletedQuests;          // 미션을 완료하면 이 List에 추가됨

        [Header("QuestMenu")]
        public GameObject questMenu;                    // 퀘스트 메뉴창 canvas
        public bool isQuestMenuOpen;

        public GameObject activeQuestPrefab;            // 
        public GameObject completedQuestPrefab;         //

        public GameObject questMenuContent;             // 콘텐츠를 추가할 스크롤뷰의 content

        [Header("QuestTracker")]
        public GameObject questTrackerContent;
        public GameObject trackerRowPrefab;

        public List<Quest> allTrackedQuests;

  


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        // Q 버튼 눌러서 UI 열기닫기
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isQuestMenuOpen && !ConstructionManager.Instance.inConstructionMode)
            {
                questMenu.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
                isQuestMenuOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.Q) && isQuestMenuOpen )
            {
                questMenu.SetActive(false);

                if (!CraftingSystem.Instance.isOpen || !InventorySystem.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }
                isQuestMenuOpen = false;
            }

        }


        #region QuestUI
        // 매개변수로 quest를 받아서 questList에 추가해줌. NPC스크립트에서 참조됨(퀘스트를 수락하는 부분)
        public void AddActiveQuest(Quest quest)
        {
            allActiveQuests.Add(quest);
            TrackQuest(quest);
            RefreshQuestList();
        }

        // active 퀘스트 제거 후 completed에 추가해주는 함수. NPC스크립트에서 참조됨(퀘스트 보상 받는 부분)
        public void MarkQuestCompleted(Quest quest)
        {
            // active list 에서 제거
            allActiveQuests.Remove(quest);

            // completed list 에 추가
            allCompletedQuests.Add(quest);

            UnTrackQuest(quest);

            RefreshQuestList();
        }

        public void RefreshQuestList()
        {
            // 추가하기 전에 기존 품목들 삭제하고 다시 밑에 코드 실행. 그래야 활성화된 퀘스트가 완료되도 prefab이 scroll에 남아있는 현상이 안 일어남
            foreach(Transform child in questMenuContent.transform)
            {
                Destroy(child.gameObject);
            }


            // 활성화된 퀘스트들
            foreach(Quest activeQuest in allActiveQuests)
            {
                // activeQuestPrefab 을 생성해서 questMenuContent의 자식오브젝트로 넣는다
                GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
                questPrefab.transform.SetParent(questMenuContent.transform, false);

                // quesPrefab의 QuestRow 스크립트에 접근
                QuestRow qRow = questPrefab.GetComponent<QuestRow>();

                qRow.thisQuest = activeQuest;

                qRow.questName.text = activeQuest.questName;
                qRow.questGiver.text = activeQuest.questGiver;               

                qRow.isActive = true;
                qRow.isTracking = true;

                qRow.coinAmount.text = $"{activeQuest.info.coinReward}";

                //qRow.firstReward.sprite = "";
                qRow.firstRewardAmount.text = "";       // 현재 questInfo 에 설정된 값이 없어서 비워놓음
                // Row.firstRewardAmount.text = activeQuest.info.보상수량; 으로 하면 됨

                //qRow.secondReward.sprite = "";
                qRow.secondRewardAmount.text = "";

            }
            // 완료된 퀘스트들
            foreach (Quest completedQuests in allCompletedQuests)
            {
                
                GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
                questPrefab.transform.SetParent(questMenuContent.transform, false);

                
                QuestRow qRow = questPrefab.GetComponent<QuestRow>();

                qRow.questName.text = completedQuests.questName;
                qRow.questGiver.text = completedQuests.questGiver;

                qRow.isActive = true;
                qRow.isTracking = true;

                qRow.coinAmount.text = $"{completedQuests.info.coinReward}";

                
                qRow.firstRewardAmount.text = "";       
                

                //qRow.secondReward.sprite = "";
                qRow.secondRewardAmount.text = "";

            }
        }

        #endregion


        #region QuestTrackerUI
        public void TrackQuest(Quest quest)
        {
            allTrackedQuests.Add(quest);
            RefreshTrackerList();
        }

        public void UnTrackQuest(Quest quest)
        {
            allTrackedQuests.Remove(quest);
            RefreshTrackerList();
        }

        public void RefreshTrackerList()
        {
            // 기존 리스트 제거. 중첩 막기위해
            foreach (Transform child in questTrackerContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Quest trackedQuest in allTrackedQuests)
            {
                GameObject trackerPrefab = Instantiate(trackerRowPrefab, Vector3.zero, Quaternion.identity);
                trackerPrefab.transform.SetParent(questTrackerContent.transform, false);

                TrackerRow tRow = trackerPrefab.GetComponent<TrackerRow>();

                tRow.questName.text = trackedQuest.questName;
                tRow.description.text = trackedQuest.questDescription;

                var req1 = trackedQuest.info.firstRequirementItem;                  // string
                var req1Amount = trackedQuest.info.firstRequirementAmount;          // int
                var req2 = trackedQuest.info.secondRequirementItem;                 // string
                var req2Amount = trackedQuest.info.secondRequirementAmount;         // int

                if (req2 != "")   // 요구조건이 2개
                {
                    tRow.requirements.text =
                        $"{req1}" + InventorySystem.Instance.CheckItemAmount(req1) + "/" + $"{req1Amount}\n" +
                        $"{req2}" + InventorySystem.Instance.CheckItemAmount(req2) + "/" + $"{req2Amount}\n";
                }
                else              // 요구조건이 1개
                {
                    tRow.requirements.text =
                        $"{req1}" + InventorySystem.Instance.CheckItemAmount(req1) + "/" + $"{req1Amount}\n";
                }


               /* if (trackedQuest.info.secondRequirementItem != "") //요구조건이 2개면
                {
                    tRow.requirements.text = 
                        $"{trackedQuest.info.firstRequirementItem}" + InventorySystem.Instance.CheckItemAmount(trackedQuest.info.firstRequirementItem) + "0/" + $"{trackedQuest.info.firstRequirementAmount}\n" +
                        $"{trackedQuest.info.secondRequirementItem}" + InventorySystem.Instance.CheckItemAmount(trackedQuest.info.secondRequirementItem) + "/" + $"{trackedQuest.info.secondRequirementAmount}\n";
                }
                else
                {
                    tRow.requirements.text = $"{trackedQuest.info.firstRequirementItem}" + InventorySystem.Instance.CheckItemAmount(trackedQuest.info.firstRequirementItem) + "/" + $"{trackedQuest.info.firstRequirementAmount}\n";
                                                        
                }*/
            }
        }


        #endregion

    }
}
