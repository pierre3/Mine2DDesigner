using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.Models
{
    public class SaveData
    {
        public IList<Block>? ToolBoxItems { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public IList<ushort[]>? Aria{ get; set; }
    }
}
