using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSCT.Helpers;
using WSCT.ISO7816;
using WSCT.ISO7816.Commands;

namespace WSCT.EMV.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class EMVSelectByNameCommand : SelectCommand
    {
        #region >> Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EMVSelectByNameCommand()
            : base()
        {
            selectionMode = SelectCommand.SelectionMode.SELECT_DF_NAME;
            fileOccurence = SelectCommand.FileOccurrence.FIRST_OR_ONLY;
            fileControlInformation = SelectCommand.FileControlInformation.RETURN_FCI;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public EMVSelectByNameCommand(Byte[] name) :
            this()
        {
            this.udc = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="le"></param>
        public EMVSelectByNameCommand(Byte[] name, uint le) :
            this(name)
        {
            this.le = le;
        }

        #endregion
    }
}
