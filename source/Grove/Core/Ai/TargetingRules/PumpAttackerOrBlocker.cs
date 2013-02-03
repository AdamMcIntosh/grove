﻿namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using Targeting;

  public class PumpAttackerOrBlocker : TargetingRule
  {
    private readonly int? _power;
    private readonly int? _toughness;
        
    public PumpAttackerOrBlocker(int? power, int? toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = _power ?? p.MaxX;
      var toughness = _toughness ?? p.MaxX;

      var candidates = None<Card>();

      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p);
      }

      else if (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p);
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}