using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class ConstructionManager : MonoBehaviour
    {
        public static ConstructionManager Instance { get; set; }
                
        public GameObject itemToBeConstructed;      // Construct 하고 싶은 아이템
        public bool inConstructionMode = false;
        public GameObject constructionHoldingSpot;  // 플레이어에 있는 오브젝트

        public bool isValidPlacement;

        public bool selectingAGhost;
        public GameObject selectedGhost;

        // Materials we store as refereces for the ghosts
        public Material ghostSelectedMat;
        public Material ghostSemiTransparentMat;
        public Material ghostFullTransparentMat;

        public GameObject itemToBeDestroyed;


        // 월드에 존재하는 모든 ghost 들을 추적하는 list생성
        // We keep a reference to all ghosts currently in our world,
        // so the manager can monitor them for various operations
        public List<GameObject> allGhostsInExistence = new List<GameObject>();

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

        public void ActivateConstructionPlacement(string itemToConstruct)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));

            //change the name of the gameobject so it will not be (clone) 아이템 뒤에 (클론) 을 제거하기 위해 item.name = itemToConstruct 로 바꿔줌
            item.name = itemToConstruct;

            item.transform.SetParent(constructionHoldingSpot.transform, false);
            itemToBeConstructed = item;
            itemToBeConstructed.gameObject.tag = "activeConstructable";

            // Disabling the non-trigger collider so our mouse can cast a ray
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;

            // Actiavting Construction mode
            inConstructionMode = true;
        }

        private void GetAllGhosts(GameObject itemToBeConstructed)
        {
            List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

            foreach (GameObject ghost in ghostlist)
            {
                Debug.Log(ghost);
                allGhostsInExistence.Add(ghost);
            }
        }

        // ghost 오브젝트가 다른 오브젝트랑(ghost이거나 buildableObject) overlap 되있을 시 삭제해줌
        private void PerformGhostDeletionScan()
        {

            foreach (GameObject ghost in allGhostsInExistence)
            {
                if (ghost != null)
                {
                    if (ghost.GetComponent<GhostItem>().hasSamePosition == false) // if we did not already add a flag
                    {
                        foreach (GameObject ghostX in allGhostsInExistence)
                        {
                            // First we check that it is not the same object
                            if (ghost.gameObject != ghostX.gameObject)
                            {
                                // If its not the same object but they have the same position
                                if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                                {
                                    if (ghost != null && ghostX != null)
                                    {
                                        // setting the flag
                                        ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                        break;
                                    }

                                }

                            }

                        }

                    }
                }
            }

            foreach (GameObject ghost in allGhostsInExistence)
            {
                if (ghost != null)
                {
                    if (ghost.GetComponent<GhostItem>().hasSamePosition)
                    {
                        DestroyImmediate(ghost);
                    }
                }

            }
        }

        private float XPositionToAccurateFloat(GameObject ghost)
        {
            if (ghost != null)
            {
                // Turning the position to a 2 decimal rounded float
                Vector3 targetPosition = ghost.gameObject.transform.position;
                float pos = targetPosition.x;
                float xFloat = Mathf.Round(pos * 100f) / 100f;
                return xFloat;
            }
            return 0;
        }

        private float ZPositionToAccurateFloat(GameObject ghost)
        {

            if (ghost != null)
            {
                // Turning the position to a 2 decimal rounded float
                Vector3 targetPosition = ghost.gameObject.transform.position;
                float pos = targetPosition.z;
                float zFloat = Mathf.Round(pos * 100f) / 100f;
                return zFloat;

            }
            return 0;
        }

        private void Update()
        {
            // 색상 초록색 혹은 빨강색으로 확인
            if (itemToBeConstructed != null && inConstructionMode)
            {
                if (CheckValidConstructionPosition())
                {
                    isValidPlacement = true;
                    itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                }
                else
                {
                    isValidPlacement = false;
                    itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                }


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var selectionTransform = hit.transform;
                    // ray가 맞은 위치값의 오브젝트의 tag 가 ghost 일 때
                    if (selectionTransform.gameObject.CompareTag("ghost"))
                    {
                        itemToBeConstructed.SetActive(false);
                        selectingAGhost = true;
                        // ray가 맞은 오브젝트를 seletedGhost로 지정
                        selectedGhost = selectionTransform.gameObject;
                    }
                    else
                    {
                        itemToBeConstructed.SetActive(true);
                        selectedGhost = null;
                        selectingAGhost = false;
                    }

                }
            }

            // Left Mouse Click to Place item
            if (Input.GetMouseButtonDown(0) && inConstructionMode)
            {
                // ghost의 위치가 아닌 곳에 건설할 때 (자유건설)
                if (isValidPlacement && selectedGhost == false) // We don't want the freestyle to be triggered when we select a ghost.
                {
                    PlaceItemFreeStyle();                       // 실제 모델
                    DestroyItem(itemToBeDestroyed);             // 인벤토리 내의 아이템
                }
                //  ghost의 위치에 건설할 때
                if (selectingAGhost)
                {
                    PlaceItemInGhostPosition(selectedGhost);
                    DestroyItem(itemToBeDestroyed);

                }
            }
            // X 으로 건축취소                      //TODO - don't destroy the ui item until you actually placed it.
            if (inConstructionMode && Input.GetKeyDown(KeyCode.X))        // 우클릭이 아니고 x로 하는 이유는 인벤토리 창이 실행되어있는 상태에서 우클릭을 하게 되면 한 프레임안에 두번 인식되어서 인벤토리 창에서 건축모드로 안넘어가지고 바로 취소됨(우클릭중첩)
            {
                // 인벤토리 내의 오브젝트 숨김 취소
                itemToBeDestroyed.SetActive(true);
                itemToBeDestroyed = null;

                // 건축 모델 오브젝트
                DestroyItem(itemToBeConstructed);               // DestroyItem 을 먼저 해주고 null로 바꿔줘야됨. 순서가중요. null 로 먼저 바꾸면 destroyItem 이 작동 못함
                itemToBeConstructed = null;

                inConstructionMode = false;
            }
        }

        private void PlaceItemInGhostPosition(GameObject copyOfGhost) //  매개변수 selectedGhost(오브젝트)
        {

            Vector3 ghostPosition = copyOfGhost.transform.position;     
            Quaternion ghostRotation = copyOfGhost.transform.rotation;

            selectedGhost.gameObject.SetActive(false);

            // Setting the item to be active again (after we disabled it in the ray cast)
            itemToBeConstructed.gameObject.SetActive(true);
            // 건설한 오브젝트의 부모를 스스로로 만들어줌(root of our scene)
            itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

            // 오브젝트를 ghost의 위치로 지정해줌 
            itemToBeConstructed.transform.position = ghostPosition;
            itemToBeConstructed.transform.rotation = ghostRotation;

            // Making the Ghost Children to no longer be children of this item
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            // Setting the default color/material
            itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
            itemToBeConstructed.tag = "placedFoundation";

            // 앞전에 비활성화 시킨 solid collider 컴포넌트를 재활성화
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

            //Adding all the ghosts of this item into the manager's ghost bank
            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();

            itemToBeConstructed = null;

            inConstructionMode = false;
        }


        private void PlaceItemFreeStyle()
        {
            // 건설한 오브젝트의 부모를 스스로로 만들어줌(root of our scene)
            itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

            // Making the Ghost Children to no longer be children of this item
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            // Setting the default color/material
            itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
            itemToBeConstructed.tag = "placedFoundation";
            itemToBeConstructed.GetComponent<Constructable>().enabled = false;
            // 앞전에 비활성화 시킨 solid collider 컴포넌트를 재활성화
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

            //Adding all the ghosts of this item into the manager's ghost bank
            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();

            itemToBeConstructed = null;

            inConstructionMode = false;
        }

        private void DestroyItem(GameObject item)
        {
            DestroyImmediate(item);                          
            InventorySystem.Instance.ReCalculateList();            
            CraftingSystem.Instance.RefreshNeededItems();          
        }

        // isValidToBeBuilt 의 상태 확인
        private bool CheckValidConstructionPosition()
        {
            if (itemToBeConstructed != null)
            {
                return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
            }

            return false;
        }
    }
}