namespace WpfColorMixer
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Набор цветов (с кодами), напрмер - Rosehill TPV
    /// </summary>
    public class MixPalette
    {
        public string Name { get; set; }
        
        [NotNull]
        public List<MixComponent> Components { get; set; } = new List<MixComponent>();
    }
}