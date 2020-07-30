using UnityEngine;
using System.Collections.Generic;

public class MatchInfo
{
    public int matchStart;
    public int matchEnd;

    private List<Item> _match = null;
    public List<Item> match
    {
        get { return _match; }
        set { _match = value; }
    }

    public bool valid
    {
        get { return _match != null && _match.Count >= 3; }
    }

    public MatchInfo(List<Item> match)
    {
        _match = match;
    }
}
