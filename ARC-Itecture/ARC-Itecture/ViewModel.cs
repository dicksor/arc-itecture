using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Shapes;

namespace ARC_Itecture
{
    public class ViewModel
    {
        public MainWindow _mainWindow { get; private set; }
        private Invoker _invoker;
        public Receiver _receiver { get; private set; }
        public Plan plan { get; private set; }

        public IDrawCommand Command => _invoker.DrawCommand;

        public ViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            this._invoker = new Invoker();
            plan = new Plan();
            this._receiver = new Receiver(this);
        }

        public void UpdateHistory(Stack<Tuple<Shape, IDrawComponent>> history)
        {
            _mainWindow.listBoxHistory.Items.Add(history.Peek().Item2.GetName());
            //object o = _mainWindow.listBoxHistory.SelectedItem;
        }

        internal void RemoveFromHistory()
        {
            
        }

        public void AddArea()
        {
            _invoker.DrawCommand = new AreaCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewAreaCommand(this._receiver);
        }

        public void AddCamera()
        {
            _invoker.DrawCommand = new CameraCommand(this._receiver);
        }

        public void AddDoor()
        {
            _invoker.DrawCommand = new DoorCommand(this._receiver);
        }

        public void AddWall()
        {
            _invoker.DrawCommand = new WallCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewWallCommand(this._receiver);
        }

        public void AddWindow()
        {
            _invoker.DrawCommand = new WindowCommand(this._receiver);
        }

        public void CanvasClick(Point p)
        {
            _invoker.InvokeClick(p);
        }

        public void CanvasMouseMove(Point p)
        {
            _invoker.InvokeMouseMove(p);
        }

        public void LoadJson(string filename)
        {
            plan = JsonConvert.DeserializeObject<Plan>(File.ReadAllText(filename));
            plan.ImportDraw(_receiver, _invoker);

            _mainWindow.textBoxDoorH2.Text = plan.DoorH2.ToString();
            _mainWindow.textBoxWallHeight.Text = plan.WallHeight.ToString();
            _mainWindow.textBoxWallWidth.Text = plan.WallWidth.ToString();
            _mainWindow.textBoxWindowH1.Text = plan.WindowH1.ToString();
            _mainWindow.textBoxWindowH2.Text = plan.WindowH2.ToString();
        }

        public void SaveJson(string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                (new JsonSerializer()).Serialize(file, plan);
            }
        }

        public void ClearCanvas()
        {
            _mainWindow.canvas.Children.Clear();
            plan = new Plan();
            _receiver = new Receiver(this);
            _invoker = new Invoker();
        }

        public void StartNewWall()
        {
            _receiver.StartNewWall();
        }
    }
}
