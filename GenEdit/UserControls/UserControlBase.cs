using System.ComponentModel;
using System.Windows.Forms;

namespace GenEdit.UserControls
{
    public class UserControlBase : UserControl
    {
        private BindingSource _bindingSource;
        protected BindingSource BindingSource
        {
            get { return _bindingSource; }
            set
            {
                if (value != null)
                    DefaultDataSource = value.DataSource;

                _bindingSource = value;
            }
        }

        protected UserControlBase(string viewModelName)
        {
            ViewModelName = viewModelName;
        }

        public void SaveChanges()
        {
            var control = ActiveControl;
            var textBox = control as TextBox;
            if (textBox != null && textBox.Modified)
            {
                Validate();
            }
        }

        protected object DefaultDataSource { get; private set; }

        protected void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Split('.')[0] == ViewModelName)
                BindingSource.ResetBindings(false);
        }

        private string ViewModelName { get; set; }
    }
}