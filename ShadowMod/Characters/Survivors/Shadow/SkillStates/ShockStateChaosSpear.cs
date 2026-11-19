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
        private string enterSoundString = "";
        private EffectManagerHelper stunVfxInstance;
        protected virtual Material overlayMaterial { get { return ShadowAssets.chaosSpearShockingOverlay; } }
        protected virtual GameObject stunVfxPrefab { get { return null; } }
        private static readonly int Hurt1StateHash = Animator.StringToHash("Hurt1");
        private static readonly int Hurt2StateHash = Animator.StringToHash("Hurt2");
        private static readonly int FlinchStartStateHash = Animator.StringToHash("FlinchStart");
        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            flinchLayerIndex = this.animator.GetLayerIndex("Flinch");
            if (base.sfxLocator && base.sfxLocator.barkSound != "")
            {
                Util.PlaySound(base.sfxLocator.barkSound, base.gameObject);
            }
            Util.PlaySound(this.enterSoundString, base.gameObject);
            this.PlayAnimation("Body", (Random.Range(0, 2) == 0) ? ShockState.Hurt1StateHash : ShockState.Hurt2StateHash);
            if (this.overlayMaterial != null)
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
                this.stunVfxInstance = EffectManager.GetAndActivatePooledEffect(this.stunVfxPrefab.gameObject, base.transform, true);
                this.stunVfxInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = this.shockDuration;
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
