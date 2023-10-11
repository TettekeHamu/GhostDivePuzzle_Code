using System;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 障害物やゴールなどを管理するクラス
    /// </summary>
    public class GimmicksBehaviour : MonoBehaviour
    {
        /// <summary>
        /// ステージのゴールになるオブジェクト
        /// </summary>
        private GoalObjectManager goalObject;
        /// <summary>
        /// オハカを格納する配列
        /// </summary>
        private TombObjectManager[] tombObjects;
        /// <summary>
        /// TVを格納する配列
        /// </summary>
        private TVObjectManager[] tvObjects;
        /// <summary>
        /// 扇風機を格納する配列
        /// </summary>
        private FanObjectManager[] fanObjects;
        /// <summary>
        /// 冷蔵庫を格納する配列
        /// </summary>
        private RefrigeratorObjectManager[] refrigeratorObjects;
        /// <summary>
        /// 電子レンジを格納する配列
        /// </summary>
        private MicrowaveManager[] microwaveObjects;
        /// <summary>
        /// オソナエサキとスイッチを繋げるクラスの配列
        /// </summary>
        private SwitchConnectManager[] switchConnects;
        /// <summary>
        /// ジャマモノを格納する配列
        /// </summary>
        private ObstacleObjectManager[] obstacleObjects;

        /// <summary>
        /// ステージのゴールになるオブジェクトのgetter
        /// </summary>
        public GoalObjectManager GoalObject => goalObject;
        
        /// <summary>
        /// 初期化用のメソッド   
        /// </summary>
        public void InitializeGimmicks()
        {
            //オハカを初期化させる
            tombObjects = FindObjectsOfType<TombObjectManager>();
            if (tombObjects.Length > 0)
            {
                foreach (var tomb in tombObjects)
                {
                    tomb.Initialize();
                }   
            }
            
            //TVを初期化させる
            tvObjects = FindObjectsOfType<TVObjectManager>();
            if (tvObjects.Length > 0)
            {
                foreach (var tv in tvObjects)
                {
                    tv.Initialize();
                }
            }
            
            //扇風機を初期化させる
            fanObjects = FindObjectsOfType<FanObjectManager>();
            if (fanObjects.Length > 0)
            {
                foreach (var fan in fanObjects)
                {
                    fan.Initialize();
                }
            }
            
            //冷蔵庫を初期化させる
            refrigeratorObjects = FindObjectsOfType<RefrigeratorObjectManager>();
            if (refrigeratorObjects.Length > 0)
            {
                foreach (var refrigerator in refrigeratorObjects)
                {
                    refrigerator.Initialize();
                }
            }
            
            //電子レンジを初期化させる
            microwaveObjects = FindObjectsOfType<MicrowaveManager>();
            if (microwaveObjects.Length > 0)
            {
                foreach (var microwave in microwaveObjects)
                {
                    microwave.Initialize();
                }
            }
            
            //ジャマモノの初期化をおこなう
            obstacleObjects = FindObjectsOfType<ObstacleObjectManager>();
            if (obstacleObjects.Length > 0)
            {
                foreach (var obstacleObject in obstacleObjects)
                {
                    obstacleObject.Initialize();
                }   
            }
            
            //ゴールを初期化させる
            goalObject = FindObjectOfType<GoalObjectManager>();
            if (goalObject != null)
            {
                goalObject.Initialize();
            }

            //オソナエサキとスイッチを繋げるクラス
            switchConnects = FindObjectsOfType<SwitchConnectManager>();
            if (switchConnects.Length > 0)
            {
                foreach (var switchConnect in switchConnects)
                {
                    switchConnect.Initialize();
                }   
            }
        }

        public void MyUpdate()
        {
            //オハカのUpdateをおこなう
            foreach (var tomb in tombObjects)
            {
                tomb.MyUpdate();
            }  
            //TVのUpdateをおこなう
            foreach (var tv in tvObjects)
            {
                tv.MyUpdate();
            }
            //扇風機のUpdateをおこなう
            foreach (var fan in fanObjects)
            {
                fan.MyUpdate();
            }
            //冷蔵庫のUpdateをおこなう
            foreach (var refrigerator in refrigeratorObjects)
            {
                refrigerator.MyUpdate();
            }
            //電子レンジのUpdateをおこなう
            foreach (var microwave in microwaveObjects)
            {
                microwave.MyUpdate();
            }
        }

        public void MyFixedUpDate()
        {
            //オハカのFixedUpdateをおこなう
            foreach (var tomb in tombObjects)
            {
                tomb.MyFixedUpdate();
            }  
            //TVのFixedUpdateをおこなう
            foreach (var tv in tvObjects)
            {
                tv.MyFixedUpdate();
            }
            //扇風機のFixedUpdateをおこなう
            foreach (var fan in fanObjects)
            {
                fan.MyFixedUpdate();
            }
            //冷蔵庫のFixedUpdateをおこなう
            foreach (var refrigerator in refrigeratorObjects)
            {
                refrigerator.MyFixedUpdate();
            }
            //電子レンジのFixedUpdateをおこなう
            foreach (var microwave in microwaveObjects)
            {
                microwave.MyFixedUpdate();
            }
        }
    }
}
