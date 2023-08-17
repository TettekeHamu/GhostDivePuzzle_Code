using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// BoxCollider2dの頂点座標を返す用のメソッド
    /// </summary>
    public static class BoxCollider2DPositionCalculator
    {
        /// <summary>
        /// BoxCollider2dの頂点座標を返すメソッド
        /// </summary>
        /// <param name="collider2D">頂点座標が欲しいBoxCollider2d</param>
        /// <returns>頂点座標</returns>
        public static Vector2[] GetBoxCollide2DVertices(BoxCollider2D collider2D)
        {
            //Colliderの座標と大きさを取得
            var collider2DTransform = collider2D.transform;
            var collider2DScale = collider2DTransform.lossyScale;

            //Colliderの大きさにScaleを合わせる
            collider2DScale.x *= collider2D.size.x;
            collider2DScale.y *= collider2D.size.y;
            
            collider2DScale *= 0.5f;

            //座標を変換
            var centerPosition = collider2DTransform.TransformPoint(collider2D.offset);
            
            var vx = collider2DTransform.right * collider2DScale.x;
            var vy = collider2DTransform.up * collider2DScale.y;

            //左上から座標を設定
            var p1 = -vx + vy ;
            var p2 = vx + vy ;
            var p3 = vx + -vy ;
            var p4 = -vx + -vy;

            //返り値用の配列を用意
            var vertices = new Vector2[4];

            vertices[0] = centerPosition + p1;
            vertices[1] = centerPosition + p2;
            vertices[2] = centerPosition + p3;
            vertices[3] = centerPosition + p4;

            return vertices;
        }

        public static Vector2 GetBoxCollider2DCenter(BoxCollider2D collider2D)
        {
            //Colliderの座標と大きさを取得
            var collider2DTransform = collider2D.transform;
            var collider2DScale = collider2DTransform.lossyScale;

            //Colliderの大きさにScaleを合わせる
            collider2DScale.x *= collider2D.size.x;
            collider2DScale.y *= collider2D.size.y;

            //座標を変換
            var centerPosition = collider2DTransform.TransformPoint(collider2D.offset);

            return centerPosition;
        }
    }
}
