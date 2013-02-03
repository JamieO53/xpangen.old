using System.Collections.ObjectModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.ViewModel
{
    public class GenObjectViewModel: BindableObject
    {
        private ObservableCollection<GenObjectFieldViewModel> _fields;
        /// <summary>
        /// The list of fields belonging to the <see cref="GenObject"/> being edited.
        /// </summary>
        public ObservableCollection<GenObjectFieldViewModel> Fields
        {
            get
            {
                var classId = GenAttributes.GenObject.ClassId;
                if (_fields == null)
                {
                    _fields = new ObservableCollection<GenObjectFieldViewModel>();
                    var n = GenAttributes.GenDataDef.Properties[classId].Count;
                    var @class = Definition as Class;
                    var subClass = Definition as SubClass;
                    var property = Definition as Property;
                    for (var i = 0; i < n; i++)
                        if (@class != null)
                            _fields.Add(new GenObjectFieldViewModel(GenAttributes, i, @class.PropertyList[i]));
                        else if (subClass != null)
                            _fields.Add(new GenObjectFieldViewModel(GenAttributes, i, null));
                        else if (property != null)
                            _fields.Add(new GenObjectFieldViewModel(GenAttributes, i, property));
                }
                return _fields;
            }
        }

        /// <summary>
        /// Initializes a new instance of the GenObjectViewModel class.
        /// </summary>
        /// <param name="genObject">The <see cref="GenObject"/> being edited.</param>
        /// <param name="definition">The class definition of the object being edited.</param>
        public GenObjectViewModel(GenObject genObject, GenApplicationBase definition)
        {
            Definition = definition;
            GenAttributes = new GenAttributes(genObject.GenData.GenDataDef) { GenObject = genObject };
        }

        private GenApplicationBase Definition { get; set; }

        public GenAttributes GenAttributes { get; set; }

        /// <summary>
        /// The heading value of the tree node
        /// </summary>
        public string Name { get { return GenAttributes.AsString("Name"); } }

        /// <summary>
        /// A hint describing the use of the node data
        /// </summary>
        public string Hint { get { return GenAttributes.AsString("Title"); } }

        /// <summary>
        /// Establish the context of the current object.
        /// </summary>
        public void EstablishContext()
        {
            GenAttributes.GenData.EstablishContext(GenAttributes.GenObject);
        }
    }

    public class ObservableCollection<T>: Collection<T>
    {
    }
}