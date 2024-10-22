using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance { get; set; }

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


        public GameObject allItems;



    }
}
