using JSAM;
using UnityEngine;

namespace GameMain
{
    public partial class MusicSettingPage : SettingPage
    {
        public override void Init()
        {
            base.Init();
            GetBindComponents(gameObject);
        }

        public override void Open()
        {
            base.Open();
            m_slider_mainVolume.value = AudioManager.MasterVolume;
            m_slider_musicVolume.value = AudioManager.MusicVolume;
            m_slider_sfxVolume.value = AudioManager.SoundVolume;

            ChangeVolumeMuteGraphics(!AudioManager.MasterMuted);
        }

        public override void Close()
        {
            base.Close();
            AudioManager.InternalInstance.SaveVolumeSettings();
        }

        protected override void RegisterEvents()
        {
            m_btn_volumeOn.onClick.AddListener(OnClickVolumeOn);
            m_btn_volumeOff.onClick.AddListener(OnClickVolumeOff);

            m_slider_mainVolume.onValueChanged.AddListener(OnMainVolumeChange);
            m_slider_musicVolume.onValueChanged.AddListener(OnMusicVolumeChange);
            m_slider_sfxVolume.onValueChanged.AddListener(OnSFXVolumeChange);
        }

        protected override void RemoveEvents()
        {
            m_btn_volumeOn.onClick.RemoveListener(OnClickVolumeOn);
            m_btn_volumeOff.onClick.RemoveListener(OnClickVolumeOff);

            m_slider_mainVolume.onValueChanged.RemoveListener(OnMainVolumeChange);
            m_slider_musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChange);
            m_slider_sfxVolume.onValueChanged.RemoveListener(OnSFXVolumeChange);
        }

        private void OnClickVolumeOff()
        {
            AudioManager.MasterMuted = true;
            ChangeVolumeMuteGraphics(false);
        }

        private void OnClickVolumeOn()
        {
            AudioManager.MasterMuted = false;
            ChangeVolumeMuteGraphics(true);
        }

        private void ChangeVolumeMuteGraphics(bool enable)
        {
            if (enable)
            {
                m_trans_volumeOffChose.gameObject.SetActive(false);
                m_trans_volumeOnChose.gameObject.SetActive(true);
            }
            else
            {
                m_trans_volumeOffChose.gameObject.SetActive(true);
                m_trans_volumeOnChose.gameObject.SetActive(false);
            }
        }

        private void OnMainVolumeChange(float value)
        {
            AudioManager.MasterVolume = value;
        }

        private void OnMusicVolumeChange(float value)
        {
            AudioManager.MusicVolume = value;
        }

        private void OnSFXVolumeChange(float value)
        {
            AudioManager.SoundVolume = value;
        }
    }
}