﻿
//---------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//---------------------------------------------------

using NotesDomain.Entities;
using System;
namespace NotesManagerTransferEntities
{

			
	[Serializable]            
	public class NoteSectionDTO 
	{	
		public NoteSectionDTO() 
		{                    
		        NoteVersionDTO = new NoteVersionDTO();
	 
		}	
	
		                    
		public  System.String  SectionName { get; set; }
	                    
		public  System.String  SectionColor { get; set; }
	                    
		public  NoteVersionDTO  NoteVersionDTO { get; set; }
	                    
		public  System.Int32  FK_NoteVersionId { get; set; }
	                    
		public  NoteSectionTypeDTO  SectionTypeDTO { get; set; }
	                    
		public  Guid?  AddtionalId { get; set; }
	                    
		public  System.Int32  Id { get; set; }
	                    
		public  System.Boolean  IsActive { get; set; }
	                    
		public  System.DateTime  DateModified { get; set; }
	                    
		public  System.DateTime  DateCreated { get; set; }
	                    
		public  System.Guid  UserId { get; set; }
	                    
		public  EntityStateDTO  EntityStateDTO { get; set; }
	                    
		public  System.Boolean  MarkAsDeleted { get; set; }
	}   
	 		
	[Serializable]            
	public class NoteDTO 
	{	
		public NoteDTO() 
		{                    
			  NoteVersions = new System.Collections.Generic.List<NoteVersionDTO>();
							 
		}	
	
		                    
		public  System.String  Title { get; set; }
	                    
		public  System.String  Description { get; set; }
	                    
		public  System.Collections.Generic.List<NoteVersionDTO>  NoteVersions { get; set; }
	                    
		public  System.Int32  Id { get; set; }
	                    
		public  System.Boolean  IsActive { get; set; }
	                    
		public  System.DateTime  DateModified { get; set; }
	                    
		public  System.DateTime  DateCreated { get; set; }
	                    
		public  System.Guid  UserId { get; set; }
	                    
		public  EntityStateDTO  EntityStateDTO { get; set; }
	                    
		public  System.Boolean  MarkAsDeleted { get; set; }
	}   
	 		
	[Serializable]            
	public class NoteVersionDTO 
	{	
		public NoteVersionDTO() 
		{                    
		        NoteDTO = new NoteDTO();
	                    
			  NoteSection = new System.Collections.Generic.List<NoteSectionDTO>();
							 
		}	
	
		                    
		public  System.Int32  Version { get; set; }
	                    
		public  System.String  Name { get; set; }
	                    
		public  NoteDTO  NoteDTO { get; set; }
	                    
		public  System.Int32  FK_NoteId { get; set; }
	                    
		public  System.Collections.Generic.List<NoteSectionDTO>  NoteSection { get; set; }
	                    
		public  System.Int32  Id { get; set; }
	                    
		public  System.Boolean  IsActive { get; set; }
	                    
		public  System.DateTime  DateModified { get; set; }
	                    
		public  System.DateTime  DateCreated { get; set; }
	                    
		public  System.Guid  UserId { get; set; }
	                    
		public  EntityStateDTO  EntityStateDTO { get; set; }
	                    
		public  System.Boolean  MarkAsDeleted { get; set; }
	}   
	             

	public enum NoteSectionTypeDTO
	{
	   Full,
	  Half,
	  Quatar,
		}            

	public enum EntityStateDTO
	{
	   Unchanged,
	  Added,
	  Modified,
	  Deleted,
	  DeAttached,
		}    
}      
	
	
	

