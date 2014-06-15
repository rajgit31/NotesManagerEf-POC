using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NotesDomain;
using NotesDomain.Entities;

namespace NotesManagerTransferEntities
{
    public static class Conversions
    {
        public static Note ConvertToDomain(this NoteDTO note)
        {
            return new Note
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                EntityState = ConvertState(note.EntityState),
                IsActive = note.IsActive,
                NoteVersions = note.NoteVersions.Select(x => new NoteVersion()
                {
                    Name = x.Name,
                    Version = x.Version,
                    EntityState = ConvertState(x.EntityState),
                    NoteSection = x.NoteSection.Select(dto => dto.ConvertToDomain()).ToList(),
                }).ToList()
            };
        }

        public static NoteDTO ConvertToDTO(this Note note)
        {
            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                EntityState = ConvertState(note.EntityState),
                IsActive = note.IsActive,
                NoteVersions = note.NoteVersions.Select(x => new NoteVersionDTO()
                {
                    Name = x.Name,
                    Version = x.Version,
                    EntityState = ConvertState(x.EntityState),
                    NoteSection = x.NoteSection.Select(domain => domain.ConvertToDTO()).ToList(),
                    
                }).ToList()
            };
        }

        public static NoteSection ConvertToDomain(this NoteSectionDTO noteSection)
        {
            return new NoteSection
            {
                Id = noteSection.Id,
                SectionColor = noteSection.SectionColor,
                SectionName = noteSection.SectionName,
                EntityState = ConvertState(noteSection.EntityState),
                IsActive = noteSection.IsActive,
            };
        }

        public static NoteSectionDTO ConvertToDTO(this NoteSection noteSection)
        {
            return new NoteSectionDTO
            {
                Id = noteSection.Id,
                SectionColor = noteSection.SectionColor,
                SectionName = noteSection.SectionName,
                EntityState = ConvertState(noteSection.EntityState),
                IsActive = noteSection.IsActive,
            };
        }

        public static EntityState ConvertState(EntityStateDTO state)
        {
            switch (state)
            {
                case EntityStateDTO.Added:
                    return EntityState.Added;
                case EntityStateDTO.Modified:
                    return EntityState.Modified;
                case EntityStateDTO.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        public static EntityStateDTO ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    return EntityStateDTO.Added;
                case EntityState.Modified:
                    return EntityStateDTO.Modified;
                case EntityState.Deleted:
                    return EntityStateDTO.Deleted;
                default:
                    return EntityStateDTO.Unchanged;
            }
        }
    }
}
