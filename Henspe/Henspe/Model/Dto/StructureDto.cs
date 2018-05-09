﻿using System;
using System.Collections.Generic;

namespace Henspe.Core.Model.Dto
{
    public class StructureDto
	{
        public List<StructureSectionDto> structureSectionList { get; set; }
        public int currentStructureSectionId { get; set; }

        public StructureDto()
        {
            structureSectionList = new List<StructureSectionDto>();
        }

        public StructureSectionDto AddStructureSection(StructureSectionDto.StepType stepType, string id, string name, string description, string alternateDescription, string video)
        {
            StructureSectionDto structureSectionDto = new StructureSectionDto(stepType, id, name, description, alternateDescription, video);
            structureSectionDto.structureElementList = new List<StructureElementDto>();
            structureSectionList.Add(structureSectionDto);
            return structureSectionDto;
        }

        public StructureSectionDto GetStructureSectionFromId(string id)
        {
            StructureSectionDto result = null;

            foreach(StructureSectionDto structureSectionDto in structureSectionList)
            {
                if(structureSectionDto.id == id)
                {
                    result = structureSectionDto;
                    break;
                }
            }

            return result;
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