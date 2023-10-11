using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// タイトルシーンのStateを管理するMonoBehaviourなクラス
    /// </summary>
    public class TitleStateBehaviour : MonoBehaviour
    {
        /// <summary>
        /// UI管理を行うクラスのコンポーネント
        /// </summary>
        [SerializeField] private TitleUIController titleUIController;
        /// <summary>
        /// タイトルシーンのStateMachine
        /// </summary>
        private TitleStateMachine titleStateMachine;

        private void Awake()
        {
            //UIManagerの初期化
            titleUIController.Initialize();
        }

        private void Start()
        {
            //StateMachineのインスタンスを生成
            titleStateMachine = new TitleStateMachine(titleUIController);
            //StateMachineの初期化・Stateを渡す
            titleStateMachine.Initialize(TitleSceneModeType.DataLoading);
        }

        private void Update()
        {
            //StateMachineのUpdate()を実行
            titleStateMachine?.MyUpdate();
        }

        private void FixedUpdate()
        {
            //StateMachineのFixedUpdate()を実行
            titleStateMachine?.MyFixedUpdate();
        }
        
        private void OnDestroy()
        {
            titleStateMachine?.Dispose();
        }
    }
}
