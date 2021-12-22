using DoppelgangstersOnline.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoppelgangstersOnline.Services.Interfaces
{
    public interface IContentService
    {
        Task<string> GetContent(string page);
    }
}
