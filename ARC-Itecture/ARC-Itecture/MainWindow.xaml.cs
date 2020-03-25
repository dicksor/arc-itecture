using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace ARC_Itecture
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Invoker _invoker;
        private Receiver _receiver;

        public MainWindow()
        {
            InitializeComponent();
            this._invoker = new Invoker();
            this._receiver = new Receiver(this.canvas);
        }

        private void ButtonAddArea_Click(object sender, EventArgs e)
        {
            this._invoker.Command = new AreaCommand(this._receiver);
        }
        
        private void ButtonAddCamera_Click(object sender, EventArgs e)
        {
            this._invoker.Command = new CameraCommand(this._receiver);
        }

        private void ButtonAddDoor_Click(object sender, EventArgs e)
        {
            this._invoker.Command = new DoorCommand(this._receiver);
        }

        private void ButtonAddWall_Click(object sender, EventArgs e)
        {
            this._invoker.Command = new WallCommand(this._receiver);
        }

        private void ButtonAddWindow_Click(object sender, EventArgs e)
        {
            this._invoker.Command = new WindowCommand(this._receiver);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this.canvas);
            this._invoker.Invoke(p);
        }
    }
}
