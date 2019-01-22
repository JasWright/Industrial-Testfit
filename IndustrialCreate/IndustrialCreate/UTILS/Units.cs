#region COPYRIGHT
//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion

#region REFERENCES
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;

using Autodesk.Revit.DB;
#endregion

namespace IndustrialCreate
{
    class Units
    {
        #region METHODS

        /// <summary>
        /// Convert the value get from RevitAPI to the value indicated by DisplayUnitType
        /// </summary>
        /// <param name="to"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static double ConvertFromAPI(DisplayUnitType to, double value)
        {
            return value *= ImperialDutRatio(to);
        }

        /// <summary>
        /// Convert a value indicated by DisplayUnitType to the value used by RevitAPI
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static double CovertToAPI(double value, DisplayUnitType from)
        {
            return value /= ImperialDutRatio(from);

        }

        /// <summary>
        /// Get ratio between value in RevitAPI and value to display indicated by DisplayUnitType
        /// </summary>
        /// <param name="dut">DisplayUnitType indicateds display unit type</param>
        /// <returns></returns>
        private static double ImperialDutRatio(DisplayUnitType dut)
        {
            switch (dut)
            {
                case DisplayUnitType.DUT_DECIMAL_FEET: return 1;
                case DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES: return 1;
                case DisplayUnitType.DUT_DECIMAL_INCHES: return 12;
                case DisplayUnitType.DUT_FRACTIONAL_INCHES: return 12;
                case DisplayUnitType.DUT_METERS: return 0.3048;
                case DisplayUnitType.DUT_CENTIMETERS: return 30.48;
                case DisplayUnitType.DUT_MILLIMETERS: return 304.8;
                case DisplayUnitType.DUT_METERS_CENTIMETERS: return 0.3048;
                default: return 1;
            }
        }
        #endregion
    }
}
