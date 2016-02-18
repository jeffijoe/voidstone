// Voidstone
// - Voidstone
// -- MainWindow.xaml.cs
// -------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2016. All rights reserved.

using System.Windows;

using Jeffijoe.Voidstone.ViewModels;

using ReactiveUI;

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
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        public MainViewModel ViewModel { get; set; }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets or sets the view model.
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
    }
}