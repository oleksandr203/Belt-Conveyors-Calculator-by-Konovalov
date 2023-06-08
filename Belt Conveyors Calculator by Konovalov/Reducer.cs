using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    public class Reducer
    {
        public readonly int _id;
        public readonly string _name;
        public readonly int _maxTorque;
        public readonly double _ratio;

        public Reducer(int id, string name, int maxTorque, double ratio)
        {
            _id = id;
            _name = name;
            _maxTorque = maxTorque;
            _ratio = ratio;
        }
    }
}
