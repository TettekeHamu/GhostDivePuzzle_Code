using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TettekeKobo.EditorExtension
{
    public class TileMap_ClearTiles : MonoBehaviour
    {
        /// <summary>
        /// グリッド直下にあるすべてのTilemapのTileを削除する処理
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem( "CONTEXT/Grid/Clear All Tiles" )]
        private static void ClearAllTilesFromGrid( MenuCommand menuCommand )
        {
            var grid = menuCommand.context as Grid;
            if ( grid == null ) return;
            var tilemap = grid.GetComponentInChildren<Tilemap>();
            if ( tilemap == null ) return;
            tilemap.ClearAllTiles();
        }

        /// <summary>
        /// 選択したTilemapのTileを削除する処理
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem( "CONTEXT/Tilemap/Clear All Tiles" )]
        private static void ClearAllTiles( MenuCommand menuCommand )
        {
            var tilemap = menuCommand.context as Tilemap;
            if ( tilemap == null ) return;
            tilemap.ClearAllTiles();
        }
    }
}
