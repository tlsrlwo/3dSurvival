using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SUR
{
    [RequireComponent(typeof(Animator))]  // 이거 해놓으면 이 스크립트가 붙은 오브젝트는 animator 컴포넌트가 생성
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
