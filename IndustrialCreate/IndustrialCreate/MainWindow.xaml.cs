using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace IndustrialCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double value, m_xOrigin, m_yOrigin, m_xSpacing, m_ySpacing;
        uint valuee, m_xNumber, m_yNumber;
        BubbleLocation m_xBubbleLc, m_yBubbleLc;
        String m_xFirstLabel, m_yFirstLabel;

        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        private void UserInputs_TextChanged(object sender, TextChangedEventArgs e)
        {       
            if (double.TryParse(xCoordTextBox.Text.ToString(), out value))
            {
                if (sender == xCoordTextBox)
                {
                    m_xOrigin = value;
                }

                
            }

            if (double.TryParse(yCoordTextBox.Text.ToString(), out value))
            {
                if (sender == yCoordTextBox)
                {
                    m_yOrigin = value;
                }                
            }

            if (double.TryParse(xSpacingTextBox.Text.ToString(), out value))
            {
                if (sender == xSpacingTextBox)
                {
                    m_xSpacing = value;
                }
            }

            if (double.TryParse(ySpacingTextBox.Text.ToString(), out value))
            {

                if (sender == ySpacingTextBox)
                {
                    m_ySpacing = value;
                }
            }

            if (uint.TryParse(xNumberTextBox.Text.ToString(), out valuee))
            {
                m_xNumber = valuee;
            }

            if (uint.TryParse(yNumberTextBox.Text.ToString(), out valuee))
            {
                m_yNumber = valuee;
            }
        }

       

        private void BubbleLocTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CreateGridsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class CreateOrthogonalGridsData : CreateGridsData
    {
        

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
        #endregion

        #region methods
        /* Constructor */
        #region constructor
        public CreateOrthogonalGridsData(
            UIApplication app,
            DisplayUnitType dut,
            ArrayList labels) :
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
                XYZ srtpt;
                XYZ endpt;
                Autodesk.Revit.DB.Line line;

                try
                {
                    if (m_yNumber != 0)
                    {
                        // Grids will have an extension distance of m_ySpaceing /2
                        srtpt = new XYZ(
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
                        srtpt = new XYZ(m_xOrigin, m_yOrigin + i * m_xSpacing, 0);
                        endpt = new XYZ(m_xOrigin + m_xSpacing / 2, m_yOrigin + i * m_xSpacing, 0);
                    }

                    try
                    {
                        // Create a line according to the bubble location
                        if (m_xBubbleLc == BubbleLocation.StartPoint)
                        {
                            line = NewLine(srtpt, endpt);
                        }
                        else
                        {
                            line = NewLine(endpt, srtpt);
                        }
                    }
                    catch (ArgumentException)
                    {
                        /* res failure */
                    }
                    if (i == 0)
                    {
                        Autodesk.Revit.DB.Grid grid;
                        //Create grid with line
                        grid = NewGrid(line);
                        try
                        {
                            // set the label of first horizontal grid
                            grid.Name = m_xFirstLabel
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
