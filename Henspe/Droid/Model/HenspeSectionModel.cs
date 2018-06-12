using System;

namespace Henspe.Core.Model.Dto
{
	public class HenspeSectionModel
    {
		public string image { get; set; }
        public string description { get; set; }

		public HenspeSectionModel(string image, string description)
        {
			this.image = image;
			this.description = description;
        }
    }
}