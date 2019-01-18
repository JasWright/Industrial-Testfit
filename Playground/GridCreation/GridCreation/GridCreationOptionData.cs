using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace GridCreation
{
    /* Data class which stores the information of the way to create grids*/
    class GridCreationOptionData
    {
        #region Fields
        private CreateMode m_createGridsMode;

        private bool m_hasSelectedLinesOrArcs;
        #endregion

        #region Properties
        public CreateMode CreateGridMode
        {
            get
            {
                return m_createGridsMode;
            }
            set
            {
                m_createGridsMode = value;
            }
        }

        public bool HasSelectedLinesOrArcs
        {
            get
            {
                return m_hasSelectedLinesOrArcs;
            }
        }
        #endregion

        #region Methods
        public GridCreationOptionData(bool hasSelectedLineOrArcs)
        {
            m_hasSelectedLinesOrArcs = hasSelectedLineOrArcs;
        }
        #endregion
    }
}
