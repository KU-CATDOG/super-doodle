using System.Collections;
using Tool;
using UnityEngine;

namespace Controllers
{
    public class PlayerObjectController : EarthObjectController
    {
        public override ObjectSide Side => ObjectSide.Player;

        protected override bool AttackEnabled => true;

        protected override float InvincibleSecond => 3;

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
            Holder.MoveSpeed = corrected
                ? Mathf.Min(Holder.MoveSpeed + 0.2f, Mathf.PI / 2)
                : Mathf.Max(Holder.MoveSpeed - 0.2f, 0);
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // SceneManager.LoadScene("ResultScene");
            Debug.Log("게임 오버!");
        }
    }
}
