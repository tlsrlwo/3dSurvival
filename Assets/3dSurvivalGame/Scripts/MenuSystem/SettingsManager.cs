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
            // delegate�� ���� ����
            backBTN.onClick.AddListener(() =>
            {
                // �Ű������� ������ value���� SaveManager�� VolumeSettingsŬ������ �����ϰ�, �� ���� SaveVolumeSettings�Լ��� �̿��ؼ� Json���� ��ȯ �� ����
                SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
            });

            StartCoroutine(LoadAndApplyeSettings());

        }

        // �Ҹ������Ӹ� �ƴ϶� ��� ������ �� �� �Լ����� �����ϱ� ���� LoadAndApplyeSettings()�Լ��� ����� �ȿ� ���� ���õ��� ȣ��
        private IEnumerator LoadAndApplyeSettings()
        {
            
            LoadAndSetVolumes();        // ����
            yield return new WaitForSeconds(0.1f);
        }

        private void LoadAndSetVolumes()
        {
            // �ٸ� Ŭ������ �Լ��� �̱������� ����������, �ٸ� Ŭ���� ���� Ŭ������ using.static ���� ������. 
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
