using System;
using UnityEngine;
using UnityEngine.Animations;

namespace TettekeKobo.GhostDivePuzzle
{
    public class ConstraintsTest : MonoBehaviour
    {
        /// <summary>
        /// PositionConstraintコンポーネント
        /// </summary>
        [SerializeField] private PositionConstraint positionConstraint;
        /// <summary>
        /// 追従させたいTransform
        /// </summary>
        private ConstraintSource playerConstraintSource;

        private void Start()
        {
            //追従先のオブジェクトを取得
            var player = FindObjectOfType<PlayerComponentController>();
            //追従先のオブジェクトをソースに設定
            playerConstraintSource.sourceTransform = player.transform;
            //ソースのweightを設定（設定しないと0になり動かない)
            playerConstraintSource.weight = 1;
            //オフセットを追加
            var offset = transform.position - player.transform.position;
            positionConstraint.translationOffset = offset;
            //x軸のみ追従するようにする
            positionConstraint.translationAxis = Axis.X;
            //リストに追加する
            positionConstraint.AddSource(playerConstraintSource);
            //追従するようにする
            positionConstraint.weight = 1f;
        }
    }
}
