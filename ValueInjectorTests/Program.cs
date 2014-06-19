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
        public List<TemplateVersionDTO> TemplateVesionsDTOs { get; set; }
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

    public class TemplateVersionDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<SectionDomain> Secitons { get; set; }
    }

    public class SectionDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionDomain> Questions { get; set; }
    }

    public class QuestionDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion 
    

    public static class ViExt
    {
        public static List<TTo> InjectFrom<TFrom, TTo>(this List<TTo> to, params IEnumerable<TFrom>[] sources) where TTo : new()
        {
            if (to == null)
            {
                throw new ArgumentNullException("to : You must initialize the source collection.");
            }

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

    [TestFixture]
    public class CollectionTests
    {
         [Test]
        public void DtoToDomain_WhenCollectionIsNull_ConvertSuccessfully()
        {
           
            var templateDomain= new TemplateDomain()
            {
                Name = "template1",
                TemplateVesions = new List<TemplateVersionDomain>()
                {
                    new TemplateVersionDomain
                    {
                        Name = "version1"
                    }
                }
            };
            var templateDTO = new TemplateDTO(){};
            templateDTO.InjectFrom(templateDomain);

            templateDTO.TemplateVesionsDTOs.InjectFrom<TemplateVersionDomain, TemplateVersionDTO>(templateDomain.TemplateVesions);
            

            

            Assert.AreEqual("version1", templateDTO.TemplateVesionsDTOs.First().Name);
        }

        [Test]  
        public void ResoveNestedCollections_MapToSameEntity()
        {
            var originalTemplate = OriginalTemplate();
            var newTemplate = new TemplateDTO();

            newTemplate.InjectFrom(originalTemplate);

            Assert.AreEqual(originalTemplate.Name, newTemplate.Name);
            Assert.AreEqual(originalTemplate.TemplateType, newTemplate.TemplateType);

            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Name, newTemplate.TemplateVesionsDTOs.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Number, newTemplate.TemplateVesionsDTOs.ElementAt(0).Number);

            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Name, newTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Id, newTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Id);

            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name, newTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name);
            Assert.AreEqual(originalTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id, newTemplate.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id);
        }


       

        [Test]
        public void ResoveNestedCollections_MapToDifferentEntity()
        {
            var templateDTO = OriginalTemplate();
            var templateDomain = new TemplateDomain() {TemplateVesions = new List<TemplateVersionDomain>()};

            templateDomain.InjectFrom(templateDTO);
            templateDomain.InjectFrom<MapEnum>(new { TemplateType = templateDTO.TemplateType});
            templateDomain.TemplateVesions.InjectFrom<TemplateVersionDTO, TemplateVersionDomain>(templateDTO.TemplateVesionsDTOs);
            templateDomain.TemplateVesions.ForEach((v) =>
            {
                v.Secitons = new List<SectionDomain>();
                var versions = templateDTO.TemplateVesionsDTOs;

                foreach (var templateVersionDTO in versions)
                {
                    v.Secitons.InjectFrom<SectionDTO, SectionDomain>(templateVersionDTO.Secitons);
                    var sections = templateVersionDTO.Secitons;

                    foreach (var sectionDTO in sections)
                    {
                        v.Secitons.ForEach((s) =>
                        {
                            s.Questions = new List<QuestionDomain>();
                            s.Questions.InjectFrom<QuestionDTO, QuestionDomain>(sectionDTO.Questions);
                        });
                    }
                }
            });
            
            
            Assert.AreEqual(templateDTO.Name, templateDomain.Name);

            Assert.AreEqual(templateDTO.TemplateType, templateDomain.TemplateType);

            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Number, templateDomain.TemplateVesions.ElementAt(0).Number);

            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Id, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Id);

            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Name);
            Assert.AreEqual(templateDTO.TemplateVesionsDTOs.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id, templateDomain.TemplateVesions.ElementAt(0).Secitons.ElementAt(0).Questions.ElementAt(0).Id);
        }


        private static TemplateDTO OriginalTemplate()
        {
            return new TemplateDTO()
            {
                Id = 1,
                Name = "TestTemaplte",
                TemplateType = TemplateTypeDTO.Double,
                TemplateVesionsDTOs = new List<TemplateVersionDTO>
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
