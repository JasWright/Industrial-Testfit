using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace GridCreation.DATA
{
    /// <summary>
    /// Data class which stores information for creating orthongonal grids
    /// </summary>
    public class CreateOrthogonalGridsData : CreateGridsData
    {
        #region Fields
        // X coordinate of origin
        // y coordinate of origin
        // spacing btwn horizontal grids
        // spacing btwn vertical grids
        // number of horizontal grids
        // number of vertical grids
        // bubble location of horizontal grids 
        // bubble location of vertical grids
        // label of first horizontal grid
        // label of first vertical grid
        private double m_xOrigin;
        private double m_yOrigin;

        private double m_xSpacing;
        private double m_ySpacing;

        private uint m_xNumber;
        private uint m_yNumber;

        private BubbleLocation m_xBubbleLc;
        private BubbleLocation m_yBubbleLc;

        private String m_xFirstLabel;
        private String m_yFirstLabel;
        #endregion

        #region Properties
        // x coordinate of origin
        public double XOrigin
        {
            get
            {
                return m_xOrigin;
            }
            set
            {
                m_xOrigin = value;
            }
        }

        // y coordinate of origin
        public double YOrigin
        {
            get
            {
                return m_yOrigin;
            }
            set
            {
                m_yOrigin = value;
            }
        }
        // spacing btwn horizontal grids
        public double XSpacing
        {
            get
            {
                return m_xSpacing;
            }
            set
            {
                m_xSpacing = value;
            }
        }
        // spacing btwn vertical grids
        public double YSpacing
        {
            get
            {
                return m_ySpacing;
            }
            set
            {
                m_ySpacing = value;
            }
        }
        // number of horizontal grids
        public uint XNumber
        {
            get
            {
                return m_xNumber;
            }
            set
            {
                m_xNumber = value;
            }
        }
        // number of vertical grids
        public uint YNumber
        {
            get
            {
                return m_yNumber;
            }
            set
            {
                m_yNumber = value;
            }
        }
        // bubble location of horizontal grids
        public BubbleLocation XBubbleLc
        {
            get
            {
                return m_xBubbleLc;
            }
            set
            {
                m_xBubbleLc = value;
            }
        }
        // bubble location of vertical grids
        public BubbleLocation YBubbleLc
        {
            get
            {
                return m_yBubbleLc;
            }
            set
            {
                m_yBubbleLc = value;
            }
        }
        // label of the first horizonal grid
        public String XFirstLabel
        {
            get
            {
                return m_xFirstLabel;
            }
            set
            {
                m_xFirstLabel = value;
            }
        }
        // label of the first vertical grid
        public String YFirstLabel
        {
            get
            {
                return m_yFirstLabel;
            }
            set
            {
                m_yFirstLabel = value;
            }
        }

        #region methods
        /* Constructor */
        #region constructor
        public CreateOrthogonalGridsData(
            UIApplication app, 
            DisplayUnitType dut, 
            ArrayList labels):
            base(app, labels, dut)
        {

        }
        #endregion

        /* Create Grids*/
        #region create grids
        public void CreateGrids()
        {
            ArrayList failureReasons = new ArrayList();
            
        }
        #endregion

        /* Create Horizontal Grids*/
        #region horizontal grids
        private int CreateXGrids(ref ArrayList failureReasons)
        {
            int errorCount = 0;

            // Curve array which stores all Curves for batch creation
            CurveArray curves = new CurveArray();

            for (int i = 0; i < m_xNumber; ++i)
            {
                XYZ startpt;
                XYZ endpt;
                Line line;

                try
                {
                    if (m_yNumber != 0)
                    {
                        // Grids will have an extension distance of m_ySpaceing /2
                        startpt = new XYZ(
                            m_xOrigin - m_ySpacing / 2,
                            m_yOrigin + i * m_xSpacing,
                            0);
                        endpt = new XYZ(
                            m_xOrigin + (m_yNumber - 1) * m_ySpacing + m_ySpacing / 2,
                            m_yOrigin + i * m_xSpacing,
                            0);
                    }
                    else
                    {
                        startpt = new XYZ(m_xOrigin, m_yOrigin + i * m_xSpacing, 0);
                        endpt = new XYZ(m_xOrigin + m_xSpacing / 2, m_yOrigin + i * m_xSpacing, 0);
                    }

                    try
                    {
                        // Create a line according to the bubble location
                        if (m_xBubbleLc == BubbleLocation.StartPoint)
                        {
                            
                        }
                    }
                    catch
                    {

                    }
                    if
                    {
                        //Create grid with line
                        try
                        {
                            // set the label of first horizontal grid
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        // add the line to curve array
                    }
                }
                catch
                {

                }
            }
            // Create grids with curve array
        }
        #endregion

        /* Create Vertical Grids*/
        #region vertical grids
        private int CreateYGrids(ref ArrayList failurReasons)
        {
            // curve array which stores all curves for batch creation
        }
        #endregion
        #endregion
    }
}
