using System;
using System.Collections.Generic;
using System.Collections;
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
            #region Implement this method as an external command for Revit
            /* [Param - CommandData] is an obj that is passed to the external application
             which contains dtat related tot he command, 
             such as the application obj and the active view.

             * [Param - message] is a messge that can be set by the external application
             which will be displayed if a failure or canellation is rutrned by the external command

             * [Param - Elements] is a set of elemetns to whcih the external appllication can add elements 
             that are  to be highlighted incase of falure or cancellation

             * Returns - the status of the external command. 
             A result of Succeeded means that the API external method functioned as expected. 
             Cancelled can be used to signify that user canelled the external operation at some point.
             Failure should be returned if the applicaitons is unable to proceed iwth the operation */
            UIApplication rvtuiapp = commandData.Application;
            UIDocument rvtuidoc = rvtuiapp.ActiveUIDocument;
            _app = rvtuiapp.Application;
            _doc = rvtuidoc.Document;

            try
            {
                
                #region Get all selected lines and arcs
                CurveArray selectedCurves = GetSelectedCurves(_doc);
                #endregion

                #region Show UI
                GridCreationOptionData gridCreationOption = new GridCreationOptionData(!selectedCurves.IsEmpty);
                //using (GridCreationOptionForm gridCreationOptionForm = new GridCreationOptionForm(gridCreationOption))
                {
                    
                }
                #endregion
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return Result.Failed;
            }
            #endregion
        }

        private static CurveArray GetSelectedCurves(Document doc)
        {
            #region Get all selected lines and arcs
            /* Param - Revit's document = doc
             * Returns - CurveArray contains all selected lines and arcs */
            CurveArray selectedCurves = new CurveArray();
            UIDocument newRvtUIDoc = new UIDocument(doc);
            ElementSet elements = new ElementSet();
            foreach (ElementId elmId in newRvtUIDoc.Selection.GetElementIds())
            {
                elements.Insert(newRvtUIDoc.Document.GetElement(elmId));
            }
            foreach (Element element in elements)
            {
                if ((element is ModelLine) || (element is ModelArc))
                {
                    ModelCurve modelCurve = element as ModelCurve;
                    Curve curve = modelCurve.GeometryCurve;
                    if ( curve != null)
                    {
                        selectedCurves.Append(curve);
                    }
                } else if ((element is DetailLine) || (element is DetailArc))
                {
                    DetailCurve detailCurve = element as DetailCurve;
                    Curve curve = detailCurve.GeometryCurve;
                    if (curve != null)
                    {
                        selectedCurves.Append(curve);
                    }
                }
            }

            return selectedCurves;

            #endregion
        }

        public static ElementSet GetSelectedModelLinesAndArcs(Document doc)
        {
            #region Get all model and detail lines/arcs within selected elements
            /* Param - Revit's document = doc
             * Returns - ElementSet contains all model and
             * detail lines/arcs within selected elements */

            UIDocument newRvtUIDoc = new UIDocument(doc);
            ElementSet elements = new ElementSet();
            foreach (ElementId elmId in newRvtUIDoc.Selection.GetElementIds())
            {
                elements.Insert(newRvtUIDoc.Document.GetElement(elmId));
            }

            ElementSet tmpSet = new ElementSet();
            foreach (Element element in elements)
            {
                if ((element is ModelLine) || 
                    (element is ModelArc) ||
                    (element is DetailLine) ||
                    (element is DetailArc))
                {
                    tmpSet.Insert(element);
                }
            }

            return tmpSet;
            #endregion
        }

        private static DisplayUnitType GetLengthUnitType(Document doc)
        {
            #region Get current length display unit type
            /* Param - Revit's document = doc
             * Returns - current length display unit type */

            UnitType untT = UnitType.UT_Length;
            Units prjUnt = doc.GetUnits();
            try
            {
                FormatOptions formatOption = prjUnt.GetFormatOptions(untT);
                return formatOption.DisplayUnits;
            } catch (Exception /*e*/)
            {
                return DisplayUnitType.DUT_DECIMAL_FEET;
            }

            #endregion
        }

        private static ArrayList GetAllLabelsOfGrids(Document doc)
        {
            #region Get all grid labels in current document
            // ArrayList contains all grid labels in current document

            ArrayList labels = new ArrayList();
            FilteredElementIdIterator itor = new FilteredElementCollector(doc).OfClass(
                typeof(Grid)
                ).GetElementIdIterator();
            itor.Reset();
            for (; itor.MoveNext(); )
            {
                Grid grid = itor.Current as Grid;
                if (null != grid)
                {
                    labels.Add(grid.Name);
                }
            }

            return labels;

            #endregion
        }
    }
}
