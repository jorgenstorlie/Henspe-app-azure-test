using System;
using System.Collections.Generic;

namespace Henspe.Core.Model.Dto
{
    public class StructureSectionDto
	{
        public enum StepType
        {
            schemeStep,
            iktus,
            medicines,
            state,
        } 

        public StepType stepType { get; set; }
		public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string alternateDescription { get; set; }
        public string video { get; set; }
        public List<StructureElementDto> structureElementList { get; set; }

        public StructureSectionDto (StepType stepType, string id, string name, string description, string alternateDescription, string video)
		{
            this.stepType = stepType;
            this.id = id;
            this.name = name;
            this.description = description;
            this.alternateDescription = alternateDescription;
            this.video = video;
		}

        public void AddStructureElement(StructureElementDto.ElementType elementType, string id, int score, string image, string imageLeft, string description, string animate)
        {
            StructureElementDto structureElementDto = new StructureElementDto(elementType, id, score, image, imageLeft, description, animate);
            structureElementList.Add(structureElementDto);
        }
	}
}