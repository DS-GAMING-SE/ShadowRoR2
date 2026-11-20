using EntityStates;
using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace ShadowMod.Characters.Survivors.Shadow.SkillStates.DoomPowerUpgrades
{
    public class GainingDoomPower : EntityState
    {
        protected virtual float duration { get { return 5f; } }
        protected virtual float eyeFlashDurationPercent { get { return 0.58f; } }
        protected virtual float burstDurationPercent { get { return 0.72f; } }
        protected virtual string startSoundString { get { return "Play_dsshadowthehedgehog_doom_power_gain_start"; } }
        protected virtual string endSoundString { get { return "Play_dsshadowthehedgehog_doom_power_gain_end"; } }
        protected State state;

        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
            Util.PlaySound(startSoundString, gameObject);
            PlayAnimation();
        }
        protected virtual void PlayAnimation()
        {

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > duration * eyeFlashDurationPercent && state == State.Start)
            {
                state = State.EyeFlash;
                EyeFlash();
            }
            if (fixedAge > duration * burstDurationPercent && state == State.EyeFlash)
            {
                state = State.End;
                Burst();
                Util.PlaySound(endSoundString, gameObject);
            }
            if (fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }
        }
        public override void OnExit()
        {
            if (state != State.End)
            {
                Util.PlaySound(endSoundString, gameObject);
            }
            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
            Paths.Items.DoomPowerItemNotification(characterBody.master);
            base.OnExit();
        }
        protected virtual void EyeFlash()
        {
            Util.PlaySound("Play_dsshadowthehedgehog_doom_power_eye_flash", gameObject);
        }
        protected virtual void Burst()
        {

        }
        protected enum State
        {
            Start,
            EyeFlash,
            End
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Vehicle;
        }
    }
}
