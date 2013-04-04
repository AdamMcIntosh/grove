﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Congregate : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Congregate")
        .ManaCost("{3}{W}")
        .Type("Instant")
        .Text("Target player gains 2 life for each creature on the battlefield.")
        .FlavorText(
          "In the gathering there is strength for all who founder, renewal for all who languish, love for all who sing.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerGainsLifeEqualToCreatureCount(multiplier: 2);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new EndOfTurn());
            p.TimingRule(new PermanentCountIs(c => c.Is().Creature, 3));
            p.TargetingRule(new SpellOwner());
          });
    }
  }
}