using System;

namespace org.xpangen.Generator.Data
{
    public class GenDataChangedEventArgs : EventArgs
    {
        public string ClassName { get; private set; }
        public string PropertyName { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        public GenDataChangedEventArgs(string className, string propertyName, string oldValue, string newValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
            PropertyName = propertyName;
            ClassName = className;
        }
    }
}