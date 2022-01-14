﻿using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BossHadesMeteor : MonoBehaviour
    {
        private static readonly HashSet<BossHadesMeteor> allMeteor = new HashSet<BossHadesMeteor>();

        public static void DestroyAll()
        {
            foreach (var i in allMeteor)
            {
                Destroy(i.gameObject);
            }

            allMeteor.Clear();
        }

        private EarthObject targetPlayer;
        private float moveSpeed;

        private const float CollisionDistance = 1f;

        public void Init(EarthObject target, float speed)
        {
            targetPlayer = target;
            moveSpeed = speed;
        }

        private void Awake()
        {
            allMeteor.Add(this);
        }

        private void Update()
        {
            var t = transform;
            var pos = t.localPosition;
            pos.y -= moveSpeed * Time.deltaTime;
            t.localPosition = pos;

            if (pos.y < -10)
            {
                allMeteor.Remove(this);
                Destroy(gameObject);
            }

            if (targetPlayer == null) return;

            var playerPos = targetPlayer.Controller.ResourceWorldPos;
            var meteorPos = t.position;

            if ((playerPos.x - meteorPos.x) * (playerPos.x - meteorPos.x) +
                (playerPos.y - meteorPos.y) * (playerPos.y - meteorPos.y) >
                CollisionDistance * CollisionDistance) return;

            Debug.Log("Player hit by meteor!");

            allMeteor.Remove(this);
            Destroy(gameObject);
        }
    }
}
