using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// パズルアクションシーンのマネージャークラス
    /// </summary>
    public class PuzzleActionStateBehaviour : MonoBehaviour,ISetUpScene
    {
        /// <summary>
        /// プレイヤーのコンポーネント
        /// </summary>
        [SerializeField] private PlayerStateBehaviour playerStateBehaviour;
        /// <summary>
        /// UIを管理するコンポーネント
        /// </summary>
        [SerializeField] private UIManager uiManager;
        /// <summary>
        /// プレイヤーのUIを管理するコンポーネント
        /// </summary>
        [SerializeField] private PlayerUIManager playerUIManager;
        /// <summary>
        /// ギミックを管理するクラス
        /// </summary>
        [SerializeField] private GimmicksBehaviour gimmicksBehaviour;
        /// <summary>
        /// 敵を管理するクラス
        /// </summary>
        [SerializeField] private EnemiesBehaviourController enemiesBehaviourController;
        /// <summary>
        /// シーンを管理するStateMachine
        /// </summary>
        private PuzzleActionStateMachine puzzleActionStateMachine;

        private void Start()
        {
#if UNITY_EDITOR
            SetUpScene();
#else
#endif
        }
        
        public void SetUpScene()
        {
            //Playerの初期化
            playerStateBehaviour.InitializePlayer(playerUIManager);
            //ギミックたちの初期化
            gimmicksBehaviour.InitializeGimmicks();
            //敵の初期化
            enemiesBehaviourController.InitializeEnemies();
            //puzzleActionStateMachineの初期化
            puzzleActionStateMachine = new PuzzleActionStateMachine
                (playerStateBehaviour, enemiesBehaviourController, uiManager, gimmicksBehaviour);
            puzzleActionStateMachine.Initialize(PuzzleActionSceneModeType.ShowStageNumber);
            //Update()とFixedUpdate()を登録する
            this.UpdateAsObservable()
                .Subscribe(_ => MyUpdate())
                .AddTo(this);
            this.FixedUpdateAsObservable()
                .Subscribe(_ => MyFixedUpdate())
                .AddTo(this);
        }

        private void MyUpdate()
        {
            puzzleActionStateMachine.MyUpdate();
        }

        private void MyFixedUpdate()
        {
            puzzleActionStateMachine.MyFixedUpdate();
        }

        /// <summary>
        /// 破棄されたときにおこなう処理
        /// </summary>
        private void OnDestroy()
        {
           puzzleActionStateMachine?.Dispose();
        }
    }
}
