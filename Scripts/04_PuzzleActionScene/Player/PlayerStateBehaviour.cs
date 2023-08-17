using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーにアタッチするクラス
    /// </summary>
    public class PlayerStateBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 現在のState（Debug用）
        /// </summary>
        [SerializeField] private PlayerStateType playerCurrentState;
        /// <summary>
        /// コンポーネントを扱う用のクラス
        /// </summary>
        [SerializeField] private PlayerComponentController componentController;
        /// <summary>
        /// プレイヤーのStateMachine
        /// </summary>
        private PlayerStateMachine playerStateMachine;
        /// <summary>
        /// プレイヤーのStateMachineのgetter(Stateを取得する用)
        /// </summary>
        public PlayerStateMachine PlayerStateMachine => playerStateMachine;
        
        /// <summary>
        /// 初期化用の処理
        /// </summary>
        public void InitializePlayer(PlayerUIManager uiManager)
        {
            componentController.Initialize(uiManager);
            //StateMachineの初期化をおこなう
            playerStateMachine = new PlayerStateMachine(componentController);
            playerStateMachine.OnChangeStateObservable
                .Subscribe(x => playerCurrentState = x)
                .AddTo(this);
            playerStateMachine.Initialize(PlayerStateType.Idle);
        }
        
        /// <summary>
        /// 毎フレーム実行したい処理
        /// </summary>
        public void MyUpDate()
        {
            playerStateMachine.MyUpdate();
        }

        /// <summary>
        /// 一定間隔でおこないたい処理
        /// </summary>
        public void MyFixedUpDate()
        {
            playerStateMachine.MyFixedUpdate();
        }

        private void LateUpdate()
        {
            //ライトを追従させる
            for (var i = 0; i < componentController.PlayerLights.Length; ++i)
            {
                componentController.PlayerLights[i].transform.position = transform.position;
            }
        }

        /// <summary>
        /// ダイブ先のオハカを設定する処理
        /// </summary>
        /// <param name="tombObject">ダイブ先のオハカ</param>
        public void SetTombObject(TombObjectManager tombObject)
        {
            componentController.SetTombObject(tombObject);
        }

        /// <summary>
        /// ダイブ先のTVを設定する処理
        /// </summary>
        /// <param name="tvObject">ダイブ先のTV</param>
        public void SetTVObject(TVObjectManager tvObject)
        {
            componentController.SetTVObject(tvObject);
        }

        /// <summary>
        /// ダイブ先の扇風機を設定する処理
        /// </summary>
        /// <param name="fanObject"></param>
        public void SetFanObject(FanObjectManager fanObject)
        {
            componentController.SetFanObject(fanObject);
        }

        /// <summary>
        /// ポーズ中などにプレイヤーの移動を停止させるメソッド
        /// </summary>
        public void StopMoving()
        {
            playerStateMachine.StopPlayer();
        }

        /// <summary>
        /// オブジェクトが破棄されたときに非同期処理をキャンセルする用の処理
        /// </summary>
        private void OnDestroy()
        {
            playerStateMachine?.Dispose();
        }
    }
}
