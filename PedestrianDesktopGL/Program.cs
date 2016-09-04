using Pedestrian;
using System;

namespace DesktopGL
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new PedestrianGame())
                game.Run();
        }
    }
}
