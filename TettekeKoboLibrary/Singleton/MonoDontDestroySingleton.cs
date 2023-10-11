using UnityEngine;

namespace TettekeKobo.Singleton
{
    /// <summary>
    /// DontDestroyなシングルトンの親クラス
    /// シーン内に存在しない時は警告文を出す、複数存在するときは自動的に削除する
    /// </summary>
    /// <typeparam name="T">子クラス</typeparam>
    public class MonoDontDestroySingletonBase<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// インスタンス 
        /// </summary>
        private static T instance;
        
        /// <summary>
        /// インスタンスのGetter 
        /// </summary>
        public static T Instance
        {
            get
            {
                //インスタンスが存在するなら返す
                if (instance != null) return instance;
                
                //インスタンスがなければシーン内から探して取得
                instance = (T)FindObjectOfType(typeof(T));

                //インスタンスがシーン内になければ警告
                if (instance == null)
                {
                    Debug.LogWarning($"{typeof(T)}がシーン内に存在しません！！");
                    return null;
                }

                //取得したインスタンスを返す
                return instance;
            }
        }

        /// <summary>
        /// シーン開始時の処理,virtualにすることで上書きを可能に
        /// </summary>
        protected virtual void Awake()
        {
            //複数同じインスタンスがあれば削除する
            SetupInstance();
        }
        

        /// <summary>
        /// シングルトンの設定をおこなう処理
        /// </summary>
        private void SetupInstance()
        {
            if (instance == null)
            {
                //インスタンスがなければ自身を入れる
                instance = this as T;
            }
            else
            {
                //インスタンスがあれば自身を破棄する
                Debug.LogWarning($"{typeof(T)}が複数存在したため破棄します");
                Destroy(gameObject);
            }
            
            //シーンが変わっても破棄されないようにする
            DontDestroyOnLoad(gameObject);
        }
    }
}
