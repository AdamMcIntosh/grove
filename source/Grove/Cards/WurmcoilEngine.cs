﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class WurmcoilEngine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Wurmcoil Engine")
        .ManaCost("{6}")
        .Type("Artifact Creature - Wurm")
        .Text(
          "{Deathtouch}, {Lifelink}{EOL}When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.")
        .Power(6)
        .Toughness(6)
        .Abilities(
          StaticAbility.Deathtouch,
          StaticAbility.Lifelink,
          C.TriggeredAbility(
            "When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.",
            C.Trigger<ChangeZone>((t, _) => {
              t.From = Zone.Battlefield;
              t.To = Zone.Graveyard;
            }),
            C.Effect<CreateTokens>((e, c) => e.Tokens(
              c.Card
                .Named("Wurm Token")
                .Text("{Deathtouch}")
                .FlavorText("When wurms aren't hungry{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(ManaColors.Colorless)
                .Abilities(StaticAbility.Deathtouch),
              c.Card
                .Named("Wurm Token")
                .Text("{Lifelink}")
                .FlavorText("When wurms aren't hungry{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(ManaColors.Colorless)
                .Abilities(StaticAbility.Lifelink)))));
    }
  }
}