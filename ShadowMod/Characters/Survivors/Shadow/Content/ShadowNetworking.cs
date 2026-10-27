using HedgehogUtils.Forms;
using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace ShadowMod.Characters.Survivors.Shadow
{
    public class NetworkKillCooldown : INetMessage
    {
        private NetworkInstanceId netId;
        public NetworkKillCooldown()
        {

        }
        public NetworkKillCooldown(NetworkInstanceId netId)
        {
            this.netId = netId;
        }
        public void OnReceived()
        {
            CharacterBody body = RoR2.Util.FindNetworkCharacterBody(netId);
            if (body && body.skillLocator && body.skillLocator.hasEffectiveAuthority)
            {
                foreach (var skill in body.skillLocator.AllSkills)
                {
                    if (skill.skillDef is KillCooldownSkillDef)
                    {
                        skill.RunRecharge(1);
                    }
                }
            }
        }
        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
        }

        public void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
        }
    }
}
