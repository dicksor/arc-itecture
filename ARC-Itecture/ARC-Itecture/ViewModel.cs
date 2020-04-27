/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Linq;

namespace ARC_Itecture
{
    public class ViewModel
    {
        public MainWindow _mainWindow { get; private set; }
        private Invoker _invoker;
        public Receiver _receiver { get; private set; }
        public Plan _plan { get; private set; }
        public Stack<Tuple<Object, Object, String>> _stackHistory { get; set; }

        public IDrawCommand Command => _invoker.DrawCommand;

        public DialogClosingEventHandler ClosingEventHandler { get; private set; }

        public ViewModel(MainWindow mainWindow)
        {
            this._stackHistory = new Stack<Tuple<Object, Object, string>>();
            this._mainWindow = mainWindow;
            this._invoker = new Invoker();
            this._plan = new Plan(this._mainWindow.gridGeometry.Bounds);
            this._receiver = new Receiver(this);
        }

        /// <summary>
        /// Add an area to the canvas
        /// </summary>
        public void AddArea()
        {
            _invoker.DrawCommand = new AreaCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewAreaCommand(this._receiver);
        }

        /// <summary>
        /// Add the camera to the canvas
        /// </summary>
        public void AddCamera()
        {
            _invoker.DrawCommand = new CameraCommand(this._receiver);
        }

        /// <summary>
        /// Add a door to the canavas
        /// </summary>
        public void AddDoor()
        {
            _invoker.DrawCommand = new DoorCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewDoorCommand(this._receiver);
        }

        /// <summary>
        /// Add a wall to the canavas
        /// </summary>
        public void AddWall()
        {
            _invoker.DrawCommand = new WallCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewWallCommand(this._receiver);
        }

        /// <summary>
        /// Add a window to the canavas
        /// </summary>
        public void AddWindow()
        {
            _invoker.DrawCommand = new WindowCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewWindowCommand(this._receiver); 
        }

        public void CanvasClick(Point p)
        {
            _invoker.InvokeClick(p);
        }

        public void CanvasMouseMove(Point p)
        {
            _invoker.InvokeMouseMove(p);
        }

        /// <summary>
        /// Allow to create the dialog the dialog for specifying the field type after creation
        /// </summary>
        /// <returns>Async string tha represent the area type name</returns>
        public async Task<string> ShowAreaDialog()
        {
            List<string> areaTypes = new List<string>() { "LivingRoom", "Room", "Kitchen", "Bathroom" };

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Add area type name";

            ComboBox comboBox = new ComboBox();
            foreach(string areaType in areaTypes)
            {
                comboBox.Items.Add(areaType);
            }
            comboBox.SelectedIndex = 0;
            comboBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            comboBox.Margin = new Thickness(0, 12, 0, 0);

            Button button = new Button();
            button.Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            button.IsDefault = true;
            button.Margin = new Thickness(0, 8, 8, 0);
            button.Command = DialogHost.CloseDialogCommand;
            button.Content = "ADD AREA";

            StackPanel stk = new StackPanel();
            stk.Margin = new Thickness(16);
            stk.Children.Add(textBlock);
            stk.Children.Add(comboBox);
            stk.Children.Add(button);
            string areaTypeName = "";
            object result = await DialogHost.Show(stk, "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
            StackPanel sp = (StackPanel)args.Session.Content;
                areaTypeName = ((ComboBox)((StackPanel)args.Session.Content).Children[1]).SelectedItem.ToString();
            });
            return areaTypeName;
        }

        /// <summary>
        /// Load a JSON plan
        /// </summary>
        /// <param name="filename">Filename of the plan to load</param>
        public void LoadJson(string filename)
        {

            _plan = JsonConvert.DeserializeObject<Plan>(File.ReadAllText(filename));
            _plan.GridRatio = _mainWindow.gridGeometry.Bounds.Width;
            _plan.ImportDraw(_receiver, _invoker);

            //Set the plan properties on the GUI
            _mainWindow.textBoxDoorH2.Text = _plan.DoorH2.ToString();
            _mainWindow.textBoxWallHeight.Text = _plan.WallHeight.ToString();
            _mainWindow.textBoxWallWidth.Text = _plan.WallWidth.ToString();
            _mainWindow.textBoxWindowH1.Text = _plan.WindowH1.ToString();
            _mainWindow.textBoxWindowH2.Text = _plan.WindowH2.ToString();

            _stackHistory.Reverse();
        }

        /// <summary>
        /// Save the plan on the computer
        /// </summary>
        /// <param name="filename">Filname of the plan to save</param>
        public void SaveJson(string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                (new JsonSerializer()).Serialize(file, _plan);
            }
        }

        /// <summary>
        /// Remove the last drawed items from the history stack
        /// </summary>
        public void RemoveFromHistory()
        {
            Tuple<Object, Object, String> shapeHistory = _stackHistory.Pop();

            int index = _mainWindow.canvas.Children.IndexOf(shapeHistory.Item1 as UIElement);
            if(shapeHistory.Item3 == "Area")
            {
                _mainWindow.canvas.Children.RemoveRange(index-1, 2);
            }
            else if(shapeHistory.Item3 == "Window")
            {
                Rectangle rect = shapeHistory.Item1 as Rectangle;
                _receiver.UpdateAvailableWindowList(rect);
                _mainWindow.canvas.Children.RemoveAt(index);
            }
            else
            {
                _mainWindow.canvas.Children.RemoveAt(index);
            }

            _plan.RemoveObject(shapeHistory.Item2);
        }

        /// <summary>
        /// Clear all elements in the canavas.
        /// Create a new plan.
        /// Reinstanciate Invoker and Receiver
        /// Clear history
        /// </summary>
        public void ClearCanvas()
        {
            _mainWindow.canvas.Children.Clear();
            _plan = new Plan(_mainWindow.gridGeometry.Bounds);
            _receiver = new Receiver(this);
            _invoker = new Invoker();
            CameraCommand.ResetIsAlreadyUsed();
            _stackHistory.Clear();
        }

        /// <summary>
        /// Tells the receiver to start drawing a wall from a new point
        /// </summary>
        public void StartNewWall()
        {
            _receiver.StartNewWall();
        }

        /// <summary>
        /// Tells the receiver to cancel the last shape preview
        /// </summary>
        public void RemoveLastPreview()
        {
            _receiver.RemoveLastPreview();
        }
    }
}
