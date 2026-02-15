namespace ChloePrime.Godot.RPG;

public enum Dichotomy {
    /// <summary>
    /// Positive, usually gives benefit to the holder.
    /// (e.g. Max Health, Damage)
    /// </summary>
    Positive = 1,
    
    /// <summary>
    /// Neutral.
    /// </summary>
    Neutral = 0,
    
    /// <summary>
    /// Negative, usually harmful to the holder.
    /// (e.g. scale of damage received)
    /// </summary>
    Negative = 2,
    
    /// <summary>
    /// Double-sided, has both positive and negative effect.
    /// </summary>
    Paradoxical = 3,
    
    /// <summary>
    /// Technical, those who does not affect the holder's in-game battle performance,
    /// but may be useful to be considered suitable to use the RPG system.
    /// (e.g. Render resolution scale, FSR sharpness).
    /// </summary>
    Technical = 4,
}

public static class DichotomyExt {
    public static bool HasPositiveEffect(this Dichotomy dichotomy) {
        return dichotomy is Dichotomy.Positive or Dichotomy.Paradoxical;
    }
    
    public static bool HasNegativeEffect(this Dichotomy dichotomy) {
        return dichotomy is Dichotomy.Negative or Dichotomy.Paradoxical;
    }
}