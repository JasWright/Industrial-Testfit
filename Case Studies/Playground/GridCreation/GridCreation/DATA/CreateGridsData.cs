using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Resources;


using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace GridCreation.DATA
{
    /// <summary>
    ///  Base class of all grid creation data class
    /// </summary>
    public class CreateGridsData
    {
        #region Fields
        /// <summary>
        /// The active document of Revit
        /// </summary>
        protected Document m_rvtDoc;
        /// <summary>
        /// Document Creation object to create new elements
        /// </summary>
        protected Autodesk.Revit.Creation.Document m_docCreator;
        /// <summary>
        /// Application Creation object to create new elements
        /// </summary>
        protected Autodesk.Revit.Creation.Application m_appCreator;
        /// <summary>
        /// Array list contains all grid labels in current document
        /// </summary>
        private ArrayList m_labelsList;
        /// <summary>
        /// Current display unit type
        /// </summary>
        protected DisplayUnitType m_dut;
        /// <summary>
        /// Resource manager
        /// </summary>
        //protected static ResourceManager resManager = GridCreation.pr
        
        #endregion

        #region Properties
        /// <summary>
        /// Current display unit type
        /// </summary>
        public DisplayUnitType Dut
        {
            get
            {
                return m_dut;
            }
        }
        /// <summary>
        /// Get Array list conatins all grid labels in current document
        /// </summary>
        public ArrayList LabelsList
        {
            get
            {
                return m_labelsList;
            }
        }
        #endregion

        #region Methods

        // 1 Constructor without display unit type

        // 2 Constructor with display unit type

        // 3 Get the line to create grid according to the specified bubble location

        // 4 Get the arc to create grid according to the specified bubble location

        // 5 Get the arc the create grid according to the specified bubble location

        // 6 Split a circle inot the upper and lower parts

        // 7 Create a new bound line

        // 8 Create a grid with a line

        // 9 Create a grid with an arc

        // 10 Create linear grid

        // 11 Create batch of grids with curves

        // 12 Add curve to curve array for batch creation

        // 13 Show a message box

        #region constructor WITHOUT display unit type

        /// <summary>
        /// Constructor without display unit type
        /// </summary>
        /// <param name="app">Revit application</param> 
        /// <param name="labels">All existing labels in Revit's document</param>
        public CreateGridsData(UIApplication app, ArrayList labels)
        {
            m_rvtDoc = app.ActiveUIDocument.Document;
            m_appCreator = app.Application.Create;
            m_docCreator = app.ActiveUIDocument.Document.Create;
            m_labelsList = labels;
        }
        #endregion

        #region constructor WITH display unit type
        public CreateGridsData(UIApplication app, ArrayList labels, DisplayUnitType dut)
        {
            m_rvtDoc = app.ActiveUIDocument.Document;
            m_appCreator = app.Application.Create;
            m_docCreator = app.ActiveUIDocument.Document.Create;
            m_labelsList = labels;
            m_dut = dut;
        }
        #endregion

        #region GET line according to bubble location
        /* Param - line is the original selected line
         * Param - bubble location
         * Return- The line to create grid 
         */
         protected Line TransformLine(Line line, BubbleLocation bublc)
        {
            Line lineToCreate;

            // Create grid according to the bubble location
            if (bublc == BubbleLocation.StartPoint)
            {
                lineToCreate = line;
            }
            else
            {
                XYZ srtpt = line.GetEndPoint(1);
                XYZ endpt = line.GetEndPoint(0);
                lineToCreate = NewLine(srtpt, endpt);
            }

            return lineToCreate;
        }
        #endregion

        #region GET arc according to bubble location
        protected Arc TransformArc(Arc arc, BubbleLocation bublc)
        {
            Arc arc2Create;
            if (bublc == BubbleLocation.StartPoint)
            {
                arc2Create = arc;
            }
            else
            {
                // Get start point, end point of the arc and the middle point on it
                XYZ srt = arc.GetEndPoint(0);
                XYZ end = arc.GetEndPoint(1);
                bool clockwise = (arc.Normal.Z == -1);

                // Get start angle and end angle of arc
                double startDegree = arc.GetEndParameter(0);
                double endDegree = arc.GetEndParameter(1);

                // Handle the case that the arc is clockwise
                if (clockwise && startDegree > 0 && endDegree > 0)
                {
                    startDegree = 2 * Values.PI - startDegree;
                    endDegree = 2 * Values.PI - endDegree;
                }
                else if ( clockwise && startDegree < 0)
                {
                    double temp = endDegree;
                    endDegree = -1 * startDegree;
                    startDegree = -1 * temp;
                }

                double sumDegree = (startDegree + endDegree) / 2;
                while (sumDegree > 2 * Values.PI)
                {
                    sumDegree -= 2 * Values.PI;
                }

                while (sumDegree < -2 * Values.PI)
                {
                    sumDegree += 2 * Values.PI;
                }

                XYZ midPoint = new XYZ(arc.Center.X + arc.Radius * Math.Cos(sumDegree),
                    arc.Center.Y + arc.Radius * Math.Sin(sumDegree), 0);

                arc2Create = Arc.Create(end, srt, midPoint);
            }

            return arc2Create;
        }
        #endregion

        #region GET arc according to bubble location
        /* Param - Origin is arc grid's origin
         * Param - Radius is arc grid's radius
         * Param - StartDegree is Arc grid's start degree
         * Param - EndDegree is Arc grid's end degree
         * Param - Bubloc is Arc grid's bubble location
         * Returns - The expected arc to create grid 
         */
        protected Arc TransformArc(
            XYZ origin,
            double radius,
            double startDegree,
            double endDegree,
            BubbleLocation bubLoc)
        {
            Arc arcToCreate;
            // Get start point and end point of the arc and the middle point on the arc
            XYZ startPoint = new XYZ(origin.X + radius * Math.Cos(startDegree),
                origin.Y + radius * Math.Sin(startDegree), origin.Z);
            XYZ midPoint = new XYZ(origin.X + radius * Math.Cos((startDegree + endDegree) / 2),
                origin.Y + radius * Math.Sin((startDegree + endDegree) / 2), origin.Z);
            XYZ endPoint = new XYZ(origin.X + radius * Math.Cos(endDegree),
                origin.Y + radius * Math.Sin(endDegree), origin.Z);

            if (bubLoc == BubbleLocation.StartPoint)
            {
                arcToCreate = Arc.Create(startPoint, endPoint, midPoint);
            }
            else
            {
                arcToCreate = Arc.Create(endPoint, startPoint, midPoint);
            }

            return arcToCreate;
        }
            #endregion

        #region SPLIT a circle into the upper and lower parts

        #endregion

        #region CREATE new bound line
        /* Param - start point of line
         * Param - end point of line
         */
        protected Line NewLine(XYZ start, XYZ end)
        {
            return Line.CreateBound(start, end);
        }
        #endregion

        #region CREATE grid w/ line
        /* Param - line  to create grid
         * Returns - Newly created grid
         */
        protected Grid NewGrid(Line line)
        {
            return Grid.Create(m_rvtDoc, line);
        }
        #endregion

        #region CREATE grid w/ arc
        protected Grid NewGrid(Arc arc)
        {
            return Grid.Create(m_rvtDoc, arc);
        }
        #endregion

        #region CREATE linear grid
        protected Grid CreateLinearGrid(Line line)
        {
            return Grid.Create(m_rvtDoc, line);
        }
        #endregion

        #region CREATE batch of grids with curves
        protected void CreateGrids(CurveArray curves)
        {
            foreach (Curve c in curves)
            {
                Line line = c as Line;
                Arc arc = c as Arc;

                if (line != null)
                {
                    Grid.Create(m_rvtDoc, line);
                }

                if (arc != null)
                {
                    Grid.Create(m_rvtDoc, arc);
                }
            }
        }
        #endregion

        #region ADD curve to curve array 
        public static void AddCurveForBatchCreation(ref CurveArray curves, Curve curve)
        {
            curves.Append(curve);
        }
        #endregion

        #region SHOW a message box
        public static void ShowMessage(String msg, String caption)
        {
            TaskDialog.Show(caption, msg, TaskDialogCommonButtons.Ok);
        }
        #endregion


        #endregion
    }
}
