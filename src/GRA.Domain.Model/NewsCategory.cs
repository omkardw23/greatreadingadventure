﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRA.Domain.Model
{
    public class NewsCategory : Abstract.BaseDomainEntity
    {
        public int SiteId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [DisplayName("Display in sidebar")]
        public bool DisplayInSidebar { get; set; }

        public bool IsDefault { get; set; }

        public int PostCount { get; set; }
    }
}