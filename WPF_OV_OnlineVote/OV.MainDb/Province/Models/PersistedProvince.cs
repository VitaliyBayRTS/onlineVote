using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OV.MainDb.Province.Models
{
    [Table("tblProvinces")]
    public class PersistedProvince
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }

    }

    public class ProvinceConfiguration : EntityConfiguration<PersistedProvince>
    {
        public override void Configure(EntityTypeBuilder<PersistedProvince> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
