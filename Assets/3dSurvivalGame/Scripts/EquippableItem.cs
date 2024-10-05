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
                StartCoroutine(SwingSoundDelay());
            }
        }

        public void GetHit()
        {
            GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
        }

        private IEnumerator SwingSoundDelay()
        {
            yield return new WaitForSeconds(0.2f);
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
        }
    }
}
