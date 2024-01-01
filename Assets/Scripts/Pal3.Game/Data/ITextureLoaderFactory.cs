﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2024, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Game.Data
{
    using Engine.Core.Abstraction;

    public interface ITextureLoaderFactory
    {
        /// <summary>
        /// Get ITextureLoader instance based on image file extension.
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public ITextureLoader GetTextureLoader(string fileExtension);
    }
}