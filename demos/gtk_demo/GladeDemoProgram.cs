namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using GLib;
    using GtkApplication = Gtk.Application;

    internal static class GladeDemoProgram
    {
        public static void Main(string[] args)
        {
            GtkApplication.Init();

            GtkApplication application =
                new GtkApplication(
                    "DotNetCoreBootstrap.GtkDemo.GladeDemo",
                    ApplicationFlags.None);
            application.Register(Cancellable.Current);

            GladeDemoMainWindow window = new GladeDemoMainWindow();
            application.AddWindow(window);

            window.Show();
            GtkApplication.Run();
        }
    }
}
