/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   GladeDemoProgram.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core GTK# + Glade demos.
 * Reference:   https://github.com/GtkSharp/GtkSharp
 *              https://github.com/GtkSharp/GtkSharp/blob/master/Source/Templates/GtkSharp.Template.CSharp/content/GtkSharp.Application.CSharp/Program.cs
 *              https://github.com/GtkSharp/GtkSharp/blob/develop/Source/Libs/GtkSharp/Application.cs
 *              https://developer.gnome.org/gtk3/stable/GtkApplication.html
 *              https://developer.gnome.org/gtk3/stable/GtkWindow.html
 *              https://developer.gnome.org/gtk3/stable/GtkWidget.html
 *              https://developer.gnome.org/gio/stable/GApplication.html
 *              https://developer.gnome.org/gio/stable/GCancellable.html
 *              https://wiki.gnome.org/HowDoI/GtkApplication
 *              https://developer.gnome.org/gtk3/stable/gtk3-General.html
 *****************************************************************************/

namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using GLib;
    using GtkApplication = Gtk.Application;

    /// <summary>
    /// Defines the GTK# + Glade demo application.
    /// </summary>
    internal static class GladeDemoProgram
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The application command line arguments.</param>
        public static void Main(string[] args)
        {
            // call this function before using any other GTK+ functions
            GtkApplication.Init();

            // create primary instance
            GtkApplication application =
                new GtkApplication(
                    "DotNetCoreBootstrap.GtkDemo.GladeDemo",
                    ApplicationFlags.None);
            application.Register(Cancellable.Current);

            // create demo window and add to application
            GladeDemoMainWindow window = new GladeDemoMainWindow();
            application.AddWindow(window);

            // flags the demo window to be displayed.
            window.Show();

            // run application and aquire main thread.
            GtkApplication.Run();
        }
    }
}
