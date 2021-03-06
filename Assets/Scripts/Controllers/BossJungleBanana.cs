using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BossJungleBanana : MonoBehaviour
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
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 4f * Time.deltaTime);

            if (targetPlayer == null) return;

            var bananaPos = transform.localPosition;
            var playerPos = targetPlayer.Controller.ResourceWorldPos;

            if (isPlayerDodging) return;

            if ((playerPos.x - bananaPos.x) * (playerPos.x - bananaPos.x) +
                (playerPos.y - bananaPos.y) * (playerPos.y - bananaPos.y) >
                CollisionDistance * CollisionDistance) return;

            targetPlayer.MoveSpeed *= 0.4f;
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.KeyWrong);

            Destroy(gameObject);
        }

        public static IEnumerator DodgeBanana(float dodgeTime)
        {
            isPlayerDodging = true;

            yield return new WaitForSeconds(dodgeTime);

            isPlayerDodging = false;
        }
    }
}
