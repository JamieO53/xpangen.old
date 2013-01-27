using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.View
{
    public partial class GenProfileEditor : UserControl
    {
        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }
        
        public GenProfileEditor()
        {
            InitializeComponent();
        }
    }
}
