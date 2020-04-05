using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace ARC_Itecture
{
    class ViewModel
    {
        private MainWindow _mainWindow;
        private Invoker _invoker;
        private Receiver _receiver;
        public Plan plan;

        public ViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            this._invoker = new Invoker();
            plan = new Plan();
            this._receiver = new Receiver(_mainWindow.canvas, plan);
        }

        public void AddArea()
        {
            _invoker.Command = new AreaCommand(this._receiver);
        }

        public void AddCamera()
        {
            _invoker.Command = new CameraCommand(this._receiver);
        }

        public void AddDoor()
        {
            _invoker.Command = new DoorCommand(this._receiver);
        }

        public void AddWall()
        {
            _invoker.Command = new WallCommand(this._receiver);
        }

        public void AddWindow()
        {
            _invoker.Command = new WindowCommand(this._receiver);
        }

        public void CanvasClick(Point p)
        {
            _invoker.Invoke(p);
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
        }
    }
}
