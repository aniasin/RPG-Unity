using System.Collections.Generic;

namespace RPG.Statistics
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdittiveModifiers(Stats stat);
        IEnumerable<float> GetMultiplierModifiers(Stats stat);
    }
}
