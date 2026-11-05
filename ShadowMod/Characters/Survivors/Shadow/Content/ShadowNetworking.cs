using HedgehogUtils.Forms;
using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using ShadowMod.Characters.Survivors.Shadow.Components;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class NetworkChaosMarkConsume : INetMessage
    {
        private CharacterBody characterBody;
        private GenericSkill skillSlot;
        public NetworkChaosMarkConsume()
        {

        }
        public NetworkChaosMarkConsume(CharacterBody characterBody, GenericSkill slot)
        {
            this.characterBody = characterBody;
            this.skillSlot = slot;
        }
        public void OnReceived()
        {
            ShadowDamageTypes.ConsumeChaosMark(characterBody, skillSlot);
        }
        public void Serialize(NetworkWriter writer)
        {
            writer.Write(characterBody.netId);
            writer.Write((sbyte)characterBody.skillLocator.GetSkillSlotIndex(skillSlot));
        }

        public void Deserialize(NetworkReader reader)
        {
            characterBody = RoR2.Util.FindNetworkCharacterBody(reader.ReadNetworkId());
            if (characterBody && characterBody.hasAuthority && characterBody.skillLocator)
            {
                skillSlot = characterBody.skillLocator.GetSkillAtIndex(reader.ReadSByte());
            }
        }
    }
}
