using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Element = Autodesk.Revit.DB.Element;

namespace GridCreation
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        Application _app;
        Document _doc;

        public virtual Result Execute(
            ExternalCommandData commandData,
            ref string msg, 
            ElementSet elements)
        {
            UIApplication rvtuiapp = commandData.Application;
            UIDocument rvtuidoc = rvtuiapp.ActiveUIDocument;
            _app = rvtuiapp.Application;
            _doc = rvtuidoc.Document;

            try
            {
                Document doc = commandData.Application.ActiveUIDocument.Document;

                CurveArray selectedCurves = GetSelectedCurves(_doc);
            }
        }
    }
}
