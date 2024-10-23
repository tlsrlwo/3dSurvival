using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [System.Serializable]
    public class EnvironmentData
    {
        public List<string> pickedUpItems;

        public EnvironmentData(List<string> _pickedupItems)
        {
            pickedUpItems = _pickedupItems;
        }
    }
}