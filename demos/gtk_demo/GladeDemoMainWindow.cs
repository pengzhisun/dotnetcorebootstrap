namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using System.Threading;
    using Gtk;
    using Action = System.Action;

    public sealed class GladeDemoMainWindow : Window
    {
        [Builder.Object]
        private Label messageLabel = null;

        [Builder.Object]
        private Button countbutton = null;

        private int counter;

        public GladeDemoMainWindow()
            : this(new Builder("GladeDemoMainWindow.glade"))
        {
        }

        private GladeDemoMainWindow(Builder builder)
            : base(builder.GetObject("GladeDemoMainWindow").Handle)
        {
            builder.Autoconnect(this);

            this.DeleteEvent += this.Window_DeleteEvent;
            this.FocusInEvent += this.Window_FocusInEvent;
            this.countbutton.Clicked += CountButton_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs eventArgs)
        {
            Application.Quit();
        }

        private void Window_FocusInEvent(object sender, FocusInEventArgs eventArgs)
        {
            Thread demoThread = new Thread(new ThreadStart(this.RunDemoActions));
            demoThread.Start();
        }

        private void RunDemoActions()
        {
            const int ClickTimes = 2;
            RunDemoAction(() =>
            {
                this.messageLabel.Text =
                    $"Auto click count button {ClickTimes} times after 1 second";
            });

            for (int i = 0; i < ClickTimes; ++i)
            {
                RunDemoAction(() =>
                {
                    this.countbutton.Click();
                });
            }

            RunDemoAction(() =>
            {
                this.messageLabel.Text =
                    $"Auto close this form after 2 seconds";
            });

            RunDemoAction(() =>
            {
                this.Close();
            },
            runAfterSeconds: 2);
        }

        private void RunDemoAction(Action action, int runAfterSeconds = 1)
        {
            Thread.Sleep(TimeSpan.FromSeconds(runAfterSeconds));
            Application.Invoke(
                delegate
                {
                    action();
                });
        }

        private void CountButton_Clicked(object sender, EventArgs eventArgs)
        {
            this.counter++;
            this.messageLabel.Text =
                $"This button has been clicked {this.counter} time(s).";
        }
    }
}
