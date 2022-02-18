using System.Collections;
using Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class PlayerObjectController : EarthObjectController
    {
        public override ObjectSide Side => ObjectSide.Player;

        protected override bool AttackEnabled => true;

        protected override float InvincibleSecond => 3;

        protected float maxMoveSpeed =
#if UNITY_EDITOR
            Mathf.PI;
#else
            Mathf.PI / 2;
#endif

        private bool isOnAnimation = false;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("EarthObjects/Player", x =>
            {
                resource = Object.Instantiate(x);
            });
        }

        protected override void OnAttached()
        {
            Holder.MoveSpeed = 0;
        }

        protected override void OnDetached()
        {
        }

        protected override void UnloadResources()
        {
        }

        public override void OnUpdate()
        {
        }

        private float Mod(float x, float m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public override void OnEarthKeyPressed(bool corrected)
        {
#if UNITY_EDITOR
            Holder.MoveSpeed = corrected
                ? Mathf.Min(Holder.MoveSpeed + Mathf.PI, maxMoveSpeed)
                : Mathf.Max(Holder.MoveSpeed - 1f, 0);
#else
            // 이쪽이 빌드시 반영되는 수치
            Holder.MoveSpeed = corrected
                ? Mathf.Min(Holder.MoveSpeed + 0.2f, maxMoveSpeed)
                : Mathf.Max(Holder.MoveSpeed - 0.2f, 0);
#endif
        }

        public override void OnMeleeReady()
        {
            base.OnMeleeReady();
            if (!isOnAnimation)
            {
                Holder.StartCoroutine(MeleeReady());
            }
        }

        IEnumerator MeleeReady()
        {
            isOnAnimation = true;
            Holder.MoveSpeed += Mathf.PI;
            yield return new WaitForSeconds(0.3f);
            Holder.MoveSpeed /= 6;
            isOnAnimation = false;
        }

        public override void OnMeleeAttack()
        {
            base.OnMeleeAttack();
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.KnifeDash);
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.PaperTear);
            GameManager.Inst.gameState = GameManager.GameState.EndGame;
            GameManager.Inst.isRecentGameWin = false;
            Object.Destroy(Holder);
            Debug.Log("게임 오버!");
            SceneManager.LoadScene("ResultScene");
        }
    }
}
