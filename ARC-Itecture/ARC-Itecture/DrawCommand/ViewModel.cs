using ARC_Itecture.DrawCommand.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ARC_Itecture.DrawCommand
{
    class ViewModel
    {
        private MainWindow _mainWindow;
        private Invoker _invoker;
        private Receiver _receiver;
        private Plan _plan;

        public ViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            this._invoker = new Invoker();
            this._receiver = new Receiver(_mainWindow.canvas);
        }

        public void AddArea()
        {
            this._invoker.Command = new AreaCommand(this._receiver);
        }

        public void AddCamera()
        {
            this._invoker.Command = new CameraCommand(this._receiver);
        }

        public void AddDoor()
        {
            this._invoker.Command = new DoorCommand(this._receiver);
        }

        public void AddWall()
        {
            this._invoker.Command = new WallCommand(this._receiver);
        }

        public void AddWindow()
        {
            this._invoker.Command = new WindowCommand(this._receiver);
        }

        public void CanvasClick(Point p)
        {
            this._invoker.Invoke(p);
        }

        public void LoadJson(string filename)
        {
            _plan = JsonConvert.DeserializeObject<Plan>(File.ReadAllText(filename));
        }

        public void SaveJson(string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                (new JsonSerializer()).Serialize(file, _plan);
            }
        }
    }
}
