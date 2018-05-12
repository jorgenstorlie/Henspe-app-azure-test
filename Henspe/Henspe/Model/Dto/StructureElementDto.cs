using System;

namespace Henspe.Core.Model.Dto
{
    public class StructureElementDto
	{
		public ElementType elementType;
		public string description { get; set; }
        public string image { get; set; }
		public float percent { get; set; }

        public enum ElementType
        {
            Normal,
			Position,
            Address
        };

		public StructureElementDto(ElementType elementType, string description, string image, float percent)
        {
            this.elementType = elementType;
			this.description = description;
            this.image = image;
			this.percent = percent;
        }
	}
}