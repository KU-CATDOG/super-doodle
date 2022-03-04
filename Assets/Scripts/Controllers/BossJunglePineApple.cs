using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BossJunglePineApple : MonoBehaviour
    {
        private EarthObject targetPlayer;

        private static bool isPlayerDodging;
        
        private Vector3 targetPos;

        private const float CollisionDistance = 1f;

        public void Init(EarthObject target)
        {
            targetPlayer = target;
            targetPos = target.Controller.ResourceWorldPos;
        }

        private void Update()
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 2f * Time.deltaTime);

            if (targetPlayer == null) return;

            var pineApplePos = transform.localPosition;
            var playerPos = targetPlayer.Controller.ResourceWorldPos;

            if (isPlayerDodging) return;

            if ((playerPos.x - pineApplePos.x) * (playerPos.x - pineApplePos.x) +
                (playerPos.y - pineApplePos.y) * (playerPos.y - pineApplePos.y) >
                CollisionDistance * CollisionDistance) return;

            KnockBackPlayer();
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.SpeedUp);

            Destroy(gameObject);
        }

        public static IEnumerator DodgePineApple(float dodgeTime)
        {
            isPlayerDodging = true;

            yield return new WaitForSeconds(dodgeTime);

            isPlayerDodging = false;
        }

        private void KnockBackPlayer()
        {
            targetPlayer.Radian -= Mathf.PI / 6;

            targetPlayer.MoveSpeed *= 0.6f;
        }
    }
}
