using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.Models.MainDb.Type;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Type.Models
{
    [Table("tblType")]
    public class PersistedType
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("Description")] public string Description { get; set; }
        [Column("Code")] public string Code { get; set; }

        public PersistedElection Election { get; set; }

        public TypeObject ToType()
        {
            return new TypeObject()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Code = Code,
                Election = Election?.ToElection()
            };
        }

    }
    public class TypeConfiguration : EntityConfiguration<PersistedType>
    {
        public override void Configure(EntityTypeBuilder<PersistedType> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
