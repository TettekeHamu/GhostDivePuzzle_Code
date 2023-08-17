using System.Collections.Generic;
using TNRD;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカとオソナエサキを繋ぐ役割のクラス
    /// </summary>
    public class SwitchConnectManager : MonoBehaviour
    {
        /// <summary>
        /// 条件がそろったときスイッチオンにしたいオブジェクト
        /// </summary>
        public SerializableInterface<ISwitchable> switchObject;
        /// <summary>
        /// スイッチをオンにするのに必要なオソナエサキ
        /// </summary>
        [SerializeField] private OfferingPlaceManager[] offeringPlaces;
        /// <summary>
        /// lineRendererに設定したいマテリアル
        /// </summary>
        [SerializeField] private Material lineMaterial;
        /// <summary>
        /// ターゲットの数
        /// </summary>
        private int targetCount;
        /// <summary>
        /// 子オブジェクトとして持つ線を引くオブジェクトのリスト
        /// </summary>
        private readonly List<LineConnector> lineConnectors = new List<LineConnector>();

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            //それぞれのオソナエサキがオソナエされたときに実行したいメソッドを登録
            foreach (var offeringPlace in offeringPlaces)
            {
                offeringPlace.OnCompleteObservable
                    .Subscribe(_ => CompleteOffering())
                    .AddTo(this);
            }
            
            //オソナエサキの個数を設定
            targetCount = offeringPlaces.Length;
            
            for (var i = 0; i < targetCount; i++)
            {
                DrawConnectingLine(switchObject.Value.GetObjectPosition(), offeringPlaces[i].transform.position);
            }
        }
        
        /// <summary>
        /// オソナエサキにオソナエされたときの処理
        /// </summary>
        private void CompleteOffering()
        {
            targetCount--;
            //ターゲットにすべてお墓が持ってこられたらISwitchable側に通知
            if (targetCount == 0)
            {
                DestroyConnectLine();
                switchObject.Value.Activate();
            }
        }

        /// <summary>
        /// スイッチオブジェクトとオソナエサキを繋ぐ線を描画する処理
        /// </summary>
        private void DrawConnectingLine(Vector3 startPos, Vector3 endPos)
        {
            //1個しかAddComponent出来ないため子オブジェクトを作ってそれにアタッチしていく
            var childObject = new GameObject
            {
                transform =
                {
                    parent = transform
                }
            };

            //LineConnectorの設定をおこなう
            var lineObject = childObject.AddComponent<LineConnector>();
            lineConnectors.Add(lineObject);

            //Lineの設定をおこなう
            var line = childObject.AddComponent<LineRenderer>();
            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
            line.endWidth = line.startWidth = 1f;
            line.material = lineMaterial;
            line.textureMode = LineTextureMode.Tile;
            line.sortingLayerName = "MG_GameObject";
            line.sortingOrder = 20;
        }

        /// <summary>
        /// スイッチオブジェクトとオソナエサキを繋ぐ線を破壊する処理
        /// </summary>
        private void DestroyConnectLine()
        {
            foreach (var line in lineConnectors)
            {
                Destroy(line.gameObject);
            }
        }
    }
}
