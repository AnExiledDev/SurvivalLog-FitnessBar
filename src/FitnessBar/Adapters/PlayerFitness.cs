using System;
using FitnessBar.Core;
using GameCore.HotUpdate;
using GameCore.HotUpdate.Battle.Logic;

namespace FitnessBar.Adapters;

/// <summary>
/// Reads the player's Fitness the way the game reads it for its own bars (LeadingRole.SyncAttr2UI): the value rounded
/// from its float total, the maximum as a whole-number total. Read only. Null before a player exists.
/// </summary>
internal static class PlayerFitness
{
    public static Fitness? Read()
    {
        var world = GameSingleton.Existing<BattleLogicWorld>();
        var role = world?._AgentManager?.GetLeadingRole();
        var attributes = role == null ? null : AgentTools.GetAgentComponent<AttributeComponent>(role);

        if (attributes?.AttrDict == null || !attributes.AttrDict.ContainsKey(AttrName.Health)) return null;

        var value = (int)Math.Round(attributes.GetTotalValue_Float(AttrName.Health));

        return new Fitness(value, attributes.GetTotalValue_Int(AttrName.MaxHealth));
    }
}
