namespace WpfColorMixer.Mixer
{
    public partial class MixerView
    {
        public MixerView()
            : base(MixService.TestMixer())
        {
            InitializeComponent();
        }
    }
}
