﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class VernalBloom : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Vernal Bloom")
        .ManaCost("{3}{G}")
        .Type("Enchantment")
        .Text("Whenever a forest is tapped for mana, it produces an additional {G}.")
        .FlavorText("Many cultures have legends of a lush, hidden paradise. The elves of Argoth had no need of such stories.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new IncreaseManaOutput(Mana.Green);
            p.CardFilter = (card, effect) => card.Is("forest");
          });
    }
  }
}