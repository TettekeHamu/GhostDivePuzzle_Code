using System.Collections;
using DG.Tweening;
using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// アニメーション付きでシーンを読み込むクラス
    /// </summary>
    public class SceneLoadManager : MonoDontDestroySingletonBase<SceneLoadManager>
    {
        /// <summary>
        /// ロード用のImageなどを持つCanvas
        /// </summary>
        [SerializeField] private Canvas canvas;
        /// <summary>
        /// ロードに使う画像
        /// </summary>
        [SerializeField] private Image fadeImage;
        
        //private 

        protected override void Awake()
        {
            //親クラスのAwake()を実行
            base.Awake();
            //canvasをオフにする
            canvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// 次のシーンを読み込む処理
        /// </summary>
        /// <param name="nestSceneName">読み込むシーン名</param>
        /// <param name="loadTime">読み込みの時間</param>
        public void LoadNextScene(string nestSceneName,float loadTime = 1.5f)
        {
            StartCoroutine(LoadSceneCoroutine(nestSceneName, loadTime));
        }

        /// <summary>
        /// シーンの遷移をおこなうコルーチン
        /// </summary>
        /// <param name="nestSceneName">移行先のシーン名</param>
        /// <param name="loadTime">移行に必要な時間</param>
        private IEnumerator LoadSceneCoroutine(string nestSceneName,float loadTime)
        {
            //アニメーションさせる
            yield return StartTransitionSceneAnimeCoroutine(loadTime / 2);
            //シーンを変える
            SceneManager.LoadScene(nestSceneName);
            //1フレーム待つ（この間に切り替え先のSceneのAwake()をおこなう）
            yield return null;
            //アニメーションさせる
            yield return EndTransitionSceneAnimeCoroutine(loadTime / 2);
            //マネージャーにシーンの読み込みが終わったことを伝える
            var setUpScene = GetComponentInterface.FindObjectOfInterface<ISetUpScene>();
            setUpScene?.SetUpScene();
        }

        /// <summary>
        /// シーンを変えるときにおこなうアニメーションのコルーチン
        /// </summary>
        /// <param name="animationTime">アニメーションの時間</param>
        private IEnumerator StartTransitionSceneAnimeCoroutine(float animationTime)
        {
            //Canvasをオンにする
            canvas.gameObject.SetActive(true);
            //Imageの大きさを0にする
            fadeImage.transform.localScale = Vector3.zero;
            //キャンバスをオンに
            canvas.gameObject.SetActive(true);
            //画面サイズを取得（画像の縦横比が変わらないように大きいほうのみを取得）
            var screenSize = new Vector2(Screen.width, Screen.width);
            //大きくする
            yield return fadeImage.transform
                .DOScale(screenSize / 100f, animationTime / 3)
                .WaitForCompletion();
            //小さくする
            yield return fadeImage.transform
                .DOScale(screenSize / 150f, animationTime / 3)
                .WaitForCompletion();
            //大きくして、画面を埋める
            yield return fadeImage.transform
                .DOScale(screenSize / 25f, animationTime / 3)
                .WaitForCompletion();
        }

        /// <summary>
        /// シーンが遷移し終わったあとのアニメーションをおこなうコルーチン
        /// </summary>
        /// <param name="animationTime">アニメーションの時間</param>
        private IEnumerator EndTransitionSceneAnimeCoroutine(float animationTime)
        {
            //画面サイズを取得（画像の縦横比が変わらないように大きいほうのみを取得）
            var screenSize = new Vector2(Screen.width, Screen.width);
            //小さくする
            yield return fadeImage.transform
                .DOScale(screenSize / 150f, animationTime / 3)
                .WaitForCompletion();
            //少し大きくする
            yield return fadeImage.transform
                .DOScale(screenSize / 100f, animationTime / 3)
                .WaitForCompletion();
            //0まで小さくする
            yield return fadeImage.transform
                .DOScale(Vector2.zero, animationTime / 3)
                .WaitForCompletion();
            //キャンバスをオフにする
            canvas.gameObject.SetActive(false);
        }
    }
}
