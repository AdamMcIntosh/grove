﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.CostRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class FaultLine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Fault Line")
        .ManaCost("{R}{R}").HasXInCost()
        .Type("Instant")
        .Text("Fault Line deals X damage to each creature without flying and each player.")
        .FlavorText("We live on the serpent's back.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountPlayer: (e, player) => e.X.GetValueOrDefault(),
              amountCreature: (e, creature) => e.X.GetValueOrDefault(),
              filterCreature: (effect, card) => !card.Has().Flying);

            p.TimingRule(new MassRemoval());
            p.CostRule(new MaxAvailableMana());
          });
    }
  }
}