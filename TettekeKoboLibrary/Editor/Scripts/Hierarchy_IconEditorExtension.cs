using UnityEditor;
using UnityEngine;

namespace TettekeKobo.EditorExtension
{
    /// <summary>
    /// Hierarchy上にアイコンを表示するEditor拡張
    /// </summary>
    public static class Hierarchy_IconEditorExtension
    {
        /// <summary>
        /// アイコンの横幅（定数）
        /// </summary>
        private const int IconSize = 14;
        
        /// <summary>
        /// UnityEditorが起動した際に処理するようにする
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            //Hierarchyが更新された際にHierarchyWindowItemOnGUIを実行するようにする
            EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnHierarchy;
        }

        /// <summary>
        /// Hierarchy上にアイコンを表示する処理
        /// </summary>
        /// <param name="instanceID">GameObjectに割り当てられたID</param>
        /// <param name="selectionRect">HierarchyのGameObjectの矩形</param>
        private static void DrawIconOnHierarchy(int instanceID, Rect selectionRect)
        {
            //instanceIdからGameObjectを取得
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null) return;
            
            //GameObjectからComponentを取得
            var components = obj.GetComponents<Component>();
            if (components.Length <= 0) return;

            //アイコンを描画する位置を指定（右端）
            selectionRect.x = selectionRect.width - 50f;

            //アイコンの横幅を変更
            selectionRect.width = IconSize;
            
            //アイコンを描画する
            foreach (var component in components)
            {
                //アイコンのTextureを取得
                var texture2D = AssetPreview.GetMiniThumbnail(component);
                //描画
                GUI.DrawTexture(selectionRect, texture2D);
                //アイコンの大きさ分ずらす
                selectionRect.x += IconSize;
            }
        }
    }
}
