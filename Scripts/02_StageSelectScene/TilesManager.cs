using System.Collections.Generic;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// Managerにアタッチする、ナンバータイルを管理する用のコンポーネント
    /// </summary>
    public class TilesManager : MonoBehaviour
    {
        /// <summary>
        /// ナンバータイルのPrefab
        /// </summary>
        [SerializeField] private NumberTileManager numberTilePrefab;
        /// <summary>
        /// ナンバータイル（GameObject）を格納する配列
        /// </summary>
        private NumberTileManager[] numberTiles;
        /// <summary>
        /// 現在選択できるナンバータイルを格納するList
        /// </summary>
        private readonly List<NumberTileManager> canSelectNumberTiles = new List<NumberTileManager>();
        /// <summary>
        /// 現在選択中のナンバータイル
        /// </summary>
        private NumberTileManager currentSelectedTile;
        /// <summary>
        /// 現在選択中のステージ番号
        /// </summary>
        private int currentStageNumber;

        public NumberTileManager[] NumberTiles => numberTiles;
        public List<NumberTileManager> CanSelectNumberTiles => canSelectNumberTiles;
        public NumberTileManager CurrentSelectedTile => currentSelectedTile;
        public int CurrentStageNumber => currentStageNumber;

        /// <summary>
        /// クリアしたタイルを可視化するメソッド
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="num"></param>
        public void ShowClearTiles(bool isShow,int num)
        {
            var startPos = new Vector3(0, -14f, 0);
            numberTiles = new NumberTileManager[num];
            
            //最新のステージのみ見せなくさせる
            if (isShow)
            {
                for (var i = 0; i < num - 1; i++)
                {
                    numberTiles[i] = Instantiate(numberTilePrefab, 
                        startPos + new Vector3(2 * i, 0, 0),
                        Quaternion.Euler(45, 0, 0));
                }   
            }
            //全てのステージを表示
            else
            {
                for (var i = 0; i < num; i++)
                {
                    numberTiles[i] = Instantiate(numberTilePrefab, 
                        startPos + new Vector3(6 * i, 0, 0),
                        Quaternion.Euler(45, 0, 0));
                }
            }

            for (var i = 0; i < num; i++)
            {
                numberTiles[i].Initialize(i);
                canSelectNumberTiles.Add(numberTiles[i]);
            }
        }

        /// <summary>
        /// 新しいタイルを表示する処理
        /// </summary>
        /// <param name="isShow">新しいステージを見せるかどうか</param>
        /// <param name="num">表示するステージ数</param>
        public void ShowNewTile(bool isShow,int num)
        {
            if(!isShow || num > numberTiles.Length) return;
            numberTiles[num].gameObject.SetActive(true);
            canSelectNumberTiles.Add(numberTiles[num]);
        }

        /// <summary>
        /// 現在選択中のナンバータイルを設定する用の処理
        /// </summary>
        public void SetCurrentSelectedTile(int num)
        {
            currentSelectedTile = numberTiles[num];
            currentStageNumber = num;
        }
    }
}
