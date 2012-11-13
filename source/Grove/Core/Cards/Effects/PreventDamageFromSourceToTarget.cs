﻿namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;
  using Preventions;
  using Targeting;

  public class PreventDamageFromSourceToTarget : Effect
  {
    private ITarget DamageSource { get { return Targets[0]; } }
    public bool OnlyOnce { get; set; }    

    protected override void ResolveEffect()
    {
      var source = DamageSource.IsEffect()
        ? DamageSource.Effect().Source.OwningCard
        : DamageSource.Card();

      var prevention = Builder
        .Prevention<PreventDamageFromSource>(p =>
          {
            p.Source = source;
            p.OnlyOnce = OnlyOnce;
          });

      var modifier = Builder
        .Modifier<AddDamagePrevention>(m => m.Prevention = prevention)
        .CreateModifier(Source.OwningCard, Targets[1], X, Game);      

      Targets[1].AddModifier(modifier);
    }
  }
}