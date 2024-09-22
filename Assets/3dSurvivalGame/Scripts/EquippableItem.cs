using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SUR
{
    [RequireComponent(typeof(Animator))]  // �̰� �س����� �� ��ũ��Ʈ�� ���� ������Ʈ�� animator ������Ʈ�� ����
    public class EquippableItem : MonoBehaviour
    {
        public Animator animator;


        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0) && 
                InventorySystem.Instance.isOpen == false &&
                CraftingSystem.Instance.isOpen == false &&
                SelectionManager.Instance.handIsVisible == false)
            {               
                animator.SetTrigger("Hit");
            }
        }

        public void GetHit()
        {
            GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
        }
    }
}
