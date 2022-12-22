using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using static AAT_TextExtractor_GUI.Storage.ProcessMode;
using static AAT_TextExtractor_GUI.Storage.DirectoryPath;
using static AAT_TextExtractor_GUI.Storage.ExtractOptions;
using static AAT_TextExtractor_GUI.Process;
namespace AAT_TextExtractor_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
        }

        private void StartProcessButton_OnClick(object sender, RoutedEventArgs e)
        {
            singleFileMode = SingleFileButton.IsChecked == true;
            extractMode = ExtractionButton.IsChecked == true;
            includeWait = IncludeWait.IsChecked == true;
            includeNewLine = IncludeNewLine.IsChecked == true;
            includeSetTextColor = IncludeSetTextColor.IsChecked == true;
            includeNewTextBox = IncludeNewTextBox.IsChecked == true;
            if (!GetDirectory("Original", out originalPath))
            {
                return;
            }
            if (extractMode == false)
            {
                if (!GetDirectory("Translated", out translatedPath))
                {
                    return;
                }
            }
            if (!GetDirectory("Output", out outputPath))
            {
                return;
            }
            Initialization();
        }
        
        private bool GetDirectory(string operation, out string path)
        {
            path = String.Empty;
            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            SaveFileDialog saveDialog = new SaveFileDialog();
            string initialDirectory = recentDirectory ?? Directory.GetCurrentDirectory();
            openDialog.InitialDirectory = initialDirectory;
            saveDialog.InitialDirectory = initialDirectory;
            bool isOpen = false;
            string title = "Test";
            bool folderPicker = false;
            CommonFileDialogFilter openFilter = new CommonFileDialogFilter();
            string saveFilter = String.Empty;
            string defaultFileName = String.Empty;
            string defaultExtension = String.Empty;
            bool forceExtension = true;
            switch (operation)
            {
                case "Original":
                    isOpen = true;
                    title = "Select directory for your original file";
                    openFilter = new CommonFileDialogFilter("Text Document", "txt");
                    folderPicker = singleFileMode == false;
                    break;
                case "Translated":
                    isOpen = true;
                    title = "Select directory for your translated file";
                    folderPicker = singleFileMode == false;
                    openFilter = new CommonFileDialogFilter("Text Document", "txt");
                    break;
                case "Output":
                    isOpen = true;
                    title = "Select your output directory";
                    openFilter = new CommonFileDialogFilter("Text Document", "txt");
                    folderPicker = true;
                    break;
            }

            if (isOpen)
            {
                openDialog.IsFolderPicker = folderPicker;
                openDialog.Title = title;
                if (!openDialog.IsFolderPicker)
                {
                    openDialog.Filters.Add(openFilter);
                }
                if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    path = openDialog.FileName;
                    recentDirectory = openDialog.FileName;
                    return true;
                }

                return false;
            }
            else
            {
                saveDialog.OverwritePrompt = true;
                if (!String.IsNullOrEmpty(saveFilter))
                {
                    saveDialog.Filter = saveFilter;
                }
                saveDialog.FileName = defaultFileName;
                saveDialog.DefaultExt = defaultExtension;
                saveDialog.AddExtension = forceExtension;
                if (saveDialog.ShowDialog() == true)
                {
                    path = saveDialog.FileName;
                    recentDirectory = saveDialog.FileName;
                    return true;
                }

                return false;

            }
        }
    }
}