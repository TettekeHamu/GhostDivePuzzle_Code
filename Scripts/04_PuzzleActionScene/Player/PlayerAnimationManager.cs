using System.Linq;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーのアニメーション関連を管理するクラス
    /// </summary>
    public class PlayerAnimationManager : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーのAnimatorコンポーネント
        /// </summary>
        [SerializeField] private Animator playerAnimator;
        /// <summary>
        /// プレイヤーのスプライトレンダラー
        /// </summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        /// <summary>
        /// 移動しているかどうかを表すint値
        /// </summary>
        private readonly int isMoving = Animator.StringToHash("IsMoving");
        
        // オハカ関連
        /// <summary>
        /// ダイブしているかどうかを表すint値
        /// </summary>
        private readonly int isDiving = Animator.StringToHash("IsDiving");
        /// <summary>
        /// 落下しているかどうか
        /// </summary>
        private readonly int isFalling = Animator.StringToHash("IsFalling");
        /// <summary>
        /// オソナエできたかどうか
        /// </summary>
        private readonly int isOffering = Animator.StringToHash("IsOffering");
        
        // TV関連
        /// <summary>
        /// TVにダイブしているかどうか
        /// </summary>
        private readonly int isTVDiving = Animator.StringToHash("IsTVDiving");
        /// <summary>
        /// TVにダイブ中に落下しているかどうか
        /// </summary>
        private readonly int isTVFalling = Animator.StringToHash("IsTVFalling");

        /// 扇風機関連
        /// <summary>
        /// 扇風機にダイブしているかどうか
        /// </summary>
        private readonly int isFanDiving = Animator.StringToHash("IsFanDiving");
        /// <summary>
        /// 扇風機にダイブ中に落下してるかどうか
        /// </summary>
        private readonly int isFanFalling = Animator.StringToHash("IsFanFalling");

        /// 冷蔵庫関連
        /// <summary>
        /// 冷蔵庫にダイブしているかどうか
        /// </summary>
        private readonly int isRefrigeratorDiving = Animator.StringToHash("IsRefrigeratorDiving");
        /// <summary>
        /// 冷蔵庫にダイブ中に落下しているかどうか
        /// </summary>
        private readonly int isRefrigeratorFalling = Animator.StringToHash("IsRefrigeratorFalling");

        ///電子レンジ関連
        /// <summary>
        /// 
        /// </summary>
        private readonly int isMicrowaveDiving = Animator.StringToHash("IsMicrowaveDiving");
        /// <summary>
        /// 
        /// </summary>
        private readonly int isMicrowaveFalling = Animator.StringToHash("IsMicrowaveFalling");
        
        public Animator PlayerAnimator => playerAnimator;
        public int IsMoving => isMoving;
        public int IsDiving => isDiving;
        public int IsFalling => isFalling;
        public int IsOffering => isOffering;
        public int IsTVDiving => isTVDiving;
        public int IsTVFalling => isTVFalling;
        public int IsFanDiving => isFanDiving;
        public int IsFanFalling => isFanFalling;
        public int IsRefrigeratorDiving => isRefrigeratorDiving;
        public int IsRefrigeratorFalling => isRefrigeratorFalling;
        public int IsMicrowaveDiving => isMicrowaveDiving;
        public int IsMicrowaveFalling => isMicrowaveFalling;

        /// <summary>
        /// SpriteRendererの向きを変更する処理
        /// </summary>
        /// <param name="isL">trueなら左向きにする</param>
        public void ChangeSpriteFlipX(bool isL)
        {
            spriteRenderer.flipX = isL;
        }

        /// <summary>
        /// アニメーションの長さを取得する処理
        /// </summary>
        /// <param name="animationName">取得したいアニメーションの名前</param>
        /// <returns>アニメーションのかかる時間</returns>
        public float GetAnimationLength(string animationName)
        {
            //アニメーションの長さを取得
            var clip = playerAnimator
                .runtimeAnimatorController
                .animationClips
                .FirstOrDefault(x => x.name == animationName);
            if (clip != null) return clip.length;
            //取得できなかったら0を返す
            else return 0;
        }
    }
}
