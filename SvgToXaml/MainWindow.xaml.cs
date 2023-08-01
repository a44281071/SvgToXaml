using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using SvgToXaml.Properties;
using SvgToXaml.ViewModels;

namespace SvgToXaml
{
    //todo: github oder codeplex anlegen
    //todo: Fehlerbehandlung beim Laden

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new SvgImagesViewModel();

            DataContext = vm;
            vm.CurrentDir = Settings.Default.LastDir;
        }

        private readonly BrushConverter brushConverter = new();
        private string filterText = "";

        protected override void OnClosing(CancelEventArgs e)
        {
            //Save current Dir for next Start
            var vm = (SvgImagesViewModel)DataContext;
            Settings.Default.LastDir = vm.CurrentDir;
            Settings.Default.Save();

            base.OnClosing(e);
        }

        private void MainWindow_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        ((SvgImagesViewModel)DataContext).CurrentDir = path;
                    }
                    else
                    {
                        if (File.Exists(path))
                        {
                            ImageBaseViewModel.OpenDetailWindow(new SvgImageViewModel(path));
                        }
                    }
                }
            }
        }

        private void filterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            filterText = filterTextBox.Text?.Trim();
            var cvs = (CollectionViewSource)this.FindResource("images_collection");
            cvs.View?.Refresh();
        }

        private void CollectionViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            if (e.Item is not ImageBaseViewModel img) return;

            string txt = filterText;
            if (String.IsNullOrEmpty(txt)) e.Accepted = true;
            else
            {
                e.Accepted = img.Filename.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1;
            }
        }
    }
}