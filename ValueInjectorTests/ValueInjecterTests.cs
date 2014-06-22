using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace ValueInjecterTests
{
    public static class TemplateMappingExtensions
    {
        public static TemplateDomain ConvertToDomain(this TemplateDTO templateDTO)
        {
            var templateDomain = new TemplateDomain();

            //Map the template
            templateDomain.InjectFrom(templateDTO);
            templateDomain.InjectFrom<MapEnum>(new { TemplateType = templateDTO.TemplateType });

            //Map the version
            templateDomain.TemplateVersions.InjectFrom<TemplateVersionDTO, TemplateVersionDomain>(templateDTO.TemplateVersionsDTOs);

            //Map the sections
            templateDomain.TemplateVersions.ForEach(tv =>
            {
                //id is already mapped before, so we can get the matching dto for the domain.
                TemplateVersionDTO matchedTemplateVersionDTOs = templateDTO.TemplateVersionsDTOs.SingleOrDefault(x => x.Id == tv.Id);

                if (matchedTemplateVersionDTOs != null)
                {
                    var sectionDomains = tv.Sections.InjectFrom<SectionDTO, SectionDomain>(matchedTemplateVersionDTOs.Sections);

                    sectionDomains.ForEach(sd =>
                    {
                        SectionDTO sectionDTO = matchedTemplateVersionDTOs.Sections.SingleOrDefault(x => x.Id == sd.Id);

                        if (sectionDTO != null)
                        {
                            var sectionQuestion = (SectionQuestionDomain)sd.SectionQuestion.InjectFrom(sectionDTO.SectionQuestionDTO);
                            List<QuestionDomain> questionDomains = sectionQuestion.Questions.InjectFrom<QuestionDTO, QuestionDomain>(sectionDTO.SectionQuestionDTO.Questions);

                            questionDomains.ForEach(domain =>
                            {
                                QuestionDTO questionDTO = sectionDTO.SectionQuestionDTO.Questions.SingleOrDefault(x => x.Id == domain.Id);

                                if (questionDTO != null)
                                {
                                    domain.InjectFrom<MapEnum>(new { QuestionType = questionDTO.QuestionTypeDTO });
                                }
                            });
                        }

                    });
                }
            });

            return templateDomain;
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
        public TemplateDTO()
        {
            TemplateVersionsDTOs = new List<TemplateVersionDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateTypeDTO TemplateType { get; set; }
        public List<TemplateVersionDTO> TemplateVersionsDTOs { get; set; }
    }

    public class TemplateVersionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<SectionDTO> Sections { get; set; }
    }

    public class SectionDTO
    {
        public SectionDTO()
        {
            SectionQuestionDTO = new SectionQuestionDTO();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public SectionQuestionDTO SectionQuestionDTO { get; set; }
    }

    public class SectionQuestionDTO
    {
        public SectionQuestionDTO()
        {
            Questions = new List<QuestionDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionTypeDTO QuestionTypeDTO { get; set; }
    }

    public enum QuestionTypeDTO
    {
        Single,
        Multiple,
    }
    #endregion

    #region Domain
    public enum TemplateTypeDomain
    {
        Single,
        Double,
    }

    public class TemplateDomain
    {
        public TemplateDomain()
        {
            TemplateVersions = new List<TemplateVersionDomain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateTypeDomain TemplateType { get; set; }
        public List<TemplateVersionDomain> TemplateVersions { get; set; }
    }

    public class TemplateVersionDomain
    {
        public TemplateVersionDomain()
        {
            Sections = new List<SectionDomain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<SectionDomain> Sections { get; set; }
    }

    public class SectionDomain
    {
        public SectionDomain()
        {
            SectionQuestion = new SectionQuestionDomain();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public SectionQuestionDomain SectionQuestion { get; set; }
    }

    public class SectionQuestionDomain
    {
        public SectionQuestionDomain()
        {
            Questions = new List<QuestionDomain>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionDomain> Questions { get; set; }
    }

    public class QuestionDomain
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
    }

    public enum QuestionType
    {
        Single,
        Multiple,
    }
    #endregion

    [TestFixture]
    public class ValueInjecterTests
    {
        [Test]
        public void ResoveNestedCollections_MapToSameEntity()
        {
            var originalTemplateDTO = OriginalTemplate();
            var newTemplateDTO = new TemplateDTO();

            newTemplateDTO.InjectFrom(originalTemplateDTO);

            Assert.AreEqual(originalTemplateDTO.Name, newTemplateDTO.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateType, newTemplateDTO.TemplateType);

            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Name, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Number, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Number);

            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Name, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Id, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Id);

            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Name, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Id, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Id);

            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Text, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Id, newTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Id);
        }


        [Test]
        public void ResoveNestedCollections_MapToDifferentEntity_ConvertToDomain()
        {
            var originalTemplateDTO = OriginalTemplate();

            TemplateDomain templateDomain = originalTemplateDTO.ConvertToDomain();

            //Template Map
            Assert.AreEqual(originalTemplateDTO.Name, templateDomain.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateType.ToString(), templateDomain.TemplateType.ToString());

            //Nested collection: Template->TemplateVerion1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Name, templateDomain.TemplateVersions.ElementAt(0).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Number, templateDomain.TemplateVersions.ElementAt(0).Number);

            //Nested collection: Template->TemplateVerion2 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Name, templateDomain.TemplateVersions.ElementAt(1).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Number, templateDomain.TemplateVersions.ElementAt(1).Number);

            //Nested collection: Template->TemplateVerion1->Section1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Name, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).Id, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).Id);

            //Nested collection: Template->TemplateVerion2->Section1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).Name, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).Name, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).Id);


            //Nested collection: Template->TemplateVerion1->Section1-> SectionQuestion1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Name, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Id, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Id);

            //Nested collection: Template->TemplateVerion2->Section1-> SectionQuestion Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).SectionQuestionDTO.Name, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).SectionQuestion.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).SectionQuestionDTO.Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).SectionQuestion.Id);

            //Nested collection: Template->TemplateVerion2->Section2-> SectionQuestion Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Name, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Name);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Id);

            //TODO: questions
            //Nested collection: Template->TemplateVerion1->Section1-> SectionQuestion1->Question1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Text, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Id, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).QuestionTypeDTO.ToString(), templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).QuestionType.ToString());

            //Nested collection: Template->TemplateVerion1->Section1-> SectionQuestion1->Question2 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(1).Text, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(1).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(1).Id, templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(1).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(0).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(1).QuestionTypeDTO.ToString(), templateDomain.TemplateVersions.ElementAt(0).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(1).QuestionType.ToString());

            //Nested collection: Template->TemplateVerion2->Section1-> SectionQuestion1->Question1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Text, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(0).SectionQuestionDTO.Questions.ElementAt(0).QuestionTypeDTO.ToString(), templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(0).SectionQuestion.Questions.ElementAt(0).QuestionType.ToString());

            //Nested collection: Template->TemplateVerion2->Section2-> SectionQuestion1->Question1 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(0).Text, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(0).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(0).Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(0).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(0).QuestionTypeDTO.ToString(), templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(0).QuestionType.ToString());

            //Nested collection: Template->TemplateVerion2->Section2-> SectionQuestion1->Question2 Map
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(1).Text, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(1).Text);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(1).Id, templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(1).Id);
            Assert.AreEqual(originalTemplateDTO.TemplateVersionsDTOs.ElementAt(1).Sections.ElementAt(1).SectionQuestionDTO.Questions.ElementAt(1).QuestionTypeDTO.ToString(), templateDomain.TemplateVersions.ElementAt(1).Sections.ElementAt(1).SectionQuestion.Questions.ElementAt(1).QuestionType.ToString());

        }


        private static TemplateDTO OriginalTemplate()
        {
            return new TemplateDTO()
            {
                Id = 1,
                Name = "TestTemaplte",
                TemplateType = TemplateTypeDTO.Double,
                TemplateVersionsDTOs = new List<TemplateVersionDTO>
                {
                    new TemplateVersionDTO()
                    {
                        Id = 1,
                        Name = "TestTemplateVersion1",
                        Number = 1,
                        Sections = new List<SectionDTO>()
                        {
                            new SectionDTO()
                            {
                                Id = 1,
                                Name = "Sectionv1.1",
                                SectionQuestionDTO = new SectionQuestionDTO
                                {
                                    Id = 1,
                                    Name = "SectionQuestion1",

                                    Questions = new List<QuestionDTO>()
                                    {
                                        new QuestionDTO()
                                        {
                                            Id = 1000,
                                            Text = "Question1000?",
                                            QuestionTypeDTO = QuestionTypeDTO.Multiple
                                        },
                                        new QuestionDTO()
                                        {
                                            Id = 1001,
                                            Text = "Question1001?",
                                            QuestionTypeDTO = QuestionTypeDTO.Single
                                        }
                                    }
                                }
                            }
                        }
                    },

                    new TemplateVersionDTO()
                    {
                        Id = 2,
                        Name = "TestTemplateVersion2",
                        Number = 2,
                        Sections = new List<SectionDTO>()
                        {
                            new SectionDTO()
                            {
                                Id = 10,
                                Name = "Sectionv2.1",
                                SectionQuestionDTO = new SectionQuestionDTO
                                {
                                    Id = 100,
                                    Name = "SectionQuestion100",
                                    Questions = new List<QuestionDTO>()
                                    {
                                        new QuestionDTO()
                                        {
                                            Id = 2000,
                                            Text = "Question2000?",
                                            QuestionTypeDTO = QuestionTypeDTO.Single
                                        },
                                     }
                                }
                            },
                            new SectionDTO()
                            {
                                Id = 20,
                                Name = "Sectionv2.2",
                                SectionQuestionDTO = new SectionQuestionDTO
                                {
                                    Id = 101,
                                    Name = "SectionQuestion101",
                                    Questions = new List<QuestionDTO>()
                                    {
                                        new QuestionDTO()
                                        {
                                            Id = 3000,
                                            Text = "Question3000?",
                                            QuestionTypeDTO = QuestionTypeDTO.Multiple
                                        },
                                        new QuestionDTO()
                                        {
                                            Id = 3001,
                                            Text = "Question3001?",
                                            QuestionTypeDTO = QuestionTypeDTO.Single

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
