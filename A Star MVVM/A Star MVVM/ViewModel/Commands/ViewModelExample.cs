using A_Star_MVVM.Model.Enums;
using A_Star_MVVM.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_MVVM.ViewModel.Commands
{
    public class ViewModelExample
    {
        public Pathfinder board { get; set; }
        public SimpleCommand simpleCommand { get; set; }

        public ViewModelExample()
        {
            board = new Pathfinder(12, 6);
            this.simpleCommand = new SimpleCommand(this);
        }

        public void ExecuteProgram()
        {
            Node[,] nodeMap = new Node[12, 6];

            for (int y = 0; y < nodeMap.GetLength(1); y++)
            {
                for (int x = 0; x < nodeMap.GetLength(0); x++)
                {
                    nodeMap[x, y] = new Node(x, y);
                }
            }

            nodeMap[2, 1].NodeState = EnumNodeState.Barrier;
            nodeMap[2, 2].NodeState = EnumNodeState.Barrier;
            nodeMap[2, 3].NodeState = EnumNodeState.Barrier;
            nodeMap[2, 4].NodeState = EnumNodeState.Barrier;

            nodeMap[5, 0].NodeState = EnumNodeState.Barrier;

            nodeMap[8, 1].NodeState = EnumNodeState.Barrier;
            nodeMap[8, 2].NodeState = EnumNodeState.Barrier;
            nodeMap[8, 3].NodeState = EnumNodeState.Barrier;
            nodeMap[8, 4].NodeState = EnumNodeState.Barrier;

            //Not needed just graphic drawing of map
            string[] map = new string[]
            {
                "+------------+",
                "|     x      |",
                "|  x     x  B|",
                "|  x     x   |",
                "|  x     x   |",
                "|A x     x   |",
                "|            |",
                "+------------+",
            };

            nodeMap[0, 4].NodeState = EnumNodeState.StartNode;
            nodeMap[11, 1].NodeState = EnumNodeState.EndNode;

            bool enableDiagonals = true;
            Pathfinder pathfinder = new Pathfinder(12, 6);
            pathfinder.Run(nodeMap, enableDiagonals);
        }
    }
}
