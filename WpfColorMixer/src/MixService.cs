namespace WpfColorMixer
{
    using JetBrains.Annotations;
    using Mixer;
    using NetLib;

    public static class MixService
    {
        public static MixerVM TestMixer()
        {
            var setsData = LoadMixSetData();
            return new MixerVM(setsData.Palettes[0]);
        }

        [NotNull]
        private static MixSetData LoadMixSetData()
        {
            return "MixSetData.json".Deserialize<MixSetData>();
        }
    }
}