﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class DarkRitual : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Ritual")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Add {B}{B}{B} to your mana pool.")
        .FlavorText(
          "'From void evolved Phyrexia. Great Yawgmoth, Father of Machines, saw its perfection. Thus The Grand Evolution began.'{EOL}—Phyrexian Scriptures")
        .OverrideScore(new ScoreOverride {Hand = 80}) /* ritual score must be lowered a bit so ai casts it more eagerly */
        .Cast(p =>
          {
            p.TimingRule(new ControllerNeedsAdditionalMana(2));
            p.Effect = () => new AddManaToPool("{B}{B}{B}".ParseMana());
          });
    }
  }
}