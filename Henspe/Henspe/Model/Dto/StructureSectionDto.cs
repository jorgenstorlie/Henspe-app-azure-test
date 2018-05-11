using System;
using System.Collections.Generic;

namespace Henspe.Core.Model.Dto
{
    public class StructureSectionDto
	{
        public string description { get; set; }
		public string image { get; set; }
        public List<StructureElementDto> structureElementList { get; set; }

		public StructureSectionDto (string description, string image)
		{
            this.description = description;
			this.image = image;
		}

		public void AddStructureElement(StructureElementDto.ElementType elementType, string description, string image)
        {
			StructureElementDto structureElementDto = new StructureElementDto(elementType, description, image);
            structureElementList.Add(structureElementDto);
        }
	}
}