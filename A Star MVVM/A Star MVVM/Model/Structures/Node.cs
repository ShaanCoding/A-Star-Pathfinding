using A_Star_MVVM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_MVVM.Model.Structures
{
    public class Node : INotifyPropertyChanged
    {
        private int _x;
        private int _y;
        private Node _parentNode;
        private List<Node> _neighbours;
        private double _fCost;
        private double _gScore;
        private double _hScore;
        private EnumNodeState _nodeState;
        private ObservableCollection<EnumNodeState> _possibleValues;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            Neighbours = new List<Node>();
            NodeState = EnumNodeState.Open;
            _possibleValues = GeneratePossibleValues();
        }

        public Node(int x, int y, EnumNodeState nodeState)
        {
            X = x;
            Y = y;
            Neighbours = new List<Node>();
            NodeState = nodeState;
            _possibleValues = GeneratePossibleValues();
        }

        public int X
        {
            get
            {
                return X;
            }
            set
            {
                _x = (_x == value) ? value : _x;
                OnPropertyChanged(nameof(X));
            }
        }

        public int Y
        {
            get
            {
                return Y;
            }
            set
            {
                _y = (_y == value) ? value : _y;
                OnPropertyChanged(nameof(Y));
            }
        }

        public Node ParentNode
        {
            get
            {
                return _parentNode;
            }
            set
            {
                _parentNode = (_parentNode == value) ? value : _parentNode;
                OnPropertyChanged(nameof(ParentNode));
            }
        }

        public List<Node> Neighbours
        {
            get
            {
                return _neighbours;
            }
            set
            {
                _neighbours = (_neighbours == value) ? value : _neighbours;
                OnPropertyChanged(nameof(Neighbours));
            }
        }

        public double FCost
        {
            get
            {
                return _fCost;
            }
            set
            {
                _fCost = (_fCost == value) ? value : _fCost;
                OnPropertyChanged(nameof(FCost));
            }
        }

        public double GScore
        {
            get
            {
                return _gScore;
            }
            set
            {
                _gScore = (_gScore == value) ? value : _gScore;
                OnPropertyChanged(nameof(GScore));
            }
        }

        public double HScore
        {
            get
            {
                return _hScore;
            }
            set
            {
                _hScore = (_hScore == value) ? value : _hScore;
                OnPropertyChanged(nameof(HScore));
            }
        }

        public EnumNodeState NodeState
        {
            get
            {
                return _nodeState;
            }
            set
            {
                _nodeState = (_nodeState == value) ? value : _nodeState;
                OnPropertyChanged(nameof(NodeState));
            }
        }

        public ObservableCollection<EnumNodeState> GeneratePossibleValues()
        {
            ObservableCollection<EnumNodeState> returnPossibleValues = new ObservableCollection<EnumNodeState>();
            returnPossibleValues.Add(EnumNodeState.Barrier);
            returnPossibleValues.Add(EnumNodeState.Closed);
            returnPossibleValues.Add(EnumNodeState.EndNode);
            returnPossibleValues.Add(EnumNodeState.Open);
            returnPossibleValues.Add(EnumNodeState.Path);
            returnPossibleValues.Add(EnumNodeState.StartNode);

            return returnPossibleValues;
        }

        public ObservableCollection<EnumNodeState> PossibleValues
        {
            get
            {
                return _possibleValues;
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
