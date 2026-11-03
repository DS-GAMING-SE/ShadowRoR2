using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using ShadowMod.Characters.Survivors.Shadow.Components;
using ShadowMod.Characters.Survivors.Shadow.Paths;
using ShadowMod.Characters.Survivors.Shadow.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class ChaosSnapSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = new InstanceData { tracker = skillSlot.GetComponent<ITargetTracker>(), skillSlot = skillSlot };
            GlobalEventManager.onDeathProcServer += instanceData.OnKilledOther;
            return instanceData;
        }
        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            GlobalEventManager.onDeathProcServer -= ((InstanceData)skillSlot.skillInstanceData).OnKilledOther;
        }
        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            EntityState state = this.InstantiateNextState(skillSlot);
            skillSlot.stateMachine.SetInterruptState(state, this.interruptPriority);
            if (state is not ChaosSnapIn chaosSnap || chaosSnap.rechargeFromChaosMark)
            {
                skillSlot.stock -= this.stockToConsume;
            }
            if (this.cancelSprintingOnActivation)
            {
                skillSlot.characterBody.isSprinting = false;
            }
            if (this.resetCooldownTimerOnUse)
            {
                skillSlot.rechargeStopwatch = 0f;
            }
            if (skillSlot.characterBody)
            {
                skillSlot.characterBody.OnSkillActivated(skillSlot);
            }
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            HurtBox target = GetTarget(skillSlot);
            return target && (skillSlot.stock >= 1 || TargetHasChaosMark(target));
        }
        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState state = base.InstantiateNextState(skillSlot);
            if (state is ChaosSnapIn chaosSnapIn)
            {
                HurtBox target = GetTarget(skillSlot);
                chaosSnapIn.rechargeFromChaosMark = skillSlot.stock >= 1;
                chaosSnapIn.target = target;
                return chaosSnapIn;
            }
            return state;
        }
        private static HurtBox GetTarget([NotNull] GenericSkill skillSlot)
        {
            ITargetTracker tracker = ((InstanceData)skillSlot.skillInstanceData).tracker;
            return (tracker != null) ? tracker.GetTrackingTarget() : null;
        }
        private static bool TargetHasChaosMark(HurtBox target)
        {
            return target && target.healthComponent && target.healthComponent.body.HasBuff(ShadowBuffs.chaosMarkDebuff);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public ITargetTracker tracker;
            public GenericSkill skillSlot;
            public void OnKilledOther(DamageReport damageReport)
            {
                if (damageReport.attackerBody && damageReport.attackerBody == skillSlot.characterBody && damageReport.victimBody && damageReport.victimBody.HasBuff(ShadowBuffs.chaosMarkDebuff) &&
                    damageReport.attackerBody.hasAuthority && skillSlot.CanApplyAmmoPack())
                {
                    skillSlot.ApplyAmmoPack();
                }
            }
        }
    }

    public class KillCooldownSkillDef : SkillDef
    {
        public static void Init()
        {
            On.RoR2.GenericSkill.CanApplyAmmoPack += NoBandolierOnKillCooldownSkillDef;
        }
        private static bool NoBandolierOnKillCooldownSkillDef(On.RoR2.GenericSkill.orig_CanApplyAmmoPack orig, GenericSkill self)
        {
            if (self.skillDef && self.skillDef is KillCooldownSkillDef)
            {
                return false;
            }
            else
            {
                return orig(self);
            }
        }
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = new InstanceData { chaosBoostComponent = skillSlot.GetComponent<ChaosBoostComponent>(), skillSlot = skillSlot };
            instanceData.chaosBoostComponent.AddKillCooldownSkill(skillSlot);
            instanceData.skillSlot.characterBody.onInventoryChanged += instanceData.UpdateKillCooldown;
            if (fullRestockOnAssign && skillSlot.characterBody.inventory)
            {
                instanceData.restockGiven = true;
                skillSlot.characterBody.inventory.GiveItemRented(Items.killCooldownChargeItemDef.itemIndex, requiredStock);
            }
            return instanceData;
        }
        public override void OnUnassigned([NotNull] GenericSkill skillSlot)
        {
            ((InstanceData)skillSlot.skillInstanceData).chaosBoostComponent.RemoveKillCooldownSkill(skillSlot);
            ((InstanceData)skillSlot.skillInstanceData).skillSlot.characterBody.onInventoryChanged -= ((InstanceData)skillSlot.skillInstanceData).UpdateKillCooldown;
        }
        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            skillSlot.characterBody.inventory.RemoveItemRented(Items.killCooldownChargeItemDef.itemIndex, skillSlot.characterBody.inventory.GetItemCountRented(Items.killCooldownChargeItemDef));
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && skillSlot.characterBody.inventory;
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            // overriding fixedupdate gets rid of normal cooldowns
            // apparently, setting the recharge rate to 0 hides the cooldown in the UI, which is bad
            if (skillSlot.characterBody.inventory)
            {
                skillSlot.stock = skillSlot.characterBody.inventory.GetItemCountEffective(Items.killCooldownChargeItemDef) >= Mathf.CeilToInt(skillSlot.finalRechargeInterval) ? 1 : 0;
                if (fullRestockOnAssign && !((InstanceData)skillSlot.skillInstanceData).restockGiven)
                {
                    ((InstanceData)skillSlot.skillInstanceData).restockGiven = true;
                    skillSlot.characterBody.inventory.GiveItemRented(Items.killCooldownChargeItemDef.itemIndex, Mathf.CeilToInt(skillSlot.finalRechargeInterval));
                }
            }
        }
        private class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public ChaosBoostComponent chaosBoostComponent;
            public GenericSkill skillSlot;
            public bool restockGiven = false;
            public void UpdateKillCooldown()
            {
                if (Mathf.CeilToInt(skillSlot.rechargeStopwatch) < skillSlot.characterBody.inventory.GetItemCountEffective(Items.killCooldownChargeItemDef))
                {
                    skillSlot.RunRecharge(skillSlot.characterBody.inventory.GetItemCountEffective(Items.killCooldownChargeItemDef) - skillSlot.rechargeStopwatch);
                }
            }
        }
    }
    public class ChaosBoostSkillDef : KillCooldownSkillDef
    {
        public Sprite upgradeSkillIcon;
        public float upgradeRechargeInterval;
        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            if (upgradeSkillIcon && skillSlot.characterBody && skillSlot.characterBody.HasBuff(ShadowBuffs.chaosBoostBuff))
            {
                return upgradeSkillIcon;
            }
            else
            {
                return base.GetCurrentIcon(skillSlot);
            }
        }
        public override float GetRechargeInterval([NotNull] GenericSkill skillSlot)
        {
            if (skillSlot.characterBody && skillSlot.characterBody.HasBuff(ShadowBuffs.chaosBoostBuff))
            {
                return upgradeRechargeInterval;
            }
            else
            {
                return base.GetRechargeInterval(skillSlot);
            }
        }
    }
}
