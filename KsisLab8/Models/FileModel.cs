using KsisLab8.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KsisLab8.Models
{
    [Serializable]
    public class FileModel : BaseModel
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
