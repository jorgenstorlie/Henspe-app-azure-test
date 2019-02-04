using System;
using System.Collections.Generic;

namespace Henspe.Core.Model.Dto
{
    public class StructureDto
    {
        public List<StructureSectionDto> structureSectionList { get; set; } = new List<StructureSectionDto>();
		public int currentStructureSectionId { get; set; }

        public StructureSectionDto AddStructureSection(string description, string image)
        {
            StructureSectionDto structureSectionDto = new StructureSectionDto(description, image);
            structureSectionDto.structureElementList = new List<StructureElementDto>();
            structureSectionList.Add(structureSectionDto);
            return structureSectionDto;
        }

        public StructureSectionDto GetStructureSection(int index)
        {
            if (structureSectionList != null && structureSectionList.Count > index)
                return structureSectionList[index];
            else
                return null;
        }

        public bool IsLastStructureSection(int index)
        {
            if ((structureSectionList.Count - 1) == index)
                return true;
            else
                return false;
        }
    }
}