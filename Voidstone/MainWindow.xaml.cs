// Voidstone
// - Voidstone
// -- MainWindow.xaml.cs
// -------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2016. All rights reserved.

using System;
using System.Windows;
using System.Windows.Forms;

using Jeffijoe.Voidstone.ViewModels;

using ReactiveUI;

using MessageBox = System.Windows.MessageBox;

namespace Jeffijoe.Voidstone
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel = new MainViewModel();
            this.WhenAnyObservable(x => x.ViewModel.Start.IsExecuting)
                .BindTo(this, x => x.WorkingOnItLabel.Visibility);

            var startObs = this.WhenAnyObservable(x => x.ViewModel.Start);
            startObs
                .Subscribe(
                    x => {
                        string message = x ? "It worked!" : "Didn't work, sorry. Check your filepaths.";
                        string caption = x ? "Yep!" : "Nope";
                        MessageBox.Show(message, caption, MessageBoxButton.OK);
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the view model.
        /// </summary>
        public MainViewModel ViewModel { get; set; }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        ///     Gets or sets the view model.
        /// </summary>
        object IViewFor.ViewModel
        {
            get
            {
                return this.ViewModel;
            }

            set
            {
                this.ViewModel = (MainViewModel)value;
            }
        }

        #endregion

        /// <summary>
        /// Browses the input dir click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BrowseInputDirClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ViewModel.InputDirectory = dlg.SelectedPath;
            }
        }

        /// <summary>
        /// Browses the output file click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BrowseOutputFileClick(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "mp4";
            dlg.CheckPathExists = true;
            dlg.Filter = "MP4 files (.mp4)|*.mp4|MKV files (.mkv)|*.mkv";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ViewModel.OutputFile = dlg.FileName;
            }
        }
    }
}