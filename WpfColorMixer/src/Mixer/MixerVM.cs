namespace WpfColorMixer.Mixer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using DynamicData;
    using JetBrains.Annotations;
    using NetLib.WPF;
    using ReactiveUI;

    /// <summary>
    /// Замешивание цветов
    /// </summary>
    public class MixerVM : BaseViewModel
    {
        public MixerVM([NotNull] MixPalette palette, List<MixtureItemVM> mixture = null)
        {
            var percentSubject = new Subject<double>();
            OK = CreateCommand(() => DialogResult = true);
            PaletteName = palette.Name;
            Components = new ObservableCollection<MixComponent>(palette.Components);

            var percentObs = percentSubject.Delay(TimeSpan.FromMilliseconds(300))
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Select(_ => Unit.Default);
            percentObs.Subscribe(s => CalcPercent());

            AddComponemt = CreateCommand<MixComponent>(c =>
            {
                if (Mixture.Any(m => m.Component == c))
                    return;
                Components.Remove(c);
                var item = new MixtureItemVM { Component = c };
                item.WhenAnyValue(v => v.Percent).Skip(1).Subscribe(s => 
                    percentSubject.OnNext(s));
                Mixture.Add(item);
                CalcPercent();
            });
            DeleteComponent = CreateCommand<MixtureItemVM>(c =>
            {
                Mixture.Remove(c);
                Components.Add(c.Component);
                CalcPercent();
            });

            if (mixture?.Any() == true)
            {
                mixture.ToObservable().InvokeCommand(AddComponemt);
            }
        }

        public ReactiveCommand OK { get; set; }
        
        /// <summary>
        /// Палитра цветов
        /// </summary>
        public ObservableCollection<MixComponent> Components { get; set; }
        
        public string PaletteName { get; set; }

        public ObservableCollection<MixtureItemVM> Mixture { get; set; } = new ObservableCollection<MixtureItemVM>();

        public ReactiveCommand AddComponemt { get; set; }

        public ReactiveCommand DeleteComponent { get; set; }

        public int Total { get; set; }

        private void CalcPercent()
        {
            var locals = 0;
            var lockedsPercent = 0;
            foreach (var item in Mixture)
            {
                if (item.IsLocked)
                    lockedsPercent += item.Percent;
                else
                    locals += item.Percent;
            }

            if (locals <= 0)
            {
                Total = Mixture.Sum(s => s.Percent);
                return;
            }

            var cost = (100 - lockedsPercent) / (double) locals;

            foreach (var item in Mixture.Where(w => !w.IsLocked))
                item.Percent = Convert.ToInt32(item.Percent * cost);

            Total = Mixture.Sum(s => s.Percent);

            // Если всего не 100%, то поправить
            if (Total != 100)
            {
                var delta = Total - 100;
                var fixMixture = delta > 0
                    ? Mixture.Reverse().FirstOrDefault(w => !w.IsLocked && w.Percent > delta)
                    : Mixture.Reverse().FirstOrDefault(w => !w.IsLocked && w.Percent - delta < 100);

                if (fixMixture != null)
                    fixMixture.Percent = fixMixture.Percent - delta;

                Total = Mixture.Sum(s => s.Percent);
            }
        }
    }
}