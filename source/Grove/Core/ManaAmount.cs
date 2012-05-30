﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grove.Core
{
  public class ManaAmount : IEnumerable<Mana>
  {
    private readonly List<Mana> _amount = new List<Mana>(10);

    public ManaAmount(IEnumerable<Mana> amount)
    {
      _amount.AddRange(amount);
    }

    public ManaAmount(Mana mana)
    {
      _amount.Add(mana);
    }

    public ManaAmount()
    {
    }

    public ManaAmount(string str) : this(Mana.Parse(str))
    {
    }

    public int WhiteCount
    {
      get { return _amount.Count(x => x.IsSingleColor(ManaColors.White)); }
    }

    public int BlueCount
    {
      get { return _amount.Count(x => x.IsSingleColor(ManaColors.Blue)); }
    }

    public int BlackCount
    {
      get { return _amount.Count(x => x.IsSingleColor(ManaColors.Black)); }
    }

    public int RedCount
    {
      get { return _amount.Count(x => x.IsSingleColor(ManaColors.Red)); }
    }

    public int GreenCount
    {
      get { return _amount.Count(x => x.IsSingleColor(ManaColors.Green)); }
    }

    public int MultiCount
    {
      get { return _amount.Count(x => x.IsMultiColor); }
    }

    public ManaColors CardColor
    {
      get { return GetCardColor(); }
    }

    private IEnumerable<Mana> Colored
    {
      get { return _amount.Where(mana => mana.IsColored); }
    }

    public int Converted
    {
      get { return _amount.Count; }
    }

    public static ManaAmount Zero
    {
      get { return new ManaAmount(); }
    }

    public bool None
    {
      get { return _amount.Count == 0; }
    }

    public int ColorlessCount
    {
      get { return _amount.Count(mana => mana.IsColorless); }
    }

    public int MaxRank
    {
      get { return _amount.Count == 0 ? 0 : _amount.Max(x => x.Rank); }
    }

    #region IEnumerable<Mana> Members

    public IEnumerator<Mana> GetEnumerator()
    {
      return _amount.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #endregion

    public static ManaAmount Colorless(int amount)
    {
      return new ManaAmount(Enumerable.Repeat(new Mana(), amount));
    }

    public string[] GetSymbolNames()
    {
      var colorlessCount = _amount.Count(mana => mana.IsColorless);

      var names = new List<string>();

      if (colorlessCount > 0)
      {
        names.Add(colorlessCount.ToString());
      }

      names.AddRange(Colored.OrderBy(Wubrg).Select(mana => mana.Symbol));
      return names.ToArray();
    }

    public bool HasColor(ManaColors color)
    {
      return this.Any(x => x.HasColor(color));
    }

    public override string ToString()
    {
      return string.Join(",", _amount.Select(x => x.ToString()));
    }

    private static int Wubrg(Mana mana)
    {
      return mana.EnumerateColors().Aggregate(1, (sum, color) => sum + ((int) color*10));
    }

    private ManaColors GetCardColor()
    {
      if (Colored.Count() == 0)
      {
        return ManaColors.Colorless;
      }

      var cardColor = ManaColors.None;

      foreach (var mana in Colored)
      {
        cardColor = cardColor | mana.Colors;
      }

      return cardColor;
    }

    public static ManaAmount operator +(ManaAmount left, ManaAmount right)
    {
      return new ManaAmount(left.Concat(right));
    }

    public static implicit operator ManaAmount(Mana mana)
    {
      return new ManaAmount(new[] {mana});
    }

    public static implicit operator ManaAmount(string str)
    {
      return str == null ? null : new ManaAmount(str);
    }

    public static implicit operator ManaAmount(int colorless)
    {
      return Colorless(colorless);
    }
  }
}