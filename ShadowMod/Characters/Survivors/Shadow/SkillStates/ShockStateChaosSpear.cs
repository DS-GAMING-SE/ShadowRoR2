using EntityStates;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates
{
    public class ShockStateChaosSpear : BaseState, SetStateOnHurt.ICustomState
    {
        private Animator animator;
        private int flinchLayerIndex = -1;
        private float shockDuration;
        private float shockTimer;
        private float healthFraction;
        private float shockInterval = 0.1f;
        private TemporaryOverlayInstance temporaryOverlay;
        private string enterSoundString = "Play_captain_m2_tazed_loop";
        private string exitSoundString = "Stop_captain_m2_tazed_loop";
        private EffectManagerHelper stunVfxInstance;
        protected virtual Material overlayMaterial { get { return ShadowAssets.chaosSpearShockingOverlay; } }
        protected virtual GameObject stunVfxPrefab { get { return ShadowAssets.chaosSpearShockedEffect; } }
        private static readonly int FlinchStartStateHash = Animator.StringToHash("FlinchStart");
        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            if (animator) flinchLayerIndex = this.animator.GetLayerIndex("Flinch");
            if (base.sfxLocator && base.sfxLocator.barkSound != "")
            {
                Util.PlaySound(base.sfxLocator.barkSound, base.gameObject);
            }
            Util.PlaySound(this.enterSoundString, base.gameObject);
            PlayAnimation("Body", (Random.Range(0, 2) == 0) ? ShockState.Hurt1StateHash : ShockState.Hurt2StateHash);
            if (overlayMaterial)
            {
                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    CharacterModel component = modelTransform.GetComponent<CharacterModel>();
                    if (component)
                    {
                        temporaryOverlay = TemporaryOverlayManager.AddOverlay(base.gameObject);
                        temporaryOverlay.duration = shockDuration;
                        temporaryOverlay.originalMaterial = overlayMaterial;
                        temporaryOverlay.AddToCharacterModel(component);
                    }
                }
            }
            if (stunVfxPrefab)
            {
                stunVfxInstance = EffectManager.GetAndActivatePooledEffect(stunVfxPrefab, transform, true);
                float radius = Mathf.Max(characterBody.radius, 1f);
                stunVfxInstance.transform.localScale = new Vector3(radius, radius, radius);
                stunVfxInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = shockDuration;
            }
            if (base.characterBody.healthComponent)
            {
                this.healthFraction = base.characterBody.healthComponent.combinedHealthFraction;
            }
            if (base.characterBody)
            {
                base.characterBody.isSprinting = false;
            }
            if (base.characterDirection)
            {
                base.characterDirection.moveVector = base.characterDirection.forward;
            }
            if (base.rigidbodyMotor)
            {
                base.rigidbodyMotor.moveVector = Vector3.zero;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.shockTimer -= base.GetDeltaTime();
            if (this.shockTimer <= 0f)
            {
                this.shockTimer += shockInterval;
                this.PlayShockAnimation();
            }
            if (base.isAuthority && (base.fixedAge > this.shockDuration || this.healthFraction - base.characterBody.healthComponent.combinedHealthFraction > 0.1f))
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (this.temporaryOverlay != null)
            {
                this.temporaryOverlay.Destroy();
            }
            if (this.stunVfxInstance)
            {
                stunVfxInstance.ReturnToPool();
            }
            Util.PlaySound(this.exitSoundString, base.gameObject);
            base.OnExit();
        }

        private void PlayShockAnimation()
        {
            if (this.animator)
            {
                if (flinchLayerIndex >= 0)
                {
                    this.animator.SetLayerWeight(flinchLayerIndex, 1f);
                    this.animator.Play(FlinchStartStateHash, flinchLayerIndex);
                }
            }
        }

        public void SetCustomInfo(SetStateOnHurt.CustomStateInfo info)
        {
            shockDuration = info.duration;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.shockDuration);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.shockDuration = reader.ReadSingle();
        }
    }
}
