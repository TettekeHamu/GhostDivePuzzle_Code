using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.SoundManager
{
    /// <summary>
    /// SliderでGBMのボリュームを変更する用の機能
    /// </summary>
    public class BGMVolumeController : MonoBehaviour
    {
        /// <summary>
        /// 音量調節に使うスライダー
        /// </summary>
        [SerializeField] private Slider bgmVolumeSlider;
        /// <summary>
        /// 音量
        /// </summary>
        private readonly FloatReactiveProperty currentVolume = new FloatReactiveProperty(0.5f);
       
        /*
        /// <summary>
        /// 外部から行動する用
        /// </summary>
        public IReadOnlyReactiveProperty<float> CurrentVolume => currentVolume;
        */
        

        void Start()
        {
            //BGMマネージャーを取得
            var bgmManager = BGMManager.Instance;
            if (bgmManager == null)
            {
                Debug.LogWarning("BgmPlayerが見つかりませんでした");
                return;
            }

            //bgmのボリュームを初期設定に合わせる
            currentVolume
                .Subscribe(newVolume => bgmManager.Volume = newVolume)
                .AddTo(this);

            //スライダーがないならリターン
            if (bgmVolumeSlider == null) return;

            //スライダーの位置を音量に合わせる
            currentVolume
                .Subscribe(newVolume => bgmVolumeSlider.value = Mathf.Clamp01(newVolume))
                .AddTo(this);

            //スライダーに合わせて音量が変化するようにする
            bgmVolumeSlider.OnValueChangedAsObservable()
                .Subscribe(newVolume => currentVolume.Value = newVolume)
                .AddTo(this);
            
            bgmVolumeSlider.value = bgmManager.Volume;
        }
    }
}
