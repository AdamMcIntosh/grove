﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class AngelicWall : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Wall")
        .ManaCost("{1}{W}")
        .Type("Creature Wall")
        .Text("{Defender}, {Flying}")
        .FlavorText(
          "The air stirred as if fanned by angels wings, and the enemy was turned aside.")
        .Power(0)
        .Toughness(4)        
        .StaticAbilities(Static.Defender, Static.Flying);        
    }
  }
}