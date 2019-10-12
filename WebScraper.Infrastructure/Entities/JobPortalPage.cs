﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebScraper.Infrastructure.Entities
{
    [Table("tblData_PortalPage")]
    public class JobPortalPage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Data { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public PortalCategory Category { get; set; }

    }
}
