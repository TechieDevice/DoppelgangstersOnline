using DoppelgangstersOnline.Database;
using DoppelgangstersOnline.Database.Models;
using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace DoppelgangstersOnline.Services
{
    public class ContentService : IContentService
    {
        private ApplicationContext _context;

        public ContentService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<string> GetContent(string page)
        {
            var content = new ContentDto();

            content = ToDto(await _context.Contents.FirstOrDefaultAsync(x => x.Page == page));

            if (content == null) return null;

            return content.Data;
        }

        private static ContentDto ToDto(Content content)
        {
            return new ContentDto
            {
                Id = content.Id,
                Page = content.Page,
                Data = content.Data
            };
        }
    }
}
