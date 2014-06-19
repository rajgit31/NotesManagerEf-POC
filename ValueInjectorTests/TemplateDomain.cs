using System.Collections.Generic;

namespace ValueInjecterTests
{
    public class TemplateDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateTypeDomain TemplateType { get; set; }
        public List<TemplateVersionDomain> TemplateVesions { get; set; }
    }
}