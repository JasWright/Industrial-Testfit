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
        public DisplayUnit Dut
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

        public CreateGridsData(UIApplication app, ArrayList labels, DisplayUnitType dut)
        {
            m_rvtDoc = app.ActiveUIDocument.Document;
            m_appCreator = app.Application.Create;
            m_docCreator = app.ActiveUIDocument.Document.Create;
            m_labelsList = labels;
            m_dut = dut;
        }
        #endregion
    }
}
