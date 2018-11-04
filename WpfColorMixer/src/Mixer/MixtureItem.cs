namespace WpfColorMixer.Mixer
{
    using NetLib.WPF;

    public class MixtureItemVM : BaseModel
    {
        public MixComponent Component { get; set; }

        public int Percent { get; set; }

        public bool IsLocked { get; set; }
    }
}