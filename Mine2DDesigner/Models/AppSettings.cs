using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mine2DDesigner.Models
{
    public class AppSettings
    {
        public RconSettings Rcon { get; set; } = new RconSettings();
    }

    public class RconSettings
    {
        public string Server { get; set; } = "localhost";
        public ushort Port { get; set; } = 25575;
        public string Password { get; set; } = "minecraft";
    }
}
