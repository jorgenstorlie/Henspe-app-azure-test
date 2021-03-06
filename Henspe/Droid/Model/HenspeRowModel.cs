﻿using System;

namespace Henspe.Core.Model.Dto
{
	public class HenspeRowModel
    {
		public StructureElementDto.ElementType elementType;
        public string description { get; set; }
        public string image { get; set; }

		public HenspeRowModel(StructureElementDto.ElementType elementType, string description, string image)
        {
            this.elementType = elementType;
            this.description = description;
            this.image = image;
        }
    }
}