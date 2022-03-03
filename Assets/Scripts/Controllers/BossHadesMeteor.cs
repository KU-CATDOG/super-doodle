using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BossHadesMeteor : MonoBehaviour
    {
        private static readonly HashSet<BossHadesMeteor> allMeteor = new HashSet<BossHadesMeteor>();

        private const int MaxHit = 3;

        public static void DestroyAll()
        {
            allMeteor.Clear();
        }

        private EarthObject targetPlayer;
        private float moveSpeed;

        private float CollisionDistance;

        public void Init(EarthObject target, float speed, float size)
        {
            targetPlayer = target;
            moveSpeed = speed;
            CollisionDistance = size;
            GetComponent<SpriteController>().SetSprite(size > 0.5f ? 0 : 1);
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

            var hitCount = targetPlayer.GetValue<int>("meteor_hitcount");

            if (hitCount < MaxHit - 1)
            {
                // 운석에 맞은 횟수를 플레이어 오브젝트에 저장한다.
                targetPlayer.SetValue("meteor_hitcount", hitCount + 1);
            }
            else
            {
                Destroy(targetPlayer.gameObject);
                SceneManager.LoadScene("ResultScene");
            }

            allMeteor.Remove(this);
            Destroy(gameObject);
        }
    }
}
