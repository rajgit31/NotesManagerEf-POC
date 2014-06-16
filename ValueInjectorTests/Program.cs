using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace ValueInjecterTests
{
    public class MapEnum : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.SourceProp.Name == c.TargetProp.Name 
                && c.SourceProp.Type.IsEnum 
                && c.TargetProp.Type.IsEnum;
        }

        protected override object SetValue(ConventionInfo c)
        {
            return Enum.Parse(c.TargetProp.Type, c.SourceProp.Value.ToString());
        }
    }

    public static class ViExt
    {
        public static List<TTo> InjectFrom<TFrom, TTo>(this List<TTo> to, params IEnumerable<TFrom>[] sources) where TTo : new()
        {
            foreach (var from in sources)
            {
                foreach (var source in from)
                {
                    var target = new TTo();
                    target.InjectFrom(source);
                    to.Add(target);
                }
            }
            return to;
        }
    }

    #region DTO
    public enum TemplateTypeDTO
    {
        Single,
        Double,
    }

    public class TemplateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateTypeDTO TemplateType { get; set; }
        public List<TemplateVersionDTO> TemplateVesions { get; set; }
    }

    public class TemplateVersionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<SectionDTO> Secitons { get; set; } 
    }

    public class SectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; } 
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion 

    #region Domain
    public enum TemplateTypeDomain
    {
        Single,
        Double,
    }

    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateTypeDomain TemplateType { get; set; }
        public List<TemplateVersion> TemplateVesions { get; set; }
    }

    public class TemplateVersion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<Section> Secitons { get; set; }
    }

    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion 
    
    [TestFixture]
    public class CollectionTests
    {
        [Test]  
        public void ResoveNestedCollections_MapToSameEntity()
        {
            var originalTemplate = OriginalTemplate();
            var newTemplate = new TemplateDTO();

            newTemplate.InjectFrom(originalTemplate);

            Assert.AreEqual(originalTemplate.Name, newTemplate.Name);
            Assert.AreEqual(originalTemplate.TemplateType, newTemplate.TemplateType);

            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Name, newTemplate.TemplateVesions.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Number, newTemplate.TemplateVesions.ElementAt(0).Number);

            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Name, newTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Id, newTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Id);

            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name, newTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id, newTemplate.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id);
        }

        [Test]
        public void ResoveNestedCollections_MapToDifferentEntity()
        {
            var templateDTO = OriginalTemplate();
            var templateDomain = new Template {TemplateVesions = new List<TemplateVersion>()};

            templateDomain.InjectFrom(templateDTO);
            templateDomain.InjectFrom<MapEnum>(new { TemplateType = templateDTO.TemplateType});
            templateDomain.TemplateVesions.InjectFrom<TemplateVersionDTO, TemplateVersion>(templateDTO.TemplateVesions);
            templateDomain.TemplateVesions.ForEach((v) =>
            {
                v.Secitons = new List<Section>();
                var versions = templateDTO.TemplateVesions;

                foreach (var templateVersionDTO in versions)
                {
                    v.Secitons.InjectFrom<SectionDTO, Section>(templateVersionDTO.Secitons);
                    var sections = templateVersionDTO.Secitons;

                    foreach (var sectionDTO in sections)
                    {
                        v.Secitons.ForEach((s) =>
                        {
                            s.Questions = new List<Question>();
                            s.Questions.InjectFrom<QuestionDTO, Question>(sectionDTO.Questions);
                        });
                    }
                }
            });
            
            
            Assert.AreEqual(templateDTO.Name, templateDomain.Name);

            Assert.AreEqual(templateDTO.TemplateType, templateDomain.TemplateType);

            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Number, templateDomain.TemplateVesions.ElementAt(0).Number);

            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Id, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Id);

            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id);
        }


        private static TemplateDTO OriginalTemplate()
        {
            return new TemplateDTO()
            {
                Id = 1,
                Name = "TestTemaplte",
                TemplateType = TemplateTypeDTO.Double,
                TemplateVesions = new List<TemplateVersionDTO>
                {
                    new TemplateVersionDTO()
                    {
                        Id = 1,
                        Name = "TestTemplateVersion",
                        Number = 1,
                        Secitons = new List<SectionDTO>()
                        {
                            new SectionDTO()
                            {
                                Id = 1,
                                Name = "Section1",
                                Questions = new List<QuestionDTO>()
                                {
                                    new QuestionDTO() {Id = 1, Name = "Age?"},
                                    new QuestionDTO() {Id = 1, Name = "City?"},
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
