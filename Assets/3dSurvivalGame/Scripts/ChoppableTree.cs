using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [RequireComponent (typeof(BoxCollider))]    
    public class ChoppableTree : MonoBehaviour
    {
        public bool playerInRange;
        public bool canBeChopped;

        public float treeMaxHealth;
        public float treeHealth;

        public Animator animator;

        public float caloriesSpentChoppingWood = 20f;

        private void Start()
        {
            treeHealth = treeMaxHealth; 
            animator = transform.parent.transform.parent.GetComponent<Animator>();
        }

        private void Update()
        {
            if ((canBeChopped))
            {
                GlobalState.Instance.resourceHealth = treeHealth;
                GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }

        public void GetHit()
        {
            animator.SetTrigger("Shake");

            treeHealth -= 1;

            PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

            if (treeHealth <= 0)
            {
                TreeIsDead();
            }
        }
 
        void TreeIsDead()
        {
            Vector3 treePosition = transform.position;

            Destroy(transform.parent.transform.parent.gameObject);
            canBeChopped = false;
            SelectionManager.Instance.selectedTree = null;
            SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

            GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), 
                new Vector3(treePosition.x, treePosition.y + 0.2f, treePosition.z), Quaternion.Euler(0,0,0));
        }
    }
}
