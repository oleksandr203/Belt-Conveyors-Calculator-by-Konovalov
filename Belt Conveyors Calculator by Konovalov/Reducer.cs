namespace Belt_Conveyors_Calculator_by_Konovalov
{
    public class Reducer <T, K>
        where T : struct
        where K : struct
    {
        public readonly T _id;
        public readonly string _name;
        public readonly T _maxTorque;
        public readonly K _ratio;

        public Reducer(T id, string name, T maxTorque, K ratio)
        {
            _id = id;
            _name = name;
            _maxTorque = maxTorque;
            _ratio = ratio;
        }
    }
}
