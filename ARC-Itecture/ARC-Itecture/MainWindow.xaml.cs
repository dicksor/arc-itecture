using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ARC_Itecture
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ViewModel(this);

            DataContext = _viewModel.plan;
        }

        private void ColorCommand(Button button)
        {
            Style style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddArea.Style = style;
            buttonAddCamera.Style = style;
            buttonAddDoor.Style = style;
            buttonAddWindow.Style = style;

            button.Style = FindResource("MaterialDesignFloatingActionLightButton") as Style;
        }

        private void ButtonAddArea_Click(object sender, EventArgs e)
        {
            ColorCommand((Button)sender);
            _viewModel.AddArea();
        }
        
        private void ButtonAddCamera_Click(object sender, EventArgs e)
        {
            ColorCommand((Button)sender);
            _viewModel.AddCamera();
        }

        private void ButtonAddDoor_Click(object sender, EventArgs e)
        {
            ColorCommand((Button)sender);
            _viewModel.AddDoor();
        }

        private void ButtonAddWindow_Click(object sender, EventArgs e)
        {
            ColorCommand((Button)sender);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this.canvas);
            _viewModel.CanvasClick(p);
        }

        private void buttonCreatePlan_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ClearCanvas();
        }

        private void buttonSavePlan_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog { FileName = "Plan", DefaultExt = ".json", Filter = "JSON file (.json)|*.json" };

            if(dlg.ShowDialog() == true)
            {
                _viewModel.SaveJson(dlg.FileName);
            }
        }

        private void buttonLoadPlan_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog{DefaultExt = ".json", Filter = "JSON file (.json)|*.json" };

            if (dlg.ShowDialog() == true)
            {
                if (Path.GetExtension(dlg.FileName) == ".json")
                {
                    _viewModel.LoadJson(dlg.FileName);
                }
                else
                {
                    MessageBox.Show("The file must have the .sjson extension !");
                }
            }
        }
    }
}
