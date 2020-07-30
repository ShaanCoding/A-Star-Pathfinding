using A_Star_MVVM.Model.Enums;
using A_Star_MVVM.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_MVVM.Model.Structures
{
    public class Pathfinder : INotifyPropertyChanged
    {
        /*
        * Credits to https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
        * For showing pseudocode
        */

        public const int SLEEP_TIME = 10;
        private ObservableCollection<ObservableCollection<Node>> _nodes;

        public Pathfinder(int width, int height)
        {
            Nodes = new ObservableCollection<ObservableCollection<Node>>();

            for (int i = 0; i < height; i++)
            {
                ObservableCollection<Node> Col = new ObservableCollection<Node>();
                for (int j = 0; j < width; j++)
                {
                    Node c = new Node(i, j);
                    Col.Add(c);
                }
                Nodes.Add(Col);
            }
        }

        public ObservableCollection<ObservableCollection<Node>> Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                _nodes = value;
                OnPropertyChanged(nameof(Nodes));
            }
        }

        #region PerformAStar

        public void Run(Node[,] nodeMap, bool enableDiagonals)
        {
            Node startNode = new Node(0, 0, EnumNodeState.StartNode);
            Node endNode = new Node(0, 0, EnumNodeState.EndNode);

            for (int y = 0; y < nodeMap.GetLength(1); y++)
            {
                for (int x = 0; x < nodeMap.GetLength(0); x++)
                {
                    if (nodeMap[x, y].NodeState != EnumNodeState.Barrier)
                    {
                        if (nodeMap[x, y].NodeState == EnumNodeState.StartNode)
                        {
                            startNode = new Node(x, y, EnumNodeState.StartNode);
                            Nodes[x][y].NodeState = EnumNodeState.StartNode;
                        }
                        else if (nodeMap[x, y].NodeState == EnumNodeState.EndNode)
                        {
                            endNode = new Node(x, y, EnumNodeState.EndNode);
                            Nodes[x][y].NodeState = EnumNodeState.EndNode;
                        }
                    }
                    else
                    {
                        Nodes[x][y].NodeState = EnumNodeState.Barrier;
                    }
                }
            }

            //Put start on node on openList (set f at zero)
            startNode.FCost = 0;
            startNode.GScore = 0;
            startNode.HScore = 0;

            //Initalisation
            Node currentNode = null;
            var openList = new List<Node>();
            var closedList = new List<Node>();
            double gScore = 0;

            // start by adding the original position to the open list  
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // get the square with the lowest F score  
                currentNode = openList.OrderBy(l => l.FCost).FirstOrDefault();

                //Remove from currentList and add to closedList
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                //show current square on the map  
                nodeMap[currentNode.X, currentNode.Y].NodeState = EnumNodeState.Closed;
                Nodes[currentNode.X][currentNode.Y].NodeState = EnumNodeState.Closed;
                System.Threading.Thread.Sleep(SLEEP_TIME);

                //If we find goal
                if (closedList.FirstOrDefault(l => l.X == endNode.X && l.Y == endNode.Y) != null)
                    break;

                //Generate child - add neighbour nodes
                currentNode.Neighbours = GetNeighbours(currentNode.X, currentNode.Y, nodeMap, openList, enableDiagonals);
                gScore = currentNode.GScore + 1;

                //Looping through all neighbours
                foreach (Node neighbours in currentNode.Neighbours)
                {
                    //If child is in closed list
                    if (closedList.FirstOrDefault(l => l.X == neighbours.X
                        && l.Y == neighbours.Y) != null)
                        continue;

                    //If not in openList initalize it
                    if (openList.FirstOrDefault(l => l.X == neighbours.X
                        && l.Y == neighbours.Y) == null)
                    {
                        // compute its score, set the parent  
                        neighbours.GScore = gScore;
                        neighbours.HScore = ComputeHScore(neighbours, endNode);
                        neighbours.FCost = neighbours.GScore + neighbours.HScore;
                        neighbours.ParentNode = currentNode;

                        // and add it to the open list  
                        openList.Add(neighbours);
                    }
                    else
                    {
                        //If new fScore better than current fScore replace with better path
                        if (gScore + neighbours.HScore < neighbours.FCost)
                        {
                            neighbours.GScore = gScore;
                            neighbours.FCost = neighbours.GScore + neighbours.HScore;
                            neighbours.ParentNode = currentNode;
                        }
                    }
                }
            }

            Node end = currentNode;

            //If it goes here path is found - lets print it
            while (currentNode != null)
            {
                nodeMap[currentNode.X, currentNode.Y].NodeState = EnumNodeState.Path;
                Nodes[currentNode.X][currentNode.Y].NodeState = EnumNodeState.Path;
                currentNode = currentNode.ParentNode;
                System.Threading.Thread.Sleep(SLEEP_TIME);
            }

            //How many steps was required
            if (end != null)
            {
                Debug.WriteLine("Path : {0}", end.GScore);
            }
        }

        private List<Node> GetNeighbours(int x, int y, Node[,] nodeMap, List<Node> openList, bool enableDiagonals)
        {
            List<Node> returnList = new List<Node>();

            //Cardinal axis TOP BOTTOM LEFT RIGHT
            if (y - 1 >= 0)
            {
                if (nodeMap[x, y - 1].NodeState != EnumNodeState.Barrier)
                {
                    Node node = openList.Find(l => l.X == x && l.Y == y - 1);
                    if (node == null) returnList.Add(new Node(x, y - 1));
                }
            }

            if (y + 1 < nodeMap.GetLength(1))
            {
                if (nodeMap[x, y + 1].NodeState != EnumNodeState.Barrier)
                {
                    Node node = openList.Find(l => l.X == x && l.Y == y + 1);
                    if (node == null) returnList.Add(new Node(x, y + 1));
                }
            }

            if (x - 1 >= 0)
            {
                if (nodeMap[x - 1, y].NodeState != EnumNodeState.Barrier)
                {
                    Node node = openList.Find(l => l.X == x - 1 && l.Y == y);
                    if (node == null) returnList.Add(new Node(x - 1, y));
                }
            }

            if (x + 1 < nodeMap.GetLength(0))
            {
                if (nodeMap[x + 1, y].NodeState != EnumNodeState.Barrier)
                {
                    Node node = openList.Find(l => l.X == x + 1 && l.Y == y);
                    if (node == null) returnList.Add(new Node(x + 1, y));
                }
            }

            //If diagonals are enabled
            if (enableDiagonals)
            {
                if (y - 1 >= 0 && x - 1 >= 0)
                {
                    if (nodeMap[x - 1, y - 1].NodeState != EnumNodeState.Barrier)
                    {
                        Node node = openList.Find(l => l.X == x - 1 && l.Y == y - 1);
                        if (node == null) returnList.Add(new Node(x - 1, y - 1));
                    }
                }

                if (y - 1 >= 0 && x + 1 < nodeMap.GetLength(0))
                {
                    if (nodeMap[x + 1, y - 1].NodeState != EnumNodeState.Barrier)
                    {
                        Node node = openList.Find(l => l.X == x + 1 && l.Y == y - 1);
                        if (node == null) returnList.Add(new Node(x + 1, y - 1));
                    }
                }

                if (x - 1 >= 0 && y + 1 < nodeMap.GetLength(1))
                {
                    if (nodeMap[x - 1, y + 1].NodeState != EnumNodeState.Barrier)
                    {
                        Node node = openList.Find(l => l.X == x - 1 && l.Y == y + 1);
                        if (node == null) returnList.Add(new Node(x - 1, y + 1));
                    }
                }

                if (x + 1 < nodeMap.GetLength(0) && y + 1 < nodeMap.GetLength(1))
                {
                    if (nodeMap[x + 1, y + 1].NodeState != EnumNodeState.Barrier)
                    {
                        Node node = openList.Find(l => l.X == x + 1 && l.Y == y + 1);
                        if (node == null) returnList.Add(new Node(x + 1, y + 1));
                    }
                }
            }

            return returnList;
        }

        private double ComputeHScore(Node neighbourNode, Node endNode)
        {
            return Math.Sqrt((neighbourNode.X - endNode.X) ^ 2 + (neighbourNode.Y - endNode.Y) ^ 2);
        }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
