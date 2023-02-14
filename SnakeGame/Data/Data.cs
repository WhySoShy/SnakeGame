using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Models;
using System.IO;
using System.Text.Json;

namespace SnakeGame.Data
{
    public class Datas : IData
    {
        private readonly string path = @"../../../Data.json";
        public List<LeaderBoardItems> ReadFromJson()
            => JsonSerializer.Deserialize<List<LeaderBoardItems>>(File.ReadAllText(path));

        public async Task WriteToJson(List<LeaderBoardItems> list)
            => File.WriteAllText(path, JsonSerializer.Serialize(list));
        
    }
}
