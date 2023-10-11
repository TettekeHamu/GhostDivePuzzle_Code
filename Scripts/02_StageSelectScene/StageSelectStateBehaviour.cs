using System;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージセレクトシーンのStateを管理するMonoBehaviourなクラス
    /// </summary>
    public class StageSelectStateBehaviour : MonoBehaviour,ISetUpScene
    {
        /// <summary>
        /// 選択中のステージ名を表示するText
        /// </summary>
        [SerializeField] private Text stageNameText;
        /// <summary>
        /// タイルを生成する用のコンポーネント
        /// </summary>
        [SerializeField] private TilesManager tilesManager;
        /// <summary>
        /// プレイヤーを管理する用のコンポーネント
        /// </summary>
        [SerializeField] private StageSelectPlayerManager selectPlayerManager;
        /// <summary>
        /// タイトルシーンのStateMachine
        /// </summary>
        private StageSelectStateMachine stageSelectStateMachine;

        private void Awake()
        {
            //テキストを空にする
            stageNameText.text = "";
        }

        /// <summary>
        /// 初期化用の処理(Awakeの代わりに使用する)
        /// </summary>
        void ISetUpScene.SetUpScene()
        {
            //StateMachineの初期化・Stateを渡す
            stageSelectStateMachine = new StageSelectStateMachine(stageNameText, tilesManager, selectPlayerManager);
            stageSelectStateMachine.Initialize(StageSelectSceneModeType.StageCreating);
            //Update()とFixedUpdate()を登録する
            this.UpdateAsObservable()
                .Subscribe(_ => MyUpdate());
            this.FixedUpdateAsObservable()
                .Subscribe(_ => MyFixedUpdate());
        }

        private void MyUpdate()
        {
            //StateMachineのUpdate()を実行
            stageSelectStateMachine.MyUpdate();
        }

        private void MyFixedUpdate()
        {
            //StateMachineのFixedUpdate()を実行
            stageSelectStateMachine.MyFixedUpdate();
        }
    }
}
