using SUR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{

    public class Constructable : MonoBehaviour
    {
        // Validation
        public bool isGrounded;
        public bool isOverlappingItems;
        public bool isValidToBeBuilt;
        public bool detectedGhostMemeber;

        // Material related
        private Renderer mRenderer;
        public Material redMaterial;
        public Material greenMaterial;
        public Material defaultMaterial;

        // ghost 는 foundation  옆에 균일하게 설치 할 수 있게끔 도와주는 역할
        public List<GameObject> ghostList = new List<GameObject>();

        public BoxCollider solidCollider; // We need to drag this collider manualy into the inspector

        private void Start()
        {
            mRenderer = GetComponent<Renderer>();

            mRenderer.material = defaultMaterial;
            foreach (Transform child in transform)      // ghost 를 list 에 추가
            {
                ghostList.Add(child.gameObject);
            }

        }
        void Update()
        {
            // 바닥이랑 collide 하고 물체와 겹치지 않으면 건설 할 수 있는 상태(isValidToBeBuilt) 가 true
            if (isGrounded && isOverlappingItems == false)
            {
                isValidToBeBuilt = true;
            }
            else
            {
                isValidToBeBuilt = false;
            }
        }

        //--------------------------------------------------------------ColliderTrigger
        #region("ColliderTrigger")

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
            {
                isGrounded = true;
            }

            if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
            {
                isOverlappingItems = true;
            }

            if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
            {
                detectedGhostMemeber = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
            {
                isGrounded = false;
            }

            if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
            {
                isOverlappingItems = false;
            }

            if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
            {
                detectedGhostMemeber = false;
            }
        }


        #endregion


        public void SetInvalidColor()
        {
            if (mRenderer != null)
            {
                mRenderer.material = redMaterial;
            }
        }

        public void SetValidColor()
        {
            mRenderer.material = greenMaterial;
        }

        public void SetDefaultColor()
        {
            mRenderer.material = defaultMaterial;
        }

        // 건설한 오브젝트의 부모를 바꿔줌 자기 자신으로
        public void ExtractGhostMembers()
        {
            foreach (GameObject item in ghostList)
            {
                item.transform.SetParent(transform.parent, true);
                // 건설을 하고 난 뒤 ghost의 콜라이더를 없앤다
                item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
                item.gameObject.GetComponent<GhostItem>().isPlaced = true;
            }
        }
    }
}