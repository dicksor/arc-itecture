/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.DrawCommand.Commands;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARC_Itecture
{
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;
        private bool _isDrawing;
        private Rect _canvasRect;
        private SnackbarMessageQueue _snackbarMessageQueue;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ViewModel(this);
            _isDrawing = false;

            _snackbarMessageQueue = new SnackbarMessageQueue();
            Snackbar.MessageQueue = _snackbarMessageQueue;

            DataContext = _viewModel._plan;

            KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            main = this;
        }

        /// <summary>
        /// Change the color of the seleted tool
        /// </summary>
        /// <param name="button">Button to change the tool</param>
        private void ColorCommand(Button button)
        {
            Style style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddArea.Style = style;
            buttonAddCamera.Style = style;
            buttonAddDoor.Style = style;
            buttonAddWindow.Style = style;
            buttonAddWall.Style = style;

            button.Style = FindResource("MaterialDesignFloatingActionLightButton") as Style;

            _isDrawing = false;
            _viewModel.RemoveLastPreview();
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
            _viewModel.AddWindow();
        }

        private void ButtonAddWall_Click(object sender, EventArgs e)
        {
            ColorCommand((Button)sender);
            _viewModel.AddWall();
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this.canvas);
            Rect _canvasRect = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);

            if (_canvasRect.Contains(p))
            {
                if (!_isDrawing)
                {
                    _isDrawing = true;
                    Mouse.Capture(canvas);
                }
                else
                {
                    Mouse.Capture(null);
                }

                _viewModel.CanvasClick(p);
            }
            else
            {
                Mouse.Capture(null);
                _viewModel.StartNewWall(); 
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawing && !(_viewModel.Command is WallCommand))
            {
                _isDrawing = false;

                Point p = Mouse.GetPosition(this.canvas);
                _viewModel.CanvasClick(p);

                Mouse.Capture(null);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            _canvasRect = new Rect(0,0,canvas.ActualWidth,canvas.ActualHeight);
            if(_isDrawing)
            {
                Point p = Mouse.GetPosition(this.canvas);
                if(_canvasRect.Contains(p))
                {
                    _viewModel.CanvasMouseMove(p);
                }
                else
                {
                    _viewModel.StartNewWall();
                    Debug.WriteLine("new wall");
                }
            }
        }

        private void ButtonClearPlan_Click(object sender, RoutedEventArgs e)
        {
            ClearPlan();
        }

        /// <summary>
        /// Create a new plan
        /// </summary>
        private void ClearPlan()
        {
            _viewModel.ClearCanvas();
            listBoxHistory.Items.Clear();

            buttonAddArea.Style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddCamera.Style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddDoor.Style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddWindow.Style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;
            buttonAddWall.Style = FindResource("MaterialDesignFloatingActionDarkButton") as Style;

            _snackbarMessageQueue.Enqueue("New plan");
        }

        private void ButtonSavePlan_Click(object sender, RoutedEventArgs e)
        {
            SaveDialog();
        }

        /// <summary>
        /// Allow to save a plan with a dialog
        /// </summary>
        private void SaveDialog()
        {
            SaveFileDialog dlg = new SaveFileDialog { FileName = "Plan", DefaultExt = ".json", Filter = "JSON file (.json)|*.json" };

            if (dlg.ShowDialog() == true)
            {
                _viewModel.SaveJson(dlg.FileName);
                _snackbarMessageQueue.Enqueue("Plan saved");
            }
        }

        private void ButtonLoadPlan_Click(object sender, RoutedEventArgs e)
        {
            LoadDialog();
        }

        /// <summary>
        /// Allow to load a plan with a dialog
        /// </summary>
        private void LoadDialog()
        {
            if (canvas.Children.Count == 0)
            {


                    OpenFileDialog dlg = new OpenFileDialog { DefaultExt = ".json", Filter = "JSON file (.json)|*.json" };

                    if (dlg.ShowDialog() == true)
                    {
                        if (Path.GetExtension(dlg.FileName) == ".json")
                        {
                            _viewModel.LoadJson(dlg.FileName);
                            _snackbarMessageQueue.Enqueue("Plan opened");
                        }
                        else
                        {
                            MessageBox.Show("The file must have the .sjson extension !");
                        }
                    }
             }
            else
            {
                MessageBox.Show("The canvas must be empty to load a plan");
            }
}

        /// <summary>
        /// Allow to select the cuurent command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                _viewModel.AddArea();
                ColorCommand(FindName("buttonAddArea") as Button);
            }
            else if (e.Key == Key.C)
            {
                _viewModel.AddCamera();
                ColorCommand(FindName("buttonAddCamera") as Button);
            }
            else if (e.Key == Key.D)
            {
                _viewModel.AddDoor();
                ColorCommand(FindName("buttonAddDoor") as Button);
            }
            else if (e.Key == Key.F)
            {
                _viewModel.AddWindow();
                ColorCommand(FindName("buttonAddWindow") as Button);
            }
            else if (e.Key == Key.W)
            {
                _viewModel.AddWall();
                ColorCommand(FindName("buttonAddWall") as Button);
            }
            else if (e.Key == Key.Escape)
            {
                _viewModel.StartNewWall();
                _snackbarMessageQueue.Enqueue("Will start drawing from new point");
            }
            else if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                CleanHistory();
            }
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveDialog();
            }
            else if (e.Key == Key.O && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                LoadDialog();
            }
            else if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ClearPlan();
            }
        }

        private void buttonRemoveLastHistory_Click(object sender, RoutedEventArgs e)
        {
            CleanHistory();
        }

        /// <summary>
        /// Clean history
        /// </summary>
        private void CleanHistory()
        {
            if(listBoxHistory.Items.Count > 0)
            {
                listBoxHistory.Items.RemoveAt(0);
                _viewModel.RemoveFromHistory();
                _snackbarMessageQueue.Enqueue("Remove last trick!");
            }
            else
            {
                _snackbarMessageQueue.Enqueue("History is empty!");
            }
        }

        internal static MainWindow main;
        internal String History
        {
            set { listBoxHistory.Items.Insert(0, value); }
        }
    }
}
