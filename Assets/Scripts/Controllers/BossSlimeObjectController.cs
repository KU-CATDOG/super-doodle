using System.Collections;
using Tool;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Controllers
{
    public class BossSlimeObjectController : EarthObjectController
    {
        private int hitCount;

        protected override float InvincibleSecond => 0.5f;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        private SpriteController spriteController;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("EarthObjects/Slime", x =>
            {
                resource = Object.Instantiate(x);
                spriteController = resource.GetComponent<SpriteController>();
            });
        }

        protected override void OnAttached()
        {
            hitCount = 0;
            Holder.MoveSpeed = Mathf.PI / 10;
        }

        protected override void OnDetached()
        {
        }

        protected override void UnloadResources()
        {
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // 한대 맞으면 버티고 더 빨라짐
            if (hitCount == 0)
            {
                Holder.MoveSpeed = Mathf.PI / 7.5f;
                //resource.transform.localScale *= 1.3f;
                hitCount++;

                spriteController.SetSprite(1);
                SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.PaperCut);
                spriteController.SetAnimatiorParameter("ChangePhase");
            }
            else
            {
                SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.PaperCut);

                // 승리했으므로 오브젝트 모두 파괴하고 게임 결과창 씬으로 이동시키기
                Debug.Log("Game Win!");
                Holder.StartCoroutine(ShowResult());
            }
        }

        private IEnumerator ShowResult()
        {
            spriteController.HideSprite();
            spriteController.KillCutSprite();
            /*
            float timer = 0;
            while (timer < 1f)
            {
                Holder.MoveSpeed = Mathf.SmoothStep(Holder.MoveSpeed, 0, timer);
                timer += Time.deltaTime;
                yield return null;
            }*/
            GameManager.Inst.gameState = GameManager.GameState.EndGame;
            GameManager.Inst.isRecentGameWin = true;
            yield return new WaitForSecondsRealtime(1f);
            SceneManager.LoadScene("ResultScene");
        }
    }
}
