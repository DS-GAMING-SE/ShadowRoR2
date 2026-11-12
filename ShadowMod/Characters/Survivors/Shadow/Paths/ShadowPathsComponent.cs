using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.Paths
{
    public class ShadowPathsComponent : MonoBehaviour
    {
        public CharacterBody characterBody;
        private void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
        }

        private void Start()
        {
            if (NetworkServer.active)
            {
                this.TrySetStartingPathToNeutral();
            }
        }
        private void TrySetStartingPathToNeutral()
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(HedgehogUtils.Forms.SuperForm.Artifact.chaosEmeraldArtifactDef) && 
                characterBody.master && characterBody.master.playerStatsComponent && 
                characterBody.master.playerStatsComponent.currentStats.GetStatValueDouble(PerBodyStatDef.totalTimeAlive, BodyCatalog.GetBodyName(characterBody.bodyIndex)) == 0.0)
            {
                ShadowPaths.SetPath(characterBody.master.inventory, ShadowPaths.Path.Neutral);
            }
        }
    }
}