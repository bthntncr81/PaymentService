using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Configurations
{
    public class GoThereAppConfig
    {
        public JwtConfiguration JwtConfiguration { get; set; }
        public StaticUrls StaticUrls { get; set; }
        public LinkTokenConfiguration LinkTokenConfiguration { get; set; }
        public EmailConfiguration EmailConfiguration { get; set; }
    }
}
