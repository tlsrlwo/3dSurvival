using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{
    [System.Serializable]       // AllGameData 에서 serialize 를 위해 
    public class AllGameData
    {
        // 각각의 데이터들을 클래스로 따로 저장해줌. 
        public PlayerData playerData;

        //public EnvironmentData environmentData;
        //public ConstructionData constructionData;

    }
}
