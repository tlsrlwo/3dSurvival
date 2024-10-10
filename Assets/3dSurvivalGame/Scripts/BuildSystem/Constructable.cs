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

        // ghost �� foundation  ���� �����ϰ� ��ġ �� �� �ְԲ� �����ִ� ����
        public List<GameObject> ghostList = new List<GameObject>();

        public BoxCollider solidCollider; // We need to drag this collider manualy into the inspector

        private void Start()
        {
            mRenderer = GetComponent<Renderer>();

            mRenderer.material = defaultMaterial;
            foreach (Transform child in transform)      // ghost �� list �� �߰�
            {
                ghostList.Add(child.gameObject);
            }

        }
        void Update()
        {
            // �ٴ��̶� collide �ϰ� ��ü�� ��ġ�� ������ �Ǽ� �� �� �ִ� ����(isValidToBeBuilt) �� true
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

        // �Ǽ��� ������Ʈ�� �θ� �ٲ��� �ڱ� �ڽ�����
        public void ExtractGhostMembers()
        {
            foreach (GameObject item in ghostList)
            {
                item.transform.SetParent(transform.parent, true);
                // �Ǽ��� �ϰ� �� �� ghost�� �ݶ��̴��� ���ش�
                item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
                item.gameObject.GetComponent<GhostItem>().isPlaced = true;
            }
        }
    }
}