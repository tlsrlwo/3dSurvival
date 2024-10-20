using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SUR.SaveManager;

namespace SUR
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; set; }
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

        #region("References")

        // BackButton
        public Button backBTN;

        // VolumeScreen Slider
        public Slider masterSlider;
        public GameObject masterValue;

        public Slider musicSlider;
        public GameObject musicValue;

        public Slider effectsSlider;
        public GameObject effectsValue;

        #endregion


        private void Start()
        {
            // delegate랑 같은 원리
            backBTN.onClick.AddListener(() =>
            {
                // 매개변수로 지정된 value들을 SaveManager의 VolumeSettings클래스에 전달하고, 그 값을 SaveVolumeSettings함수를 이용해서 Json으로 변환 후 저장
                SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
            });

            StartCoroutine(LoadAndApplyeSettings());

        }

        // 소리설정뿐만 아니라 모든 세팅을 다 한 함수에서 진행하기 위해 LoadAndApplyeSettings()함수를 만들고 안에 여러 세팅들을 호출
        private IEnumerator LoadAndApplyeSettings()
        {
            
            LoadAndSetVolumes();        // 볼륨
            yield return new WaitForSeconds(0.1f);
        }

        private void LoadAndSetVolumes()
        {
            // 다른 클래스의 함수는 싱글톤으로 가져오지만, 다른 클래스 내의 클래스는 using.static 으로 가져옴. 
            VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

            masterSlider.value = volumeSettings.masterVolume;
            musicSlider.value = volumeSettings.music;
            effectsSlider.value = volumeSettings.effects;

            print("Volume settings are loaded");

        }





        private void Update()
        {
            masterValue.GetComponent<TextMeshProUGUI>().text = "" + masterSlider.value + "";
            musicValue.GetComponent<TextMeshProUGUI>().text = "" + musicSlider.value + "";
            effectsValue.GetComponent<TextMeshProUGUI>().text = "" + effectsSlider.value + "";

        }


    }
}
