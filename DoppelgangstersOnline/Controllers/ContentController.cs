using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.Services;
using DoppelgangstersOnline.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoppelgangstersOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        private IContentService _contentService;
        public ContentController(IContentService service)
        {
            _contentService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Post(ContentDto contentDto)
        {
            string data = await _contentService.GetContent(contentDto.Page);

            return Json(data);
        }
    }
}
