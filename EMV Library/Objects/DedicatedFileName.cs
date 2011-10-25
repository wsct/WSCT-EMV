using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Dedicated File Name of an EMV application
    /// </summary>
    public class DedicatedFileName : BinaryTLVObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DedicatedFileName()
            : base()
        {
        }
    }
}
