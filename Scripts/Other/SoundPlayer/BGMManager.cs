using System.Collections.Generic;
using System.Linq;
using TettekeKobo.Singleton;
using UnityEngine;

namespace TettekeKobo.SoundManager
{
    /// <summary>
    /// BGMを管理するシングルトンなクラス
    /// </summary>
    public class BGMManager : MonoDontDestroySingletonBase<BGMManager>
    {
        /// <summary>
        /// BGMをまとめたリスト
        /// </summary>
        [SerializeField] private List<AudioClip> bgmList;
        
        /// <summary>
        /// 音を鳴らすための機能(AudioSource)のコンポーネント
        /// </summary>
        private AudioSource audioSource;
        /// <summary>
        /// BGM名から番号に変更する用のDictionary
        /// </summary>
        private readonly Dictionary<string, int> nameToNumber = new Dictionary<string, int>();
        /// <summary>
        /// 現在生成中のBGMの番号
        /// </summary>
        private int currentBgmNumber = -1;

        
        /// <summary>
        /// リストにあるBGM名をすべて返す処理
        /// </summary>
        public IEnumerable<string> BgmNames
        {
            get
            {
                if (bgmList == null) yield break;
                foreach (var clip in bgmList.Where(clip => clip != null))
                {
                    yield return clip.name;
                }
            }
        }

        /// <summary>
        /// BgmのVolumeのプロパティ
        /// </summary>
        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }

        protected override void Awake()
        {
            //親クラスのawakeを実行
            base.Awake();

            //ディクショナリーに名前と番号を格納する
            for (var i = 0; i < bgmList.Count; i++)
            {
                var clip = bgmList[i];
                if (!nameToNumber.ContainsKey(clip.name))
                {
                    nameToNumber.Add(clip.name, i);
                }
            }

            //AudioSourceの設定
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
        }
        

        /// <summary>
        /// 指定された名前のBGMを再生する処理
        /// </summary>
        /// <param name="bgmName">BGM名</param>
        public void PlayBGM(string bgmName)
        {
            if (!nameToNumber.ContainsKey(bgmName))
            {
                Debug.LogError("一致するBGMはないです");
                return;
            }

            PlayBGM(nameToNumber[bgmName]);
        }

        /// <summary>
        /// 指定された番号のBGMを流す処理
        /// </summary>
        /// <param name="bgmIndex">BGMの番号</param>
        private void PlayBGM(int bgmIndex = 0)
        {
            if (!bgmList.Any())
            {
                Debug.LogError("BgmListにAudioClipが１つも設定されていないため再生できません。");
                return;
            }

            if (bgmIndex < 0 || bgmList.Count <= bgmIndex)
            {
                Debug.LogError("存在しないBGM番号を指定されたため再生できません。");
                return;
            }

            if (bgmIndex == currentBgmNumber)
            {
                //再開
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                //新規再生
                var clip = bgmList[bgmIndex];
                if (audioSource.isPlaying) audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
                currentBgmNumber = bgmIndex;
            }
        }

        /// <summary>
        /// BGMを一時停止する処理
        /// </summary>
        public void PauseBGM()
        {
            if (audioSource.isPlaying) audioSource.Pause();
        }

        /// <summary>
        /// BGMを止める処理
        /// </summary>
        public void StopBGM()
        {
            audioSource.Stop();
            currentBgmNumber = -1;
        }
    }
}
