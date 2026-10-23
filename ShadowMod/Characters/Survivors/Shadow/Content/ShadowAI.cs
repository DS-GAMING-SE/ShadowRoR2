using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public static class ShadowAI
    {
        public static void Init(GameObject bodyPrefab, string masterName)
        {
            GameObject master = Modules.Prefabs.CreateBlankMasterPrefab(bodyPrefab, masterName);

            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.1f;
            baseAI.aimVectorMaxSpeed = 360;

            //mouse over these fields for tooltips
            AISkillDriver chaosSnapDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaosSnapDriver.customName = "Use Primary Chaos Snap";
            chaosSnapDriver.skillSlot = SkillSlot.Primary;
            chaosSnapDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            chaosSnapDriver.requireSkillReady = false; //usually false for primaries
            chaosSnapDriver.requireEquipmentReady = false;
            chaosSnapDriver.minUserHealthFraction = float.NegativeInfinity;
            chaosSnapDriver.maxUserHealthFraction = float.PositiveInfinity;
            chaosSnapDriver.minTargetHealthFraction = float.NegativeInfinity;
            chaosSnapDriver.maxTargetHealthFraction = float.PositiveInfinity;
            chaosSnapDriver.minDistance = 0;
            chaosSnapDriver.maxDistance = 20;
            chaosSnapDriver.selectionRequiresTargetLoS = true;
            chaosSnapDriver.selectionRequiresOnGround = false;
            chaosSnapDriver.selectionRequiresAimTarget = false;
            chaosSnapDriver.maxTimesSelected = -1;

            //Behavior
            chaosSnapDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaosSnapDriver.activationRequiresTargetLoS = true;
            chaosSnapDriver.activationRequiresAimTargetLoS = true;
            chaosSnapDriver.activationRequiresAimConfirmation = true;
            chaosSnapDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaosSnapDriver.moveInputScale = 1;
            chaosSnapDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaosSnapDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            chaosSnapDriver.shouldSprint = true; 
            chaosSnapDriver.shouldFireEquipment = false;
            chaosSnapDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 

            //Transition Behavior
            chaosSnapDriver.driverUpdateTimerOverride = -1;
            chaosSnapDriver.resetCurrentEnemyOnNextDriverSelection = false;
            chaosSnapDriver.noRepeat = false;
            chaosSnapDriver.nextHighPriorityOverride = null;

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver chaosSpearDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaosSpearDriver.customName = "Use Secondary Chaos Spear";
            chaosSpearDriver.skillSlot = SkillSlot.Secondary;
            chaosSpearDriver.requireSkillReady = true;
            chaosSpearDriver.minDistance = 0;
            chaosSpearDriver.maxDistance = 30;
            chaosSpearDriver.selectionRequiresTargetLoS = true;
            chaosSpearDriver.selectionRequiresOnGround = false;
            chaosSpearDriver.selectionRequiresAimTarget = false;
            chaosSpearDriver.maxTimesSelected = -1;

            //Behavior
            chaosSpearDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaosSpearDriver.activationRequiresTargetLoS = true;
            chaosSpearDriver.activationRequiresAimTargetLoS = true;
            chaosSpearDriver.activationRequiresAimConfirmation = true;
            chaosSpearDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaosSpearDriver.moveInputScale = 1;
            chaosSpearDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaosSpearDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            chaosSpearDriver.nextHighPriorityOverride = chaosSnapDriver;
            
            AISkillDriver boostDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            boostDriver.customName = "Use Utility Boost";
            boostDriver.skillSlot = SkillSlot.Utility;
            boostDriver.requireSkillReady = true;
            boostDriver.minDistance = 8;
            boostDriver.maxDistance = 20;
            boostDriver.selectionRequiresTargetLoS = true;
            boostDriver.selectionRequiresOnGround = false;
            boostDriver.selectionRequiresAimTarget = false;
            boostDriver.maxTimesSelected = -1;

            //Behavior
            boostDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            boostDriver.activationRequiresTargetLoS = false;
            boostDriver.activationRequiresAimTargetLoS = false;
            boostDriver.activationRequiresAimConfirmation = false;
            boostDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            boostDriver.moveInputScale = 1;
            boostDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            boostDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            boostDriver.noRepeat = true;

            AISkillDriver chaosBoostDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaosBoostDriver.customName = "Use Special Chaos Boost";
            chaosBoostDriver.skillSlot = SkillSlot.Special;
            chaosBoostDriver.requireSkillReady = true;
            chaosBoostDriver.minDistance = 0;
            chaosBoostDriver.maxDistance = 10;
            chaosBoostDriver.selectionRequiresTargetLoS = false;
            chaosBoostDriver.selectionRequiresOnGround = false;
            chaosBoostDriver.selectionRequiresAimTarget = false;
            chaosBoostDriver.maxTimesSelected = -1;
            chaosBoostDriver.maxUserHealthFraction = 0.5f;

            //Behavior
            chaosBoostDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaosBoostDriver.selectionRequiresAimConfirmationByEnemy = true;
            chaosBoostDriver.activationRequiresTargetLoS = false;
            chaosBoostDriver.activationRequiresAimTargetLoS = false;
            chaosBoostDriver.activationRequiresAimConfirmation = false;
            chaosBoostDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaosBoostDriver.moveInputScale = 1;
            chaosBoostDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaosBoostDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.minDistance = 0;
            chaseDriver.maxDistance = float.PositiveInfinity;

            //Behavior
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.activationRequiresAimTargetLoS = false;
            chaseDriver.activationRequiresAimConfirmation = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //recommend taking these for a spin in game, messing with them in runtimeinspector to get a feel for what they should do at certain ranges and such
        }
    }
}
