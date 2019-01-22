#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Util;
#endregion

namespace IntroCs
{
    [Transaction(TransactionMode.Manual)]
    public class _10_practice : IExternalCommand
    {
        Application _app;
        Document _doc;

        public Autodesk.Revit.UI.Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication rvtUIApp = commandData.Application;
            UIDocument uiDoc = rvtUIApp.ActiveUIDocument;
            _app = rvtUIApp.Application;
            _doc = uiDoc.Document;

            using (Transaction transaction = new Transaction(_doc))
            {
                transaction.Start("Create House");
                // Calls Create House
                CreateHouse();
                transaction.Commit();
            }

            return Result.Succeeded;
        }

        #region Create House
        public static void CreateHouse(Document rvtDoc)
        {
            using (Transaction transaction = new Transaction(rvtDoc))
            {
                transaction.Start("Create House");
                // Simply create four walls with rectangular profile. 
                List<Wall> walls = CreateWalls(rvtDoc);

                // Add a door to the second wall 
                AddDoor(rvtDoc, walls[0]);

                // Add windows to the rest of the walls. 
                for (int i = 1; i <= 3; i++)
                {
                    AddWindow(rvtDoc, walls[i]);
                }

                // (optional) add a roof over the walls' rectangular profile. 

                AddRoof(rvtDoc, walls);

                transaction.Commit();
            }
            
        }
        #endregion

        #region CreateWalls
        public static List<Wall> CreateWalls(Document rvtDoc)
        {
            // Hard coding the lower-left and upper-right corners of walls. 
            XYZ pt1 = new XYZ(Constant.MmToFeet(-5000.0), Constant.MmToFeet(-2500.0), 0.0);
            XYZ pt2 = new XYZ(Constant.MmToFeet(5000.0), Constant.MmToFeet(2500.0), 0.0);

            List<Wall> walls = CreateWalls(rvtDoc, pt1, pt2);

            return walls;
        }

        private static List<Wall> CreateWalls(Document rvtDoc, XYZ pt1, XYZ pt2)
        {
            // set the lower-left (x1, y1) and upper-right (x2, y2) corners of a house. 
            double x1 = pt1.X;
            double x2 = pt2.X;
            if (pt1.X > pt2.X)
            {
                x1 = pt2.X;
                x2 = pt1.X;
            }

            double y1 = pt1.Y;
            double y2 = pt2.Y;
            if (pt1.Y > pt2.Y)
            {
                y1 = pt2.Y;
                y2 = pt1.Y;
            }

            // set four courner of walls from two corner point. 
            // 5th point is for combenince to loop through. 
            List<XYZ> pts = new List<XYZ>(5);
            pts.Add(new XYZ(x1, y1, pt1.Z));
            pts.Add(new XYZ(x2, y1, pt1.Z));
            pts.Add(new XYZ(x1, y2, pt1.Z));
            pts.Add(new XYZ(x2, y2, pt1.Z));
            pts.Add(pts[0]);

            // get the levels we want to work on.
            // Note: hard coding for simplicity. Modify here you use a different template.
            Level lvl1 = ElementFiltering.FindElement(rvtDoc, typeof(Level), "Level 1", null) as Level;
            if (lvl1 == null)
            {
                TaskDialog.Show(
                    "Create walls",
                    "Cannot find (Level 1). Maybe you used a different template? Try with DefaultMetric.rte.");
                return null;
            }

            Level lvl2 = ElementFiltering.FindElement(rvtDoc, typeof(Level), "Level 2", null) as Level;
            if (lvl2 == null)
            {
                TaskDialog.Show(
                    "Create walls",
                    "Cannot find (Level 2). Maybe you used a different template? Try with DefaultMetric.rte.");
                return null;
            }

            bool isStructural = false;                                                   // Flag for structural wall or not

            List<Wall> walls = new List<Wall>(4);                                        // save walls user created

            for (int i = 0; i <= 3; i++)                                                 // Loop through list of points and define four walls. 
            {
                Line baseCurve = Line.CreateBound(pts[i], pts[i + 1]);                   // define a base curve from two points 
                Wall aWall = Wall.Create(rvtDoc, baseCurve, lvl1.Id, isStructural);      // create a wall using the one of overloaded methods.
                aWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(lvl2.Id);     // set the Top Constraint to lvl2
                walls.Add(aWall);                                                        // save the wall
            }

            rvtDoc.Regenerate();                                                         // This is important. user needs these lines to have shrinkwrap working. 
            rvtDoc.AutoJoinElements();
            return walls;

        }
        #endregion

        #region AddDoor
        public void AddDoor(Wall hostWall)
        {

        }
        #endregion

        #region AddWindow
        public void AddWindow(Wall hostWall)
        {
            // hard coding the window type we will use. 
            // e.g., "M_Fixed: 0915 x 1830mm 

            const string windowFamilyName = "Fixed"; // "M_Fixed" 
            const string windowTypeName = "16\" x 24\""; // "0915 x 1830mm" 
            const string windowFamilyAndTypeName =
                             windowFamilyName + ": " + windowTypeName;
            double sillHeight = Util.Constant.MmToFeet(915);

            // get the door type to use. 

            FamilySymbol windowType =
                (FamilySymbol)ElementFiltering.FindFamilyType(
                 m_rvtDoc, typeof(FamilySymbol), windowFamilyName, windowTypeName,
                 BuiltInCategory.OST_Windows);

            if (windowType == null)
            {
                TaskDialog.Show("Revit Intro Lab", "Cannot find (" +
                    windowFamilyAndTypeName +
                    "). Try with DefaultMetric.rte.");
            }

            // get the start and end points of the wall. 

            LocationCurve locCurve = (LocationCurve)hostWall.Location;
            XYZ pt1 = locCurve.Curve.GetEndPoint(0);
            XYZ pt2 = locCurve.Curve.GetEndPoint(1);
            // calculate the mid point. 
            XYZ pt = (pt1 + pt2) / 2.0;

            // we want to set the reference as a bottom of the wall or level1. 

            ElementId idLevel1 =
                hostWall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).
                AsElementId();
            Level level1 = (Level)m_rvtDoc.GetElement(idLevel1);

            // finally create a window. 

            FamilyInstance aWindow = m_rvtDoc.Create.NewFamilyInstance(
                pt, windowType, hostWall, level1, StructuralType.NonStructural);

            // set the sill height 
            aWindow.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).
                Set(sillHeight);

        }
        #endregion

        #region Add Roof
        public void AddRoof(Document rvtDoc, List<Wall> walls)
        {
            // Hardcoding the roof type we will use. 
            // e.g., "Basic Roof: Generic - 400mm"

            const string roofFamilyName = "Basic Roof";
            const string roofTypeName = "Generic - 9\"";
            const string roofFamilyAndTypeName = roofFamilyName + ": " + roofTypeName;

            // Find the roof type

            RoofType roofType = (RoofType)ElementFiltering.FindFamilyType(
                rvtDoc, 
                typeof(RoofType), 
                roofFamilyName, 
                roofTypeName, 
                null);

            if (roofType == null)
            {
                TaskDialog.Show(
                    "Add roof",
                    "Cannot find (" +
                    roofFamilyAndTypeName +
                    "). Maybe you use a different template? Try with DefaultMetric.rte.");
            
            }

            /*
            Wall thickness to adjust the  footprint of the walls 
            to the outer most lines.
            * Note: This may not be the best way,
            * but we will live with this excercise.
            DIM wallThickness as Double
            */
            double wallThickness = walls[0].WallType.GetCompoundStructure().GetLayers()[0].Width;

            double dt = wallThickness / 2.0;
            List<XYZ> dts = new List<XYZ>(5);
            dts.Add(new XYZ(-dt, -dt, 0.0));
            dts.Add(new XYZ(dt, -dt, 0.0));
            dts.Add(new XYZ(dt, dt, 0.0));
            dts.Add(new XYZ(-dt, dt, 0.0));
            dts.Add(dts[0]);

            // set the profile from four wals
            CurveArray footPrint = new CurveArray();
            for (int i = 0; i <= 3; i++)
            {
                LocationCurve locCurve = (LocationCurve)walls[i].Location;
                XYZ pt1 = locCurve.Curve.GetEndPoint(0) + dts[i];
                XYZ pt2 = locCurve.Curve.GetEndPoint(1) + dts[i + 1];
                Line line = Line.CreateBound(pt1, pt2);
                footPrint.Append(line);
            }

            /////////////////////////////////////////////////
            ///new mapping
            /////////////////////////////////////////////////
            Curve getCurveOne = footPrint.get_Item(1);
            Curve getCurveTwo = footPrint.get_Item(3);
            CurveArray gabledFootprint = new CurveArray();

            gabledFootprint.Append(getCurveOne);
            gabledFootprint.Append(getCurveTwo);

            // get the lvl2 from wall 

            ElementId idlvl2 = walls[0].get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId();
            Level lvl2 = rvtDoc.GetElement(idlvl2) as Level;

            ModelCurveArray mapping = new ModelCurveArray();                                               // footprint to model curve mapping
            FootPrintRoof aRoof = rvtDoc.Create.NewFootPrintRoof(footPrint, lvl2, roofType, out mapping);
            

            //double offsetValue = 10;


            foreach /*set the slope */ (ModelCurve modelCurve in mapping)
            {
                aRoof.set_DefinesSlope(modelCurve, true);
                aRoof.set_SlopeAngle(modelCurve, 0.5);
                //aRoof.set_Offset(modelCurve, offsetValue);
            }

            /* performed automatically by transaction commit.
             * rvtDoc.Regenerate();
             * rvtDoc.AutoJoinElement();
             */
        }
        #endregion

    }

}
