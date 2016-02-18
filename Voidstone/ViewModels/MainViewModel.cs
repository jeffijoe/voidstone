// Voidstone
// - Voidstone
// -- MainViewModel.cs
// -------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2016. All rights reserved.

using System;
using System.Reactive.Linq;
using System.Windows;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Jeffijoe.Voidstone.ViewModels
{
    /// <summary>
    ///     Main view model.
    /// </summary>
    public class MainViewModel : ReactiveObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel()
        {
            this.FramerateString = "24";
            this.InputDirectory = @"C:\Users\Jeff\Desktop\anim";
            this.InputFilePattern = @"introcupcakeanimation%01d.jpg";
            this.OutputFile = @"C:\Users\Jeff\Desktop\anim\a.mp4";
            var obs = this.WhenAny(
                x => x.FramerateString, 
                x => {
                    int number;
                    if (int.TryParse(x.Value, out number))
                    {
                        return number;
                    }

                    return 0;
                });

            obs.ToPropertyEx(this, x => x.Framerate);
            obs
                .Select(x => x.ToString())
                .Where(x => x != this.FramerateString)
                .ObserveOn(Application.Current.Dispatcher)
                .Subscribe(
                    x => {
                        this.FramerateString = x;
                    });

            this.CanStart = this.WhenAny(
                x => x.Framerate, 
                x => x.InputDirectory, 
                x => x.InputFilePattern, 
                x => x.OutputFile, 
                (fr, id, ifp, of) => {
                    if (fr.Value == 0)
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(id.Value))
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(ifp.Value))
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(of.Value))
                    {
                        return false;
                    }

                    return true;
                });

            this.Start = ReactiveCommand.CreateAsyncTask(
                this.CanStart, 
                async x => {
                    var options = new FFMPEGOptions
                    {
                        Framerate = this.Framerate, 
                        InputDirectory = this.InputDirectory, 
                        OutputFile = this.OutputFile, 
                        InputFilePattern = this.InputFilePattern
                    };
                    return await FFMPEG.StartAsync(options);
                });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the can start.
        /// </summary>
        /// <value>
        ///     The can start.
        /// </value>
        public IObservable<bool> CanStart { get; private set; }

        /// <summary>
        ///     Gets the framerate.
        /// </summary>
        /// <value>
        ///     The framerate.
        /// </value>
        [ObservableAsProperty]
        public extern int Framerate { get; }

        /// <summary>
        ///     Gets or sets the framerate string.
        /// </summary>
        /// <value>
        ///     The framerate string.
        /// </value>
        [Reactive]
        public string FramerateString { get; set; }

        /// <summary>
        ///     Gets or sets the input directory.
        /// </summary>
        /// <value>
        ///     The input directory.
        /// </value>
        [Reactive]
        public string InputDirectory { get; set; }

        /// <summary>
        ///     Gets or sets the input file pattern.
        /// </summary>
        /// <value>
        ///     The input file pattern.
        /// </value>
        [Reactive]
        public string InputFilePattern { get; set; }

        /// <summary>
        ///     Gets the is busy.
        /// </summary>
        /// <value>
        ///     The is busy.
        /// </value>
        public IObservable<bool> IsBusy { get; private set; }

        /// <summary>
        ///     Gets or sets the output file.
        /// </summary>
        /// <value>
        ///     The output file.
        /// </value>
        [Reactive]
        public string OutputFile { get; set; }

        /// <summary>
        ///     Gets the start.
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public ReactiveCommand<bool> Start { get; private set; }

        #endregion
    }
}