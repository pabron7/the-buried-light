using System;

[Serializable]
public class TraitModifier
{
    public TraitModifierType ModifierType; 
    public float ModifierAmount; 

    public TraitModifier(TraitModifierType type, float amount)
    {
        ModifierType = type;
        ModifierAmount = amount;
    }
}
