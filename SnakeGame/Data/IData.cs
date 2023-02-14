using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Models;

namespace SnakeGame.Data
{
    public interface IData
    {
        List<LeaderBoardItems> ReadFromJson();
        Task WriteToJson(List<LeaderBoardItems> list);
    }
}
