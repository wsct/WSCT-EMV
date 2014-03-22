﻿using System;
using System.Collections.Generic;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Interface to be implemented by all hash algorithm provider
    /// </summary>
    public interface IHashAlgorithmProvider
    {
        /// <summary>
        /// Implementation of hash computation.
        /// </summary>
        /// <param name="data">List of Byte[] to be concatenated an hashed.</param>
        /// <returns>Hash value.</returns>
        Byte[] ComputeHash(List<Byte[]> data);
    }
}
