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

namespace ARC_Itecture
{
    public class ViewModel
    {
        public MainWindow _mainWindow { get; private set; }
        private Invoker _invoker;
        public Receiver _receiver { get; private set; }
        public Plan plan { get; private set; }
        public Stack<Tuple<Object, Object, String>> stackHistory { get; set; }

        public IDrawCommand Command => _invoker.DrawCommand;

        public DialogClosingEventHandler ClosingEventHandler { get; private set; }

        public ViewModel(MainWindow mainWindow)
        {
            stackHistory = new Stack<Tuple<Object, Object, string>>();
            _mainWindow = mainWindow;
            this._invoker = new Invoker();
            plan = new Plan();
            this._receiver = new Receiver(this);
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
            _invoker.PreviewCommand = new PreviewDoorCommand(this._receiver);
        }

        public void AddWall()
        {
            _invoker.DrawCommand = new WallCommand(this._receiver);
            _invoker.PreviewCommand = new PreviewWallCommand(this._receiver);
        }

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

        public async Task<string> ShowAreaDialog()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Add area type name";

            ComboBox comboBox = new ComboBox();
            comboBox.Items.Add("LivingRoom");
            comboBox.Items.Add("Room");
            comboBox.Items.Add("Kitchen");
            comboBox.Items.Add("Bathroom");
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
            //MessageBox.Show(plan.segments[0].Window.Start[0].ToString());
            using (StreamWriter file = File.CreateText(filename))
            {
                (new JsonSerializer()).Serialize(file, plan);
            }
        }

        public void RemoveFromHistory()
        {
            Tuple<Object, Object, String> shapeHistory = stackHistory.Pop();

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

            plan.RemoveObject(shapeHistory.Item2);
        }

        public void ClearCanvas()
        {
            _mainWindow.canvas.Children.Clear();
            plan = new Plan();
            _receiver = new Receiver(this);
            _invoker = new Invoker();
            CameraCommand.ResetIsAlreadyUsed();
        }

        public void StartNewWall()
        {
            _receiver.StartNewWall();
        }
    }
}
