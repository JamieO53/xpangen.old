using System.Windows.Forms;
using GenEdit.ViewModel;

namespace GenEdit.View
{
    public partial class GenDataEditor : UserControl
    {
        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }
        public GenDataEditor()
        {
            InitializeComponent();
        }
    }
}
