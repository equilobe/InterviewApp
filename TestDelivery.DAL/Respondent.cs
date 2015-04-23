using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.DAL
{
    [MetadataType(typeof(Metadata.RespondentMetadata))]
    public partial class Respondent : TestDelivery.DAL.IRespondent
    {
    }
}
