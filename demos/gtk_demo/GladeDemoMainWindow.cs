/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   GladeDemoMainWindow.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core GTK# + Glade MainWindow demos.
 * Reference:   https://github.com/GtkSharp/GtkSharp
 *              https://github.com/GtkSharp/GtkSharp/blob/develop/Source/Libs/GtkSharp/Builder.cs
 *              https://developer.gnome.org/gtk3/stable/ch01s03.html
 *              https://developer.gnome.org/gtk3/stable/GtkBuilder.html
 *****************************************************************************/

namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using System.Threading;
    using Gtk;
    using Action = System.Action;

    /// <summary>
    /// Defines the Glade builder based GTK# window demo.
    /// </summary>
    public sealed class GladeDemoMainWindow : Window
    {
        /// <summary>
        /// The message label, mapping to the object with same id defined in Glade template.
        /// </summary>
        [Builder.Object]
        private Label messageLabel = null;

        /// <summary>
        /// The count button, mapping to the object with same id defined in Glade template.
        /// </summary>
        [Builder.Object]
        private Button countButton = null;

        /// <summary>
        /// The internal countButton clicked counter.
        /// </summary>
        private int counter;

        /// <summary>
        /// The internal flag to indicate the demo actions are executing.
        /// </summary>
        private int demoActionsExecutingFlag;

        /// <summary>
        /// Initializes a new instance of the <see cref="GladeDemoMainWindow"/> class.
        /// </summary>
        /// <remarks>
        /// Bind with the embedded Glade template file.
        /// </remarks>
        public GladeDemoMainWindow()
            : this(new Builder("GladeDemoMainWindow.glade"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GladeDemoMainWindow"/> class.
        /// </summary>
        /// <param name="builder">The builder instance.</param>
        /// <remarks>
        /// Bind this window instance with object defined in Glade template.
        /// </remarks>
        private GladeDemoMainWindow(Builder builder)
            : base(builder.GetObject("GladeDemoMainWindow").Handle)
        {
            // Bind fields to objects defined in Glade template and events to signals.
            builder.Autoconnect(this);

            this.countButton.Clicked += CountButton_Clicked;
            this.DeleteEvent += this.Window_DeleteEvent;
            this.FocusInEvent += this.Window_FocusInEvent;
        }

        /// <summary>
        /// The countButton clicked event handler.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void CountButton_Clicked(object sender, EventArgs eventArgs)
        {
            this.counter++;
            this.messageLabel.Text =
                $"This button has been clicked {this.counter} time(s).";
        }

        /// <summary>
        /// The window delete event handler, quit the demo application.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void Window_DeleteEvent(object sender, DeleteEventArgs eventArgs)
        {
            Application.Quit();
        }

        /// <summary>
        /// The window delete event handler, run demo actions until the window got focus.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void Window_FocusInEvent(object sender, FocusInEventArgs eventArgs)
        {
            // demo actions only executed once.
            if (Interlocked.CompareExchange(ref demoActionsExecutingFlag, 1, 0) == 1)
            {
                return;
            }

            // run demo actions in background thread.
            Thread demoThread = new Thread(new ThreadStart(this.RunDemoActions));
            demoThread.Start();
        }

        /// <summary>
        /// Run demo actions.
        /// </summary>
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
                    this.countButton.Click();
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

        /// <summary>
        /// Run demo action in main UI thread after some time lapses (specific or default 1 second).
        /// </summary>
        /// <param name="action">The demo action.</param>
        /// <param name="runAfterSeconds">The time lapses between actions.</param>
        private void RunDemoAction(Action action, int runAfterSeconds = 1)
        {
            Thread.Sleep(TimeSpan.FromSeconds(runAfterSeconds));
            Application.Invoke(
                delegate
                {
                    action();
                });
        }
    }
}
