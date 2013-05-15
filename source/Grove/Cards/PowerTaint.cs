﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class PowerTaint : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Power Taint")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant enchantment}{EOL}At the beginning of the upkeep of enchanted enchantment's controller, that player loses 2 life unless he or she pays.{EOL}Cycling {2}")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true)
              {
                Condition = (t, g) => t.OwningCard.AttachedTo.Controller.IsActive
              });

            p.Effect = () => new PayManaOrLooseLife(2, 2.Colorless(), P((e, g) => g.Players.Active));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}