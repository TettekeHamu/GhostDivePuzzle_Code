using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace TettekeKobo.EditorExtension
{
    /// <summary>
    /// Hierarchy上に仕切りを描画するEditor拡張
    /// </summary>
    public class Hierarchy_PartitionEditorExtension : MonoBehaviour
    {
        /// <summary>
        /// UnityEditorが起動した際に処理するようにする
        /// </summary>
        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI() 
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawPartitionOnHierarchy;
        }
        
        /// <summary>
        /// Hierarchy上に仕切りを描画する処理
        /// </summary>
        /// <param name="instanceID">GameObjectに割り当てられたID</param>
        /// <param name="selectionRect">HierarchyのGameObjectの矩形</param>
        private static void DrawPartitionOnHierarchy(int instanceID,Rect selectionRect) 
        {
            //IDに紐づいたGameObjectを取得
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if(gameObject == null) return;
            //タグを取得
            var tag = gameObject.tag;
            //tagが ”Line” なら描画する
            if (tag == "Partition") DrawLine(instanceID, selectionRect, gameObject);
            //位置を調整
            var style = new GUIStyle();
            selectionRect.xMax -= 16;
            selectionRect.xMin += selectionRect.width - 80;
            style.alignment = TextAnchor.MiddleRight;
        }
        
        /// <summary>
        /// 描画する処理
        /// </summary>
        /// <param name="instanceID">GameObjectに割り当てられたID</param>
        /// <param name="selectionRect">HierarchyのGameObjectの矩形</param>
        /// <param name="gameObject">GameObject</param>
        private static void DrawLine(int instanceID,Rect selectionRect, [NotNull] GameObject gameObject) 
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            
            //背景
            EditorGUI.DrawRect(selectionRect,GetBackGroundColor(instanceID,selectionRect));
            selectionRect.xMin = 32;

            //上側の線
            var upperLineRect = selectionRect;
            upperLineRect.yMax = upperLineRect.yMin + 2;
            EditorGUI.DrawRect(upperLineRect,Color.black);

            //下側の線
            var lowerLineRect = selectionRect;
            lowerLineRect.yMin = lowerLineRect.yMax - 2;
            EditorGUI.DrawRect(lowerLineRect,Color.black);

            //文字
            var style = new GUIStyle
            {
                normal =
                {
                    textColor = new Color32(56,56,56,255)
                },
                alignment = TextAnchor.UpperCenter
            };
            EditorGUI.LabelField(selectionRect,gameObject.name,style);
        }

        /// <summary>
        /// 背景色を描画する処理
        /// </summary>
        /// <param name="instanceID">GameObjectに割り当てられたID</param>
        /// <param name="rect">HierarchyのGameObjectの矩形</param>
        /// <returns>色情報</returns>
        private static Color GetBackGroundColor(int instanceID, Rect rect)
        {
            //UnityのHierarchyの色は(56,56,56)です
            if(Selection.Contains(instanceID)) return new Color32(108,108,108,255);
            
            return rect.Contains(Event.current.mousePosition)
                ? new Color32(216, 216, 216, 255)
                : new Color32(199, 199, 199, 255);
        }
    }
}
