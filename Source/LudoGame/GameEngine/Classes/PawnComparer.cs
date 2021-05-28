using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LudoGame.GameEngine.Classes
{
    public class PawnComparer : IEqualityComparer<Pawn>
    {
        public bool Equals(Pawn a, Pawn b)
        {
            if (a.id == b.id) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Pawn obj) => HashCode.Combine(obj.id);
    }
}
