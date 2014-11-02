﻿namespace Grove.Modifiers
{
  using System;
  using System.Linq;

  public class DisableAbilities : Modifier, ICardModifier
  {
    private ActivatedAbilities _activatedAbilities;
    private SimpleAbilities _simpleAbilties;
    private TriggeredAbilities _triggeredAbilities;

    private readonly Func<ActivatedAbility, bool> _activated;
    private readonly Func<Static, bool> _simple;
    private readonly Func<TriggeredAbility, bool> _triggered;

    private DisableAbilities() { }

    public DisableAbilities(bool activated = false, bool simple = false, bool triggered = false)
      : this(a => activated, s => simple, t => triggered) {}

    public DisableAbilities(Func<ActivatedAbility, bool> activated, Func<Static, bool> simple, Func<TriggeredAbility, bool> triggered)
    {
      _activated = activated;
      _simple = simple;
      _triggered = triggered;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _activatedAbilities = new ActivatedAbilities(abilities.GetFiltered(_activated));
      _activatedAbilities.Initialize(OwningCard, Game);
      _activatedAbilities.DisableAll();
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _simpleAbilties = new SimpleAbilities(abilities.GetFiltered(_simple));
      _simpleAbilties.Initialize(OwningCard, Game);
      _simpleAbilties.DisableAll();
    }

    public override void Apply(TriggeredAbilities abilities)
    {
        _triggeredAbilities = new TriggeredAbilities(abilities.GetFiltered(_triggered));
        _triggeredAbilities.Initialize(OwningCard, Game);
        _triggeredAbilities.DisableAll();
    }

    protected override void Unapply()
    {
      _activatedAbilities.EnableAll();
      _simpleAbilties.Enable();
      _triggeredAbilities.EnableAll();
    }
  }
}