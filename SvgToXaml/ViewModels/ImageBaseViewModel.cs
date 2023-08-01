using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SvgConverter;
using SvgToXaml.Command;

namespace SvgToXaml.ViewModels
{
    public abstract class ImageBaseViewModel : ViewModelBase
    {
        protected ImageBaseViewModel(string filepath)
        {
            Filepath = filepath;
            OpenDetailCommand = new DelegateCommand(OpenDetailExecute);
            OpenFileCommand = new DelegateCommand(OpenFileExecute);
            OpenFileLocationCommand = new DelegateCommand(OpenFileLocationExecute);
        }

        public string Filepath { get; }
        public string Filename => Path.GetFileName(Filepath);
        public ImageSource PreviewSource => GetImageSource();

        public ICommand OpenDetailCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand OpenFileLocationCommand { get; set; }

        protected abstract ImageSource GetImageSource();
        public abstract bool HasXaml { get; }
        public abstract bool HasSvg { get; }
        public string SvgDesignInfo => GetSvgDesignInfo();

        private void OpenDetailExecute()
        {
            OpenDetailWindow(this);
        }

        public static void OpenDetailWindow(ImageBaseViewModel imageBaseViewModel)
        {
            new DetailWindow { DataContext = imageBaseViewModel }.Show();
        }

        private void OpenFileExecute()
        {
            Process.Start(Filepath);
        }

        private void OpenFileLocationExecute()
        {
            Process.Start("explorer.exe", string.Format($"""/select, "{Filepath}" """ ));
        }

        protected abstract string GetSvgDesignInfo();
    }
}