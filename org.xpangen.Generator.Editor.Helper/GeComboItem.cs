namespace org.xpangen.Generator.Editor.Helper
{
    public class GeComboItem
    {
        public GeComboItem(string displayValue, string dataValue)
        {
            DataValue = dataValue;
            DisplayValue = displayValue;
        }

        /// <summary>
        /// The value to be shown in the combo list
        /// </summary>
        public string DisplayValue { get; private set; }

        /// <summary>
        /// The value to be saved in the field value
        /// </summary>
        public string DataValue { get; private set; }
    }
}