using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPGTemplate.Context
{
    public class StatusEffect
    {
        public StatusEffectType effectType;
        public int duration;
        public float effectAmount;

        public StatusEffect(StatusEffectType type, int duration, float effectAmount)
        {
            effectType = type;
            this.duration = duration;
            this.effectAmount = effectAmount;
        }
    }
}
public enum StatusEffectType
{
    None,
    Stun,
    DoT,
    Curse,
}