using System;

namespace Henspe.Core.Model.Dto
{
    public class StructureElementDto
	{
        public string id { get; set; }
		public int score { get; set; }
        public string image { get; set; }
        public string imageLeft { get; set; }
		public string description { get; set; }
        public string animation { get; set; }
        public bool selected { get; set; }
        public ElementType elementType;

        public enum ElementType
        {
            Single,
            LeftRight
        };

        public StructureElementDto(ElementType elementType, string id, int score, string image, string imageLeft, string description, string animation)
        {
            this.elementType = elementType;
            this.id = id;
            this.score = score;
            this.image = image;
            this.imageLeft = imageLeft;
            this.description = description;
            this.animation = animation;
            this.selected = false;
        }
	}
}